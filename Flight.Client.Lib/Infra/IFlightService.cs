using Shared.Lib.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flight.Client.Lib.Infra
{
    public interface IFlightService
    {
        Task<IList<FlightHistory>> GetHistory();
    }
}
