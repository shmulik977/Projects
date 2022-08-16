using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Lib.Models
{
    public class FlightHistory
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int FlightId { get; set; }
        public int StationId { get; set; }
        public DateTime? EntringTime { get; set; }
        public DateTime? ExitTime { get; set; }
        [ForeignKey("StationId")]
        public StatusStation StatusStation { get; set; }
        [ForeignKey("FlightId")]
        public Flight Flight { get; set; }

    }
}