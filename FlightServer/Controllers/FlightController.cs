using System.Collections.Generic;
using System.Threading.Tasks;
using FlightServer.Infra;
using Microsoft.AspNetCore.Mvc;
using Shared.Lib.Models;

namespace FlightServer.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class FlightController : ControllerBase
    {
        private readonly IFlightService _flightService;
        public FlightController(IFlightService flightService)
        {
            _flightService = flightService;

        }

        [HttpPost("GetAllFlightsHistory")]
        public async Task<IList<FlightHistory>> GetAllFlightsHistory()
        {
            return await _flightService.GetAllFlights();
        }

        [HttpPost("GetAllPlannedFlights")]
        public async Task<IList<PlannedFlights>> GetAllPlannedFlights()
        {
            return await _flightService.GetAllPlannedFlights();
        }

        [HttpPost("GetAllPlannedLandings")]
        public async Task<IList<PlannedLanding>> GetAllPlannedLandings()
        {
            return await _flightService.GetAllPlannedLandings();
        }
    }
}
