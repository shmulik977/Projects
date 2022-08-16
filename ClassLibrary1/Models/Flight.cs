using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shared.Lib.Models
{
    public class Flight
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int CurrentStationId { get; set; }
        public List<FlightHistory> FlightsHistory { get; set; }
        public List<PlannedFlights> PlannedFlights { get; set; }
        public List<PlannedLanding> PlannedLandings { get; set; }
        [ForeignKey("CurrentStationId")]
        public StatusStation StatusStation { get; set; }
    }
}
