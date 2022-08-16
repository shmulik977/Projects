using FlightServer.Infra;
using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace FlightServer.DAL
{
    public class MockDB : IMockRepository
    {
        public List<Flight> Flights { get; set; } = new List<Flight>();
        public List<FlightHistory> FlightsHistory { get; set; } = new List<FlightHistory>();
        public List<PlannedFlights> PlannedFlights { get; set; } = new List<PlannedFlights>();
        public List<PlannedLanding> PlannedLandings { get; set; } = new List<PlannedLanding>();
        public List<StatusStation> StatusStations { get; set; } = new List<StatusStation>();
        public MockDB()
        {

            if (StatusStations.Count() == 0)
            {
                StatusStations.Add(new StatusStation
                {
                    Id = 1,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "2",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 2,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "3",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 3,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "4",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 4,
                    OptionalFlightStation = "9",
                    OptionalLandingStation = "5",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 5,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "6,7",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 6,
                    OptionalFlightStation = "8",
                    OptionalLandingStation = "10",
                    Status = false
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 7,
                    OptionalFlightStation = "8",
                    OptionalLandingStation = "10"
                });
                StatusStations.Add(new StatusStation
                {
                    Id = 8,
                    OptionalFlightStation = "4",
                    OptionalLandingStation = "",
                    Status = false
                });
                //In The Air
                StatusStations.Add(new StatusStation
                {
                    Id = 9,
                    OptionalFlightStation = "",
                    OptionalLandingStation = "1",
                    Status = false
                });
                //In The AirPort
                StatusStations.Add(new StatusStation
                {
                    Id = 10,
                    OptionalFlightStation = "6,7",
                    OptionalLandingStation = "",
                    Status = false
                });
            }
        }

        public async Task<List<StatusStation>> GetStations()
        {
            return StatusStations.ToList();
        }

        public bool CreatePlannedFlight(PlannedFlights plannedFlights, out IPlanned planned)
        {
            PlannedFlights.Add(plannedFlights);
            bool IsEndStation = CheckIfStationEmpty(plannedFlights, out planned);
            return IsEndStation;
        }

        public Flight CreateFlight()
        {
            var x = new Flight();
            x.CurrentStationId = 10;
            Flights.Add(x);
            return x;
        }

        public bool CreatePlannedLanding(PlannedLanding plannedLanding, out IPlanned planned)
        {
            PlannedLandings.Add(plannedLanding);
            bool IsEndStation = CheckIfStationEmpty(plannedLanding, out planned);
            return IsEndStation;
        }

        public bool CheckIfStationEmpty(IPlanned planned, out IPlanned updatePlaned)
        {
            StatusStation myStation = FindStation(planned.SourceStationId);
            StatusStation destinationStation = ChangeDestinationStation(planned, myStation);
            if (destinationStation == null || destinationStation.Status == true)//no place            
            {
                destinationStation = myStation;
                destinationStation.FlightId = planned.FlightId;
            }
            return MoveStation(planned, out updatePlaned, destinationStation);
        }


        public StatusStation FindStation(int stationId)
        {
            var y = StatusStations.Find(x => x.Id == stationId);
            return y;
        }

        public StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation)
        {
            if (planned is PlannedFlights)
            {
                foreach (var stationId in myStation.OptionalFlightStation.Split(","))
                {
                    var item = StatusStations.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                    nextStation.Id == int.Parse(stationId) && nextStation.Status == false);
                    if (item != null)
                        return item;
                }
            }
            else if (planned is PlannedLanding)
            {
                foreach (var stationId in myStation.OptionalLandingStation.Split(","))
                {
                    var item = StatusStations.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                     nextStation.Id == int.Parse(stationId) && nextStation.Status == false);
                    if (item != null)
                        return item;
                }
            }
            return null;
        }


        private bool MoveStation(IPlanned planned, out IPlanned updatePlaned, StatusStation destinationStation)
        {
            var history = FlightsHistory.FirstOrDefault(x => x.FlightId == planned.FlightId && x.StationId == planned.SourceStationId);
            if (history == null)
            {
                FlightsHistory.Add(new FlightHistory()
                {
                    FlightId = planned.FlightId,
                    EntringTime = DateTime.Now,
                    StationId = destinationStation.Id,
                });
            }
            else
            {
                FlightsHistory.Remove(history);
                history.ExitTime = DateTime.Now;
                FlightsHistory.Add(history);
                FlightsHistory.Add(new FlightHistory()
                {
                    FlightId = planned.FlightId,
                    EntringTime = DateTime.Now,
                    StationId = destinationStation.Id,
                });

            }
            if (destinationStation.Id > 8)
            {
                //end or start flight
                planned.StatusStationSource.Status = false;
                planned.StatusStationSource.FlightId = null;
                if (planned is PlannedFlights)
                {
                    PlannedFlights.Remove((PlannedFlights)planned);
                    PlannedFlights.Add((PlannedFlights)planned);
                }
                else
                {
                    PlannedLandings.Remove((PlannedLanding)planned);
                    PlannedLandings.Add((PlannedLanding)planned);
                }
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
            #region
            StatusStations.Remove(sourceStation);
            StatusStations.Add(sourceStation);
            StatusStations.Remove(destinationStation);
            StatusStations.Add(destinationStation);

            if (planned is PlannedFlights)
            {
                PlannedFlights.Remove((PlannedFlights)planned);
                PlannedFlights.Add((PlannedFlights)planned);
            }
            else
            {
                PlannedLandings.Remove((PlannedLanding)planned);
                PlannedLandings.Add((PlannedLanding)planned);
            }

            updatePlaned = planned;
            #endregion
            return false;
        }

        public async Task<IList<FlightHistory>> GetAllFlightsHistory()
        {
            return FlightsHistory.ToList();
        }

        public async Task<IList<PlannedFlights>> GetAllPlannedFlights()
        {
            return PlannedFlights.ToList();
        }

        public async Task<IList<PlannedLanding>> GetAllPlannedLandings()
        {
            return PlannedLandings.ToList();
        }
    }
}


