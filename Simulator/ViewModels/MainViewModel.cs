using GalaSoft.MvvmLight;
using Shared.Lib.Models;
using Simulator.Infra;
using System;
using System.Timers;
using Windows.UI.Core;

namespace Simulator.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ISimulatorService _simulatorService;
        private StatusStation[] _Stations;
        public StatusStation[] Stations
        {
            get { return _Stations; }
            set
            {
                _Stations = value;
                RaisePropertyChanged();
            }
        }
        Random random = new Random();

        public MainViewModel(ISimulatorService simulatorService)
        {
            _simulatorService = simulatorService;
            Timer timer = new Timer(3000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }
        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            int res = random.Next(1, 10);
            if (res == 1)
            {
                await _simulatorService.LandingFlight();
            }
            else if (res == 2)
            {
                await _simulatorService.PlaneTakingOff();
            }
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                async () =>
                     {
                         var ListStations = await _simulatorService.GetStations();
                         StatusStation[] tmpStations = new StatusStation[10];
                         foreach (var station in ListStations)
                         {
                             tmpStations[station.Id - 1] = station;
                         }
                         Stations = tmpStations;
                     }
                );
        }
    }
}
