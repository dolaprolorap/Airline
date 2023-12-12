using backend.Models.API;

namespace backend.Services
{
    public class ConvertScheduleRecordService : IConvertScheduleRecordService
    {
        private readonly Dictionary<string, ScheduleUpdateRecordType> confirmityTypes = 
            new Dictionary<string, ScheduleUpdateRecordType>()
        {
            { "ADD", ScheduleUpdateRecordType.Add },
            { "EDIT", ScheduleUpdateRecordType.Edit },
        };

        private readonly Dictionary<string, bool> confirmityActive = 
            new Dictionary<string, bool>()
        {
            { "CANCELED", false },
            { "OK", true },
        };

        private readonly Dictionary<string, int> pos =
            new Dictionary<string, int>()
        {
            { "Type", 0 },
            { "Date", 1 },
            { "Time", 2 },
            { "FlightId", 3 },
            { "From", 4 },
            { "To", 5 },
            { "AircraftId", 6 },
            { "Price", 7 },
            { "Active", 8 },
        };

        public bool ScheduleUpdateRecordFromCsv(string csv, out ScheduleUpdateRecord? record)
        {
            string[] vals = csv.Split(',');
            if (vals.Length != pos.Count) 
            {
                record = null;
                return false;
            }

            record = new ScheduleUpdateRecord();

            if (confirmityTypes.ContainsKey(vals[pos["Type"]])) 
            { 
                record.Type = confirmityTypes[vals[pos["Type"]]];
            }
            else
            {
                record = null;
                return false;
            }

            try
            {
                DateOnly date = DateOnly.ParseExact(vals[pos["Date"]], "yyyy-MM-dd", 
                    System.Globalization.CultureInfo.InvariantCulture);
                TimeOnly time = TimeOnly.ParseExact(vals[pos["Time"]], "HH:mm",
                    System.Globalization.CultureInfo.InvariantCulture);

                record.Date = date;
                record.Time = time;
            }
            catch (FormatException)
            {
                record = null;
                return false;
            }

            if (int.TryParse(vals[pos["FlightId"]], out int flightId))
            {
                record.FlightId = flightId;
            }
            else
            {
                record = null;
                return false;
            }

            record.FromAirportCode = vals[pos["From"]];
            record.ToAirportCode = vals[pos["To"]];

            if (int.TryParse(vals[pos["AircraftId"]], out int aircraftId))
            {
                record.AircraftId = aircraftId;
            }
            else
            {
                record = null;
                return false;
            }

            if (int.TryParse(vals[pos["Price"]], out int price))
            {
                record.EconomyPrice = price;
            }
            else
            {
                record = null;
                return false;
            }

            if (confirmityActive.ContainsKey(vals[pos["Active"]]))
            {
                record.IsActive = confirmityActive[vals[pos["Active"]]];
            }
            else
            {
                record = null;
                return false;
            }

            return true;
        }
    }
}
