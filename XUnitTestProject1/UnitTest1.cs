using FlightServer.DAL;
using FlightServer.DAL.Repositories;
using FlightServer.Infra;
using FlightServer.Services;
using Shared.Lib.Infra;
using Shared.Lib.Models;
using System;
using Xunit;

namespace XUnitTestProject1
{
    public class UnitTest1
    {
        static IMockRepository mock = new MockDB();
        FlightService flightService = new FlightService(null, mock);

        [Fact]
        public async void Test1()
        {
            var v = mock.StatusStations.Find(x => x.Id == 1);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 9, DestinationStationId = "1", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 1);
            Assert.Equal("", v.FlightId.ToString());
        }
        [Fact]
        public async void Test2()
        {
            var v = mock.StatusStations.Find(x => x.Id == 1);
            v.Status = false;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 9, DestinationStationId = "1", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 1);
            Assert.Equal("1", v.FlightId.ToString());
        }
        [Fact]
        public async void Test3()
        {
            var v = mock.StatusStations.Find(x => x.Id == 3);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 2, DestinationStationId = "3", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 1);
            Assert.Equal("", v.FlightId.ToString());
        }
        [Fact]
        public async void Test4()
        {
            var v = mock.StatusStations.Find(x => x.Id == 3);
            v.Status = true;
            v.FlightId = 1;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 3, DestinationStationId = "4", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 3);
            string vv = v.Status.ToString();
            Assert.Equal("False", vv);
        }
        [Fact]
        public async void Test5()
        {
            var v = mock.StatusStations.Find(x => x.Id == 6);
            v.Status = true;
            v.FlightId = 1;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 6, DestinationStationId = "10", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            Assert.True(IsEndStation);

        }

        [Fact]
        public async void Test6()
        {
            var v = mock.StatusStations.Find(x => x.Id == 3);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 2, DestinationStationId = "3", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 2);
            Assert.Equal("True", v.Status.ToString());
            v = mock.StatusStations.Find(x => x.Id == 3);
            v.Status = false;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 2);
            Assert.Equal("False", v.Status.ToString());
        }

        [Fact]
        public async void Test7()
        {

            var v = mock.StatusStations.Find(x => x.Id == 8);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 4, DestinationStationId = "5", StatusStationSource = new StatusStation() };
            PlannedFlights plannedFlights = new PlannedFlights() { FlightId = 2, Id = 2, SourceStationId = 4, DestinationStationId = "9", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 5);
            Assert.Equal("True", v.Status.ToString());
            IsEndStation = mock.CreatePlannedFlight(plannedFlights, out planned);
            v = mock.StatusStations.Find(x => x.Id == 4);
            Assert.Equal("False", v.Status.ToString());
        }
        [Fact]
        public async void Test8()
        {

            var v = mock.StatusStations.Find(x => x.Id == 8);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            v = mock.StatusStations.Find(x => x.Id == 6);
            v.Status = true;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            v = mock.StatusStations.Find(x => x.Id == 10);
            v.Status = false;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedFlights plannedFlights = new PlannedFlights() { FlightId = 1, Id = 1, SourceStationId = 6, DestinationStationId = "8", StatusStationSource = new StatusStation() };
            v = mock.StatusStations.Find(x => x.Id == 6);
            Assert.Equal("True", v.Status.ToString());

        }
        [Fact]
        public async void Test9()
        {
            var v = mock.StatusStations.Find(x => x.Id == 1);
            v.Status = false;
            mock.StatusStations.Remove(v);
            mock.StatusStations.Add(v);
            IPlanned planned = new PlannedLanding();
            Flight flight = mock.CreateFlight();
            PlannedLanding plannedLanding = new PlannedLanding() { FlightId = 1, Id = 1, SourceStationId = 9, DestinationStationId = "1", StatusStationSource = new StatusStation() };
            bool IsEndStation = mock.CreatePlannedLanding(plannedLanding, out planned);
            v = mock.StatusStations.Find(x => x.Id == 1);
            var xx = mock.FlightsHistory.Count;
            Assert.Equal("10", xx.ToString());
        }
    }
}
