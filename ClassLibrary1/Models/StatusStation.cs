using Shared.Lib.Infra;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Shared.Lib.Models
{
    public class StatusStation
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Status { get; set; }
        public int? FlightId { get; set; }
        public string OptionalLandingStation { get; set; }
        public string OptionalFlightStation { get; set; }
        public List<FlightHistory> FlightsHistory { get; set; }



    }

}
