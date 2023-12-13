using backend.DataAccess.Repository;
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
                        "There is no From Airport with that name: " + scheduleFilter.FromAirportName,
                        "There is no From Airport with that name: " + scheduleFilter.FromAirportName);
                }
            }

            if (scheduleFilter.ToAirportName != null)
            {
                toAirport = _unit.AirportRepo.ReadFirst(airport => airport.Name == scheduleFilter.ToAirportName);

                if (toAirport == null)
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "There is no To Airport with that name: " + scheduleFilter.ToAirportName,
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
                return new StatusResponse(StatusResponseType.Success, "", "", new List<ScheduleRecord>());
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
                        "Invalid DateOnly: " + scheduleFilter.Outbound,
                        "Invalid DateOnly: " + scheduleFilter.Outbound);
                }
            }

            var flights = _unit.ScheduleRepo.ReadWhere(
                flight =>
                (outbound == null || flight.Date == outbound) &&
                routesIds.Contains(flight.Id)
            );

            _unit.AirportRepo.ReadWhere(a => true).Load();
            _unit.AircraftRepo.ReadWhere(a => true).Load();

            List<ScheduleRecord> scheduleRecords = new List<ScheduleRecord>();
            foreach (var flight in flights)
            {
                scheduleRecords.Add(new ScheduleRecord()
                {
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

            return new StatusResponse(StatusResponseType.Success, "", "", scheduleRecords);
        }

        public StatusResponse SetActiveFlight(SetActiveFlight setActiveFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.FlightNumber == setActiveFlight.FlightNumber.ToString());
            if (flight == null)
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no schedule with that flight number: " + setActiveFlight.FlightNumber,
                    "There is no schedule with that flight number: " + setActiveFlight.FlightNumber);

            flight.Confirmed = setActiveFlight.ActiveState;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success);
        }

        public StatusResponse EditFlight(EditFlight editFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.FlightNumber == editFlight.FlightNumber.ToString());
            if (flight == null)
            {
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no flight with flight number: " + editFlight.FlightNumber,
                    "There is no flight with flight number: " + editFlight.FlightNumber);
            }

            if (!DateOnly.TryParseExact(editFlight.Date, "yyyy-MM-dd", out DateOnly date))
            {
                DateOnly.Parse(editFlight.Date);
                return new StatusResponse(StatusResponseType.UserFail,
                    "Invalid DateOnly string: " + editFlight.Date,
                    "Invalid DateOnly string: " + editFlight.Date);
            }

            if (!TimeOnly.TryParseExact(editFlight.Time, "HH:mm", out TimeOnly time))
            {
                return new StatusResponse(StatusResponseType.UserFail,
                    "Invalid DateOnly string: " + editFlight.Date,
                    "Invalid DateOnly string: " + editFlight.Date);
            }

            flight.Date = date;
            flight.Time = time;
            flight.EconomyPrice = editFlight.EconomyPrice;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success);
        }

        public StatusResponse EditFlightsByCsv(string csv)
        {
            string[] recordsStr = csv.Split("\r\n");

            int successUpdates = 0;
            int duplicates = 0;
            int failedConverts = 0;

            List<int> appliedUpdates = new List<int>();

            foreach (var recordStr in recordsStr)
            {
                ScheduleUpdateRecord? record;
                if (_convertScheduleRecordService.ScheduleUpdateRecordFromCsv(recordStr, out record))
                {
                    if (!appliedUpdates.Contains(record!.FlightNumber))
                    {
                        var status = _applyUpdateRecord(record);
                        if (status.Status == StatusResponseType.Success)
                        {
                            successUpdates++;
                            appliedUpdates.Add(record!.FlightNumber);
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

            return new StatusResponse(StatusResponseType.Success, "", "", new
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

            if (record.Type == ScheduleUpdateRecordType.Add)
            {
                if (_unit.ScheduleRepo.ReadFirst(s => s.FlightNumber == newSchedule.FlightNumber) == null)
                {
                    _unit.ScheduleRepo.Add(newSchedule);
                }
                else
                {
                    return new StatusResponse(StatusResponseType.UserFail,
                        "Record with this flight number already exists: " + newSchedule.FlightNumber,
                        "Record with this flight number already exists: " + newSchedule.FlightNumber);
                }
            }
            else if (record.Type == ScheduleUpdateRecordType.Edit)
            {
                var schedule = _unit.ScheduleRepo.ReadFirst(s => s.FlightNumber == newSchedule.FlightNumber);
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
                        "There is no record with this flight number: " + newSchedule.FlightNumber,
                        "There is no record with this flight number: " + newSchedule.FlightNumber);
                }
            }

            _unit.Save();

            return new StatusResponse();
        }
    }
}
