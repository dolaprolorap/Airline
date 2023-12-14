namespace backend.Models.API
{
    public class ManyFlightsData
    {
        public List<FlightData> Flights { get; set; } = new List<FlightData>();
        public int NumberOfStops => Flights.Count;
    }
}
