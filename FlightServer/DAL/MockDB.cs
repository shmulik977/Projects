using FlightServer.Infra;
using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using System.Collections.Generic;
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

            if (!StatusStations.Any())
            {
                StatusStations.Add(CreateStatusStation(1, "", "2"));
                StatusStations.Add(CreateStatusStation(2, "", "3"));
                StatusStations.Add(CreateStatusStation(3, "", "4"));
                StatusStations.Add(CreateStatusStation(4, "9", "5"));
                StatusStations.Add(CreateStatusStation(5, "", "6,7"));
                StatusStations.Add(CreateStatusStation(6, "8", "10"));
                StatusStations.Add(CreateStatusStation(7, "8", "10"));
                StatusStations.Add(CreateStatusStation(8, "4", ""));
                StatusStations.Add(CreateStatusStation(9, "", "1"));
                StatusStations.Add(CreateStatusStation(10, "6,7", ""));
            }
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

        public async Task<List<StatusStation>> GetStations()
        {
            return StatusStations.ToList();
        }

        public bool CreatePlannedFlight(PlannedFlights plannedFlights, out IPlanned planned)
        {
            PlannedFlights.Add(plannedFlights);
            bool isEndStation = CheckIfStationEmpty(plannedFlights, out planned);
            return isEndStation;
        }

        public Flight CreateFlight()
        {
            var flight = new Flight
            {
                CurrentStationId = 10
            };
            Flights.Add(flight);
            return flight;
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
            if (destinationStation == null || destinationStation.Status)
            {
                destinationStation = myStation;
                destinationStation.FlightId = planned.FlightId;
            }
            return MoveStation(planned, out updatePlaned, destinationStation);
        }

        public StatusStation FindStation(int stationId)
        {
            var statusStation = StatusStations.Find(x => x.Id == stationId);
            return statusStation;
        }

        public StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation)
        {
            if (planned is PlannedFlights)
            {
                foreach (var stationId in myStation.OptionalFlightStation.Split(","))
                {
                    var item = StatusStations.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                    nextStation.Id == int.Parse(stationId) && !nextStation.Status);
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

        private StatusStation CreateStatusStation(int id, string flightStation, string landingStation)
        {
            return new StatusStation
            {
                Id = id,
                OptionalFlightStation = flightStation,
                OptionalLandingStation = landingStation,
                Status = false
            };
        }

        private bool MoveStation(IPlanned planned, out IPlanned updatePlaned, StatusStation destinationStation)
        {
            var history = FlightsHistory.FirstOrDefault(x => x.FlightId == planned.FlightId && x.StationId == planned.SourceStationId);
            if (history == null)
                FlightsHistory.Add(CreateFlightHistory(planned, destinationStation.Id));
            else
            {
                FlightsHistory.Remove(history);
                history.ExitTime = DateTime.Now;
                FlightsHistory.Add(history);
                FlightsHistory.Add(CreateFlightHistory(planned, destinationStation.Id));
            }

            if (destinationStation.Id > 8)
            {
                planned.StatusStationSource.Status = false;
                planned.StatusStationSource.FlightId = null;
                UpdatePlannedFlight(planned);
                updatePlaned = null;
                return true;
            }
            UpdateFlightData(planned, destinationStation);
            StatusStations.Remove(destinationStation);
            StatusStations.Add(destinationStation);
            UpdatePlannedFlight(planned);
            updatePlaned = planned;
            return false;
        }

        private void UpdatePlannedFlight(IPlanned planned)
        {
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
        }

        private FlightHistory CreateFlightHistory(IPlanned planned, int id)
        {
            return new FlightHistory()
            {
                FlightId = planned.FlightId,
                EntringTime = DateTime.Now,
                StationId = id,
            };
        }

        private void UpdateFlightData(IPlanned planned, StatusStation destinationStation)
        {
            var sourceStation = FindStation(planned.SourceStationId);
            planned.StatusStationSource = destinationStation;
            planned.SourceStationId = destinationStation.Id;
            planned.DestinationStationId = destinationStation.Id.ToString();
            sourceStation.Status = false;
            destinationStation.FlightId = planned.FlightId;
            sourceStation.FlightId = null;
            destinationStation.Status = true;
            StatusStations.Remove(sourceStation);
            StatusStations.Add(sourceStation);
        }
    }
}