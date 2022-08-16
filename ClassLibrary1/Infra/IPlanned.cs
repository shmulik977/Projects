using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Lib.Infra
{
    public interface IPlanned
    {
        int Id { get; set; }
        int FlightId { get; set; }
        int SourceStationId { get; set; }
        string DestinationStationId { get; set; }
        StatusStation StatusStationSource { get; set; }

    }
}
