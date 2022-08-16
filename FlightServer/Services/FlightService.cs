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
        Timer _timer;
        LinkedList<IPlanned> _waitingList;
        private IPlanned _planned;

        public FlightService(IFlightRepository flightReposiory)
        {
            _flightReposiory = flightReposiory;
            _timer = new Timer(5000);
            _timer.Elapsed += Timer_Elapsed;
            _waitingList = new LinkedList<IPlanned>();
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

        public async Task<List<StatusStation>> GetStations()
        {
            return await _flightReposiory.GetStations();
        }

        public async Task CreatePlannedFlight()
        {
            var flight = await _flightReposiory.CreateFlight();
            var plannedFlights = new PlannedFlights() { FlightId = flight.Id, SourceStationId = 10, DestinationStationId = "6,7" };
            bool isEndStation = _flightReposiory.CreatePlannedFlight(plannedFlights, out _planned);
            _waitingList.AddLast(_planned);
            if (!isEndStation)
                Wait(isEndStation);
        }

        public async Task CreatePlannedLandings()
        {
            var flight = await _flightReposiory.CreateFlight();
            var plannedLanding = new PlannedLanding() { FlightId = flight.Id, SourceStationId = 9, DestinationStationId = "1" };
            bool isEndStation = _flightReposiory.CreatePlannedLanding(plannedLanding, out _planned);
            Wait(isEndStation);
        }

        private void Wait(bool IsEndStation)
        {
            _waitingList.AddLast(_planned);
            if (!IsEndStation)
                _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            var flights = _waitingList.First;
            if (_waitingList.Count > 0)
            {
                _planned = _waitingList.First.Value;
                _waitingList.RemoveFirst();
                bool IsEndStation = _flightReposiory.CheckIfStationEmpty(flights.Value, out _planned);
                Wait(IsEndStation);
            }
        }
    }
}