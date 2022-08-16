using Flight.Client.Lib.Infra;
using Shared.Lib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flight.Client.Services
{
    public class FlightService : IFlightService
    {
        private readonly IHttpService _httpService;
        public FlightService(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<IList<FlightHistory>> GetHistory()
        {
            return await _httpService.PostAsync<FlightHistory, IList<FlightHistory>>("/flight/GetAllFlightsHistory",null);
        }
    }
}
