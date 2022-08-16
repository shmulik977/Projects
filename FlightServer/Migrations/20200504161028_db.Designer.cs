﻿// <auto-generated />
using System;
using FlightServer.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlightServer.Migrations
{
    [DbContext(typeof(FlightContext))]
    [Migration("20200504161028_db")]
    partial class db
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("Shared.Lib.Models.Flight", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CurrentStationId");

                    b.HasKey("Id");

                    b.HasIndex("CurrentStationId");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("Shared.Lib.Models.FlightHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime?>("EntringTime");

                    b.Property<DateTime?>("ExitTime");

                    b.Property<int>("FlightId");

                    b.Property<int>("StationId");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("StationId");

                    b.ToTable("FlightHistory");
                });

            modelBuilder.Entity("Shared.Lib.Models.PlannedFlights", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DestinationStationId");

                    b.Property<int>("FlightId");

                    b.Property<int>("SourceStationId");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("SourceStationId");

                    b.ToTable("PlannedFlights");
                });

            modelBuilder.Entity("Shared.Lib.Models.PlannedLanding", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DestinationStationId");

                    b.Property<int>("FlightId");

                    b.Property<int>("SourceStationId");

                    b.HasKey("Id");

                    b.HasIndex("FlightId");

                    b.HasIndex("SourceStationId");

                    b.ToTable("PlannedLanding");
                });

            modelBuilder.Entity("Shared.Lib.Models.StatusStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int?>("FlightId");

                    b.Property<string>("OptionalFlightStation");

                    b.Property<string>("OptionalLandingStation");

                    b.Property<bool>("Status");

                    b.HasKey("Id");

                    b.ToTable("StatusStation");
                });

            modelBuilder.Entity("Shared.Lib.Models.Flight", b =>
                {
                    b.HasOne("Shared.Lib.Models.StatusStation", "StatusStation")
                        .WithMany()
                        .HasForeignKey("CurrentStationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Shared.Lib.Models.FlightHistory", b =>
                {
                    b.HasOne("Shared.Lib.Models.Flight", "Flight")
                        .WithMany("FlightsHistory")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Shared.Lib.Models.StatusStation", "StatusStation")
                        .WithMany("FlightsHistory")
                        .HasForeignKey("StationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Shared.Lib.Models.PlannedFlights", b =>
                {
                    b.HasOne("Shared.Lib.Models.Flight", "Flight")
                        .WithMany("PlannedFlights")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Shared.Lib.Models.StatusStation", "StatusStationSource")
                        .WithMany()
                        .HasForeignKey("SourceStationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Shared.Lib.Models.PlannedLanding", b =>
                {
                    b.HasOne("Shared.Lib.Models.Flight", "Flight")
                        .WithMany("PlannedLandings")
                        .HasForeignKey("FlightId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Shared.Lib.Models.StatusStation", "StatusStationSource")
                        .WithMany()
                        .HasForeignKey("SourceStationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
