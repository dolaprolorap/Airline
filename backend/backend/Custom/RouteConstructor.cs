using backend.DataAccess.Repository;
using backend.ServerResponse.Custom;
using Route = backend.Models.DB.Route;

namespace backend.Custom
{
    public class RouteConstructor
    {
        IUnitOfWork _unit;

        public RouteConstructor(IUnitOfWork unit) 
        { 
            _unit = unit;
        }

        public RouteConstructorResponse ConstructPath(
            int fromAirportId, 
            int toAirportId, 
            out IEnumerable<IEnumerable<Route>>? path) 
        {
            var allRoutes = _unit.RouteRepo.ReadAll();

            var fromAirport = _unit.AirportRepo.ReadWhere(airport => airport.Id == fromAirportId).First();
            var toAirport = _unit.AirportRepo.ReadWhere(airport => airport.Id == toAirportId).First();

            if (fromAirport == null)
            {
                path = null;
                return new RouteConstructorResponse(
                    RouteConstructorResponseType.FromAirportIdNotExists,
                    fromAirportId: fromAirportId);
            }

            if (toAirport == null)
            {
                path = null;
                return new RouteConstructorResponse(
                    RouteConstructorResponseType.ToAirportIdNotExists,
                    toAirportId: toAirportId);
            }

            path = new List<List<Route>>();

            _dfs(allRoutes, fromAirportId, toAirportId, new List<Route>(), path);

            return new RouteConstructorResponse(RouteConstructorResponseType.Ok);
        }

        private void _dfs(
            IEnumerable<Route> allRoutes, 
            int currentRouteId, 
            int finishId, 
            IEnumerable<Route> prevRoutes, 
            IEnumerable<IEnumerable<Route>> foundRoutes)
        {
            var init = allRoutes.Where(r => r.DepartureAirportId == currentRouteId);
            foreach (var ri in init)
            {
                if (prevRoutes.Contains(ri)) continue;

                if (ri.ArrivalAirportId == finishId)
                {
                    var foundRoute = new List<Route>(prevRoutes);
                    foundRoute.Append(ri);
                    foundRoutes.Append(foundRoute);
                }
                else
                {
                    var newPrevRoutes = new List<Route>(prevRoutes)
                    {
                        ri
                    };
                    _dfs(allRoutes, ri.ArrivalAirportId, finishId, newPrevRoutes, foundRoutes);
                }
            }
        }
    }
}
