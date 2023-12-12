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

            return new StatusResponse(StatusResponseType.Success, "", "", scheduleFilter);
        }

        public StatusResponse SetActiveFlight(SetActiveFlight setActiveFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == setActiveFlight.FlightId);
            if (flight == null)
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no schedule with that id: " + setActiveFlight.FlightId,
                    "There is no schedule with that id: " + setActiveFlight.FlightId);

            flight.Confirmed = setActiveFlight.ActiveState;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success);
        }

        public StatusResponse EditFlight(EditFlight editFlight)
        {
            var flight = _unit.ScheduleRepo.ReadFirst(s => s.Id == editFlight.Id);
            if (flight == null)
            {
                return new StatusResponse(StatusResponseType.UserFail,
                    "There is no flight with id: " + editFlight,
                    "There is no flight with id: " + editFlight);
            }

            flight.Date = editFlight.Date;
            flight.Time = editFlight.Time;
            flight.EconomyPrice = editFlight.EconomyPrice;

            _unit.ScheduleRepo.Update(flight);
            _unit.Save();

            return new StatusResponse(StatusResponseType.Success);
        }

        public StatusResponse EditFlightsByCsv(string csv)
        {
            string[] recordsStr = csv.Split('\n');

            int successUpdates = 0;
            int duplicates = 0;
            int failedConverts = 0;

            List<int> appliedUpdates = new List<int>();

            foreach (var recordStr in recordsStr)
            {
                ScheduleUpdateRecord? record;
                if (_convertScheduleRecordService.ScheduleUpdateRecordFromCsv(recordStr, out record))
                {
                    if (!appliedUpdates.Contains(record!.FlightId))
                    {
                        var status = _applyUpdateRecord(record);
                        if (status.Status == StatusResponseType.Success)
                        {
                            successUpdates++;
                            appliedUpdates.Add(record!.FlightId);
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
                FlightNumber = record.FlightId.ToString()
            };

            if (record.Type == ScheduleUpdateRecordType.Add)
            {
                _unit.ScheduleRepo.Add(newSchedule);
            }
            else if (record.Type == ScheduleUpdateRecordType.Edit)
            {
                _unit.ScheduleRepo.Update(newSchedule);
            }

            _unit.Save();

            return new StatusResponse();
        }
    }
}
