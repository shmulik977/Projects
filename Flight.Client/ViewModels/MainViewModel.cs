using Flight.Client.Lib.Infra;
using GalaSoft.MvvmLight;
using Shared.Lib.Models;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Timers;
using Windows.UI.Core;
using Timer = System.Timers.Timer;

namespace Flight.Client.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IFlightService _flightService;
        private ObservableCollection<FlightHistory> _historyCollection;
        public ObservableCollection<FlightHistory> HistoryCollection
        {
            get { return _historyCollection; }
            set
            {
                _historyCollection = value;
                RaisePropertyChanged();
            }
        }
        public MainViewModel(IFlightService flightService)
        {
            _flightService = flightService;
            Timer timer = new Timer(3000);
            timer.Elapsed += Timer_Elapsed;
            Thread.Sleep(5000);
            timer.Start();

        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
                 async () =>
                 {
                     HistoryCollection = new ObservableCollection<FlightHistory>(await _flightService.GetHistory());

                 }
            );
        }
    }
}
