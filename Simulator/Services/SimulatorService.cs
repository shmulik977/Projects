using Microsoft.AspNetCore.SignalR.Client;
using Shared.Lib.Models;
using Simulator.Infra;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Simulator.Services
{
    public class SimulatorService: ISimulatorService
    {
        private const string url = "http://localhost:55726/FlightHub";
        HubConnection connection;
        List<StatusStation> StatusStations;
        public SimulatorService()
        {
            StatusStations = new List<StatusStation>();
            connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();
            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.On<List<StatusStation>>("GotStations", (stations) =>
            {
                StatusStations = stations;
            });
        }

        public async Task<List<StatusStation>> GetStations()
        {
            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("GetStations");
                return StatusStations;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task LandingFlight()
        {
            try
            {
            await connection.StartAsync();
            await connection.InvokeAsync("CreateLanding");
            }
            catch (Exception e)
            {
                throw;
            }
        }
        public async Task PlaneTakingOff()
        {
            try
            {
                await connection.StartAsync();
                await connection.InvokeAsync("CreateFlight");
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
