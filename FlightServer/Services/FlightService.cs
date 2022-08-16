using FlightServer.DAL.Repositories;
using FlightServer.Infra;
using Shared.Lib.Infra;
using Shared.Lib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;

namespace FlightServer.Services
{
    public class FlightService : IFlightService
    {
        private readonly IFlightRepository _flightReposiory;
        Timer timer;
        LinkedList<IPlanned> WaitingList;
        private IPlanned planned;

        public FlightService(IFlightRepository flightReposiory,IMockRepository mockRepository=null)
        {
            _flightReposiory = flightReposiory;
            timer = new Timer(5000);
            timer.Elapsed += Timer_Elapsed;
            WaitingList = new LinkedList<IPlanned>();
        }

        public async Task<IList<FlightHistory>> GetAllFlights()
        {
            return await _flightReposiory.GetAllFlightsHistory();
        }

        public async Task<IList<PlannedFlights>> GetAllPlannedFlights()
        {
            return await _flightReposiory.GetAllPlannedFlights();
        }

        public async Task<IList<PlannedLanding>> GetAllPlannedLandings()
        {
            return await _flightReposiory.GetAllPlannedLandings();
        }


        public async Task CreatePlannedFlight()
        {
            Flight flight = await _flightReposiory.CreateFlight();
            PlannedFlights plannedFlights = new PlannedFlights() { FlightId = flight.Id, SourceStationId = 10, DestinationStationId = "6,7" };
            bool IsEndStation = _flightReposiory.CreatePlannedFlight(plannedFlights, out planned);
            WaitingList.AddLast(planned);
            if (!IsEndStation)
                Wait(IsEndStation);
        }

        public async Task CreatePlannedLandings()
        {
            Flight flight = await _flightReposiory.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = flight.Id, SourceStationId = 9, DestinationStationId = "1" };
            bool IsEndStation = _flightReposiory.CreatePlannedLanding(plannedLanding, out planned);
            Wait(IsEndStation);
        }

        private void Wait(bool IsEndStation)
        {
            WaitingList.AddLast(planned);
            if (!IsEndStation)
                timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
            LinkedListNode<IPlanned> x = WaitingList.First;
            if (WaitingList.Count > 0)
            {
                planned = WaitingList.First.Value;
                WaitingList.RemoveFirst();
                bool IsEndStation = _flightReposiory.CheckIfStationEmpty(x.Value, out planned);
                Wait(IsEndStation);
            }
        }

        public async Task<List<StatusStation>> GetStations()
        {
            return await _flightReposiory.GetStations();
        }
    }
}
