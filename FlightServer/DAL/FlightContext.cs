using Microsoft.EntityFrameworkCore;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.DAL
{
    public class FlightContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=FlightDB.db3");
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<StatusStation> StatusStation { get; set; }
        public DbSet<FlightHistory> FlightHistory { get; set; }
        public DbSet<PlannedFlights> PlannedFlights { get; set; }
        public DbSet<PlannedLanding> PlannedLanding { get; set; }
    }
}
