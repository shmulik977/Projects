using Shared.Lib.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Lib.Models
{
    public class PlannedLanding : IPlanned
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int SourceStationId { get; set; }
        public string DestinationStationId { get; set; }
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }
        [ForeignKey("SourceStationId")]
        public StatusStation StatusStationSource { get; set; }

    }
}
