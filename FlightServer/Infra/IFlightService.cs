using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.Infra
{
    public interface IFlightService
    {
        Task<IList<FlightHistory>> GetAllFlights();
        Task<IList<PlannedFlights>> GetAllPlannedFlights();
        Task<IList<PlannedLanding>> GetAllPlannedLandings();
        Task CreatePlannedFlight();
        Task CreatePlannedLandings();
        Task<List<StatusStation>> GetStations();
    }
}
