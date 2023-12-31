﻿using backend.DataAccess.Repository;
using backend.Models.API;
using backend.Models.DB;
using backend.ServerResponse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class ScheduleService : IScheduleService
    {
        private IUnitOfWork _unit;
        private IConvertScheduleRecordService _convertScheduleRecordService;

        public ScheduleService(IUnitOfWork unit, IConvertScheduleRecordService convertScheduleRecordService)
        {
            _unit = unit;
            _convertScheduleRecordService = convertScheduleRecordService;
        }

        public StatusResponse GetSchedule(ScheduleFilter scheduleFilter)
        {
            Airport? toAirport = null;
            Airport? fromAirport = null;

            if (scheduleFilter.FromAirportName != null)
            {
                fromAirport = _unit.AirportRepo.ReadFirst(airport => airport.Name == scheduleFilter.FromAirportName);

                if (fromAirport == null)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "FromAirportNotFound",
                        "There is no From Airport with that name: " + scheduleFilter.FromAirportName);
                }
            }

            if (scheduleFilter.ToAirportName != null)
            {
                toAirport = _unit.AirportRepo.ReadFirst(airport => airport.Name == scheduleFilter.ToAirportName);

                if (toAirport == null)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "ToAirportNotFound",
                        "There is no To Airport with that name: " + scheduleFilter.ToAirportName);
                }
            }

            var routes = _unit.RouteRepo.ReadWhere(
                route =>
                (fromAirport == null || route.DepartureAirportId == fromAirport.Id) &&
                (toAirport == null || route.ArrivalAirportId == toAirport.Id)
            );
            if (!routes.Any())
            {
                return new StatusResponse(StatusResponseType.Success, "RoutesFound", "", new List<ScheduleRecord>());
            }
            routes.Load();
            var routesIds = routes.Select(route => route.Id).ToList();

            DateOnly? outbound = null;
            if (scheduleFilter.Outbound != null)
            {
                try
                {
                    outbound = DateOnly.ParseExact(scheduleFilter.Outbound, "dd-MM-yyyy",
                        System.Globalization.CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "InvalidDate",
                        "Invalid DateOnly: " + scheduleFilter.Outbound);
                }
            }

            var flights = _unit.ScheduleRepo.ReadWhere(
                flight =>
                (outbound == null || flight.Date == outbound) &&
                (scheduleFilter.FlightNumber == null || flight.FlightNumber == scheduleFilter.FlightNumber) &&
                routesIds.Contains(flight.RouteId)
            );

            _unit.AirportRepo.ReadWhere(a => true).Load();
            _unit.AircraftRepo.ReadWhere(a => true).Load();

            List<ScheduleRecord> scheduleRecords = new List<ScheduleRecord>();
            foreach (var flight in flights)
            {
                scheduleRecords.Add(new ScheduleRecord()
                {
                    Id = flight.Id,
                    Date = flight.Date,
                    Time = flight.Time,
                    FromAirportName = flight.Route.DepartureAirport.Name!,
                    ToAirportName = flight.Route.ArrivalAirport.Name!,
                    FlightNumber = flight.FlightNumber!,
                    Aircraft = flight.Aircraft.Name,
                    EconomyPrice = (int)flight.EconomyPrice,
                    IsActive = flight.Confirmed
                });
            }

            return new StatusResponse(StatusResponseType.Success, "RoutesFound", "", scheduleRecords);
        }

        public StatusResponse SetActiveFlight(SetActiveFlight setActiveFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == setActiveFlight.ScheduleId);
            if (flight == null)
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "ScheduleNotFound",
                    "There is no schedule with that id: " + setActiveFlight.ScheduleId);

            flight.Confirmed = setActiveFlight.ActiveState;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(
                StatusResponseType.Success, 
                "ScheduleActiveStateChanged",
                "Schedule active state changed"
                );
        }

        public StatusResponse EditFlight(EditFlight editFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == editFlight.ScheduleId);
            if (flight == null)
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "ScheduleNotFound",
                    "There is no schedule with id: " + editFlight.ScheduleId);
            }

            if (!DateOnly.TryParseExact(editFlight.Date, "yyyy-MM-dd", out DateOnly date))
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "InvalidDate",
                    "Invalid DateOnly string: " + editFlight.Date);
            }

            if (!TimeOnly.TryParseExact(editFlight.Time, "HH:mm", out TimeOnly time))
            {
                return new StatusResponse(
                    StatusResponseType.UserFail,
                    "InvalidTime",
                    "Invalid TimeOnly string: " + editFlight.Date);
            }

            flight.Date = date;
            flight.Time = time;
            flight.EconomyPrice = editFlight.EconomyPrice;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(
                StatusResponseType.Success,
                "ScheduleEdited",
                "Schedule data edited");
        }

        public StatusResponse EditFlightsByCsv(string csv)
        {
            string[] recordsStr = csv.Split("\r\n");

            int successUpdates = 0;
            int duplicates = 0;
            int failedConverts = 0;

            List<Tuple<int, DateOnly>> appliedUpdates = new List<Tuple<int, DateOnly>>();

            foreach (var recordStr in recordsStr)
            {
                ScheduleUpdateRecord? record;
                if (_convertScheduleRecordService.ScheduleUpdateRecordFromCsv(recordStr, out record))
                {
                    var dayFlightNumber = new Tuple<int, DateOnly>(record.FlightNumber, record.Date);
                    if (!appliedUpdates.Contains(dayFlightNumber))
                    {
                        var status = _applyUpdateRecord(record);
                        if (status.Status == StatusResponseType.Success)
                        {
                            successUpdates++;
                            appliedUpdates.Add(dayFlightNumber);
                        }
                        else
                        {
                            failedConverts++;
                        }
                    }
                    else
                    {
                        duplicates++;
                    }
                }
                else
                {
                    failedConverts++;
                }
            }

            return new StatusResponse(StatusResponseType.Success, "FlightsEdited", "", new
            {
                Success = successUpdates,
                Duplicates = duplicates,
                Fails = failedConverts
            });
        }

        private StatusResponse _applyUpdateRecord(ScheduleUpdateRecord record)
        {
            _unit.AirportRepo.ReadWhere(airport => airport.Iatacode == record.FromAirportCode ||
                airport.Iatacode == record.ToAirportCode).Load();
            var route = _unit.RouteRepo.ReadFirst(route => route.DepartureAirport.Iatacode == record.FromAirportCode ||
                route.ArrivalAirport.Iatacode == record.ToAirportCode);

            if (route == null)
            {
                return new StatusResponse(StatusResponseType.UserFail,
                    string.Format("There is no such route: {0} - {1}", record.FromAirportCode, record.ToAirportCode),
                    string.Format("There is no such route: {0} - {1}", record.FromAirportCode, record.ToAirportCode));
            }

            int lastId = 0;
            if (_unit.ScheduleRepo.ReadAll().Count() != 0)
                lastId = _unit.ScheduleRepo.Max(s => s.Id);

            var newSchedule = new Schedule
            {
                Id = lastId + 1,
                Date = record.Date,
                Time = record.Time,
                AircraftId = record.AircraftId,
                RouteId = route.Id,
                EconomyPrice = record.EconomyPrice,
                Confirmed = record.IsActive,
                FlightNumber = record.FlightNumber.ToString()
            };

            var schedule = _unit.ScheduleRepo.ReadFirst(
                s => s.FlightNumber == newSchedule.FlightNumber &&
                s.Date == newSchedule.Date);

            if (record.Type == ScheduleUpdateRecordType.Add)
            {
                if (schedule == null)
                {
                    _unit.ScheduleRepo.Add(newSchedule);
                }
                else
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "Record with this flight number already exists: " + newSchedule.FlightNumber + " date: " + newSchedule.Date,
                        "Record with this flight number already exists: " + newSchedule.FlightNumber + " date: " + newSchedule.Date);
                }
            }
            else if (record.Type == ScheduleUpdateRecordType.Edit)
            {
                if (schedule != null)
                {
                    schedule.Date = newSchedule.Date;
                    schedule.Time = newSchedule.Time;
                    schedule.AircraftId = newSchedule.AircraftId;
                    schedule.RouteId = newSchedule.RouteId;
                    schedule.EconomyPrice = newSchedule.EconomyPrice;
                    schedule.Confirmed = newSchedule.Confirmed;
                    schedule.FlightNumber = newSchedule.FlightNumber;
                    _unit.ScheduleRepo.Update(schedule);
                }
                else
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "There is no record with this flight number: " + newSchedule.FlightNumber + " date: " + newSchedule.Date,
                        "There is no record with this flight number: " + newSchedule.FlightNumber + " date: " + newSchedule.Date);
                }
            }

            _unit.Save();

            return new StatusResponse();
        }
    }
}
