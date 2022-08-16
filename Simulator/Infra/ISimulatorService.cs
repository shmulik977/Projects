using System.Collections.Generic;
using System.Threading.Tasks;
using Shared.Lib.Models;

namespace Simulator.Infra
{
    public interface ISimulatorService
    {
        Task LandingFlight();
        Task PlaneTakingOff();
        Task<List<StatusStation>> GetStations();
    }
}
