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
            if (!flightContext.StatusStation.Any())
            {
                flightContext.StatusStation.Add(CreateStatusStation(1, "", "2"));
                flightContext.StatusStation.Add(CreateStatusStation(2, "", "3"));
                flightContext.StatusStation.Add(CreateStatusStation(3, "", "4"));
                flightContext.StatusStation.Add(CreateStatusStation(4, "9", "5"));
                flightContext.StatusStation.Add(CreateStatusStation(5, "", "6,7"));
                flightContext.StatusStation.Add(CreateStatusStation(6, "8", "10"));
                flightContext.StatusStation.Add(CreateStatusStation(7, "8", "10"));
                flightContext.StatusStation.Add(CreateStatusStation(8, "4", ""));
                flightContext.StatusStation.Add(CreateStatusStation(9, "", "1"));
                flightContext.StatusStation.Add(CreateStatusStation(10, "6,7", ""));
              
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
            var flight = new Flight();
            flight.CurrentStationId = 10;
            flightContext.Flights.Add(flight);
            await flightContext.SaveChangesAsync();
            return flight;
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
            StatusStation destinationStation = ChangeDestinationStation(planned, myStation);
            if (destinationStation == null || destinationStation.Status == true)//no place            
            {
                destinationStation = myStation;
                destinationStation.FlightId = planned.FlightId;
            }
            return MoveStation(planned, out updatePlaned, destinationStation);
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

        public StatusStation ChangeDestinationStation(IPlanned planned, StatusStation myStation)
        {
            if (planned is PlannedFlights)
            {
                foreach (var stationId in myStation.OptionalFlightStation.Split(","))
                {
                    var statusStation = GetStatusStation(myStation, stationId);
                    if (statusStation != null)
                        return statusStation;
                }
            }
            else if (planned is PlannedLanding)
            {
                foreach (var stationId in myStation.OptionalLandingStation.Split(","))
                {
                    var statusStation = GetStatusStation(myStation, stationId);
                    if (statusStation != null)
                        return statusStation;
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

        private StatusStation FindStation(int stationId)
        {
            var statusStation = flightContext.StatusStation.Find(stationId);
            return statusStation;
        }

        private StatusStation GetStatusStation(StatusStation myStation, string stationId)
        {
            return flightContext.StatusStation.FirstOrDefault(nextStation => nextStation.Id != myStation.Id &&
                    nextStation.Id == int.Parse(stationId) && nextStation.Status == false);
        }

        private bool MoveStation(IPlanned planned, out IPlanned updatePlaned, StatusStation destinationStation)
        {
            var history = flightContext.FlightHistory.FirstOrDefault(x => x.FlightId == planned.FlightId && x.StationId == planned.SourceStationId);
            if (history == null)
                flightContext.FlightHistory.Add(CreateFlightHistory(planned, destinationStation.Id));
            else
            {
                history.ExitTime = DateTime.Now;
                flightContext.FlightHistory.Update(history);
                flightContext.FlightHistory.Add((CreateFlightHistory(planned, destinationStation.Id)));
            }
            flightContext.SaveChangesAsync();
            if (destinationStation.Id > 8)
            {
                planned.StatusStationSource.Status = false;
                planned.StatusStationSource.FlightId = null;
                UpdatePlannedFlight(planned);
                flightContext.SaveChangesAsync();
                updatePlaned = null;
                return true;
            }
            UpdateFlightData(planned, destinationStation);
            UpdatePlannedFlight(planned);
            updatePlaned = planned;
            flightContext.SaveChangesAsync();
            return false;
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
            flightContext.StatusStation.Remove(sourceStation);
            flightContext.StatusStation.Add(sourceStation);
        }

        private void UpdatePlannedFlight(IPlanned planned)
        {
            if (planned is PlannedFlights)
                flightContext.PlannedFlights.Update((PlannedFlights)planned);
            else
                flightContext.PlannedLanding.Update((PlannedLanding)planned);
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
    }
}