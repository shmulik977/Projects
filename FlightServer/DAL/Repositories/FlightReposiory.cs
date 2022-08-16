using FlightServer.Infra;
using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.DAL.Repositories
{
    public class FlightReposiory : IFlightRepository
    {
        FlightContext flightContext;
        public FlightReposiory()
        {
            this.flightContext = new FlightContext();
            if (flightContext.StatusStation.Count() == 0)
            {
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 1,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "2",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 2,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "3",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 3,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "4",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 4,
                    OptionalFlightStation = "9",
                    OptionalLandingStation = "5",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 5,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "6,7",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 6,
                    OptionalFlightStation = "8",
                    OptionalLandingStation = "10",
                    Status = false
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 7,
                    OptionalFlightStation = "8",
                    OptionalLandingStation = "10"
                });
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 8,
                    OptionalFlightStation = "4",
                    OptionalLandingStation = "",
                    Status = false
                });
                //In The Air
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 9,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "1",
                    Status = false
                });
                //In The AirPort
                flightContext.StatusStation.Add(new StatusStation
                {
                    Id = 10,
                    OptionalFlightStation = "6,7",
                    OptionalLandingStation = "",
                    Status = false
                });
                flightContext.SaveChangesAsync();
            }
        }

        public async Task<List<StatusStation>> GetStations()
        {
            return flightContext.StatusStation.ToList();
        }

        public bool CreatePlannedFlight(PlannedFlights plannedFlights, out IPlanned planned)
        {
            flightContext.PlannedFlights.Add(plannedFlights);
            flightContext.SaveChangesAsync();
            bool IsEndStation = CheckIfStationEmpty(plannedFlights, out planned);
            return IsEndStation;
        }

        public async Task<Flight> CreateFlight()
        {
            var x = new Flight();
            x.CurrentStationId = 10;
            flightContext.Flights.Add(x);
            await flightContext.SaveChangesAsync();
            return x;
        }

        public bool CreatePlannedLanding(PlannedLanding plannedLanding, out IPlanned planned)
        {
            flightContext.PlannedLanding.Add(plannedLanding);
            flightContext.SaveChangesAsync();
            bool IsEndStation = CheckIfStationEmpty(plannedLanding, out planned);
            return IsEndStation;
        }

        public bool CheckIfStationEmpty(IPlanned planned, out IPlanned updatePlaned)
        {
            StatusStation myStation = FindStation(planned.SourceStationId);
            StatusStation destinationStation =  ChangeDestinationStation(planned, myStation);
            if (destinationStation == null || destinationStation.Status == true)//no place            
            {
                destinationStation = myStation;
                destinationStation.FlightId = planned.FlightId;
            }
            return MoveStation(planned, out updatePlaned, destinationStation);
        }


        private  StatusStation FindStation(int stationId)
        {
            var x = flightContext.StatusStation.Find(stationId);
            return x;
        }

        public StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation)
        {
            if (planned is PlannedFlights)
            {
                foreach (var stationId in myStation.OptionalFlightStation.Split(","))
                {
                    var item = flightContext.StatusStation.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                    nextStation.Id == int.Parse(stationId) && nextStation.Status == false);
                    if (item != null)
                        return item;
                }
            }
            else if (planned is PlannedLanding)
            {
                foreach (var stationId in myStation.OptionalLandingStation.Split(","))
                {
                    var item = flightContext.StatusStation.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                    nextStation.Id == int.Parse(stationId) && nextStation.Status == false);
                    if (item != null)
                        return item;
                }
            }
            return null;
        }


        private bool MoveStation(IPlanned planned, out IPlanned updatePlaned, StatusStation destinationStation)
        {
            var history = flightContext.FlightHistory.FirstOrDefault(x => x.FlightId == planned.FlightId && x.StationId == planned.SourceStationId);
            if (history == null)
            {
                flightContext.FlightHistory.Add(new FlightHistory()
                {
                    FlightId = planned.FlightId,
                    EntringTime = DateTime.Now,
                    StationId = destinationStation.Id,
                });
            }
            else
            {
                history.ExitTime = DateTime.Now;
                flightContext.FlightHistory.Update(history);
                flightContext.FlightHistory.Add(new FlightHistory()
                {
                    FlightId = planned.FlightId,
                    EntringTime = DateTime.Now,
                    StationId = destinationStation.Id,
                });

            }
            flightContext.SaveChangesAsync();
            if (destinationStation.Id > 8)
            {
                //end or start flight
                planned.StatusStationSource.Status = false;
                planned.StatusStationSource.FlightId = null;
                if (planned is PlannedFlights)
                    flightContext.PlannedFlights.Update((PlannedFlights)planned);
                else
                    flightContext.PlannedLanding.Update((PlannedLanding)planned);
                flightContext.SaveChangesAsync();
                updatePlaned = null;
                return true;
            }
            var sourceStation = FindStation(planned.SourceStationId);
            planned.StatusStationSource = destinationStation;
            planned.SourceStationId = destinationStation.Id;
            planned.DestinationStationId = destinationStation.Id.ToString();
            sourceStation.Status = false;
            destinationStation.FlightId = planned.FlightId;
            sourceStation.FlightId = null;
            destinationStation.Status = true;
            flightContext.StatusStation.Update(sourceStation);
            flightContext.StatusStation.Update(destinationStation);
            if (planned is PlannedFlights)
                flightContext.PlannedFlights.Update((PlannedFlights)planned);
            else
                flightContext.PlannedLanding.Update((PlannedLanding)planned);
            updatePlaned = planned;
            flightContext.SaveChangesAsync();
            return false;
        }

        public async Task<IList<FlightHistory>> GetAllFlightsHistory()
        {
            return flightContext.FlightHistory.ToList();
        }

        public async Task<IList<PlannedFlights>> GetAllPlannedFlights()
        {
            return flightContext.PlannedFlights.ToList();
        }

        public async Task<IList<PlannedLanding>> GetAllPlannedLandings()
        {
            return flightContext.PlannedLanding.ToList();
        }
    }
}
