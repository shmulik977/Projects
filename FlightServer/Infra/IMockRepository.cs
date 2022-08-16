using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.Infra
{
    public interface IMockRepository
    {
        List<Flight> Flights { get; set; }
        List<FlightHistory> FlightsHistory { get; set; }
        List<PlannedFlights> PlannedFlights { get; set; }
        List<PlannedLanding> PlannedLandings { get; set; }
        List<StatusStation> StatusStations { get; set; }
        Task<List<StatusStation>> GetStations();
        bool CreatePlannedLanding(PlannedLanding plannedLanding, out IPlanned planned);
        Flight CreateFlight();
        bool CreatePlannedFlight(PlannedFlights plannedFlights, out IPlanned planned);
        bool CheckIfStationEmpty(IPlanned planned, out IPlanned UpdatePlanned);
        StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation);
        StatusStation FindStation(int stationId);
        Task<IList<FlightHistory>> GetAllFlightsHistory();
        Task<IList<PlannedFlights>> GetAllPlannedFlights();
        Task<IList<PlannedLanding>> GetAllPlannedLandings();
    }
}
