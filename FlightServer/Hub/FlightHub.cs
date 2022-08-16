using FlightServer.Infra;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace FlightServer.Hub
{
    public class FlightHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly IFlightService _flightService;
        public FlightHub(IFlightService flightService)
        {
            _flightService = flightService;
        }
        public async Task CreateLanding()
        {
            await _flightService.CreatePlannedLandings();
        }
        public async Task CreateFlight()
        {
           await _flightService.CreatePlannedFlight();
        }
        public async Task GetStations()
        {
            await Clients.Client(Context.ConnectionId).SendAsync("GotStations", await _flightService.GetStations());
        }
    }
}
