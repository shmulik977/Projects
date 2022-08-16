using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.Infra
{
    public interface IFlightRepository
    {
        Task<List<StatusStation>> GetStations();
        bool CreatePlannedLanding(PlannedLanding plannedLanding, out IPlanned planned);
        Task<Flight> CreateFlight();
        bool CreatePlannedFlight(PlannedFlights plannedFlights, out IPlanned planned);
        bool CheckIfStationEmpty(IPlanned planned, out IPlanned UpdatePlanned);
        StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation);
        Task<IList<FlightHistory>> GetAllFlightsHistory();
        Task<IList<PlannedFlights>> GetAllPlannedFlights();
        Task<IList<PlannedLanding>> GetAllPlannedLandings();
    }
}
