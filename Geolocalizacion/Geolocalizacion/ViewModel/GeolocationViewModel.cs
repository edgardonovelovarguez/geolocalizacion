using Geolocalizacion.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Geolocalizacion.ViewModel
{
    public class GeolocationViewModel : INotifyPropertyChanged
    {

        public string lat = string.Empty;
        public string lon = string.Empty;

        Page page;
        public string Latitud
        {
            get => lat;
            set
            {
                if (lat == value)
                    return;
                lat = value;
                OnPropertyChanged(nameof(Latitud));
                OnPropertyChanged(nameof(DisplayCor));

            }
        }
        public string Longitud
        {
            get => lon;
            set
            {
                if (lon == value)
                    return;
                lon = value;
                OnPropertyChanged(nameof(Longitud));
                OnPropertyChanged(nameof(DisplayCor));

            }
        }
        public string DisplayCor => $"Coordenadas :{Latitud} , {Longitud}";


        #region Command
        public ICommand CommandStartGeolocalizacion { get => new Command(StartGeolocalizacion); }

        public ICommand CommandStopGeolocalizacion { get => new Command(stopGeolocalizacion); }
        #endregion
 
        public GeolocationViewModel()
        {
            
        }
        public GeolocationViewModel(Page page)
        {
            this.page = page;
            MessagingCenter.Unsubscribe<string>(this, "value");
            MessagingCenter.Subscribe<string>(this, "serviceGeolocaizacion", (value) =>
            {
                if (value == "start")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        InitializePlugin();
                    });
                }
            });
        }
        private void StartGeolocalizacion()
        {
            MessagingCenter.Send<string>("start", "serviceGeolocaizacion");
            DependencyService.Get<IMessage>().ShortAlert("Inciando geolocalizacion");
        }
        private void stopGeolocalizacion()
        {
            MessagingCenter.Send<string>("stop", "serviceGeolocaizacion");
            StopGps();
            DependencyService.Get<IMessage>().ShortAlert("Deteniendo geolocalizacion");
        }
        private async void InitializePlugin()
        {
           if (!CrossGeolocator.IsSupported)
            { 
                await page.DisplayAlert("Error","Error al cargar el plugin","OK");
                return;
            }
            if(!CrossGeolocator.Current.IsGeolocationEnabled||!CrossGeolocator.Current.IsGeolocationAvailable)
            {
                await page.DisplayAlert("Error", "Revisar configuración del GPS", "OK");
                return;
            }
            await CrossGeolocator.Current.StartListeningAsync(new TimeSpan(0, 0, 3), 0.3, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                ActivityType = Plugin.Geolocator.Abstractions.ActivityType.AutomotiveNavigation,
                AllowBackgroundUpdates = true,
                DeferLocationUpdates = true,
                DeferralDistanceMeters = 1,
                DeferralTime = new TimeSpan(0, 0, 1),
                ListenForSignificantChanges = true,
                PauseLocationUpdatesAutomatically = false
            });
            CrossGeolocator.Current.PositionChanged += Current_PositionChanged;
        }

       private void Current_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            if (!CrossGeolocator.Current.IsListening)
            {
                return;
            }
           var position = CrossGeolocator.Current.GetPositionAsync();
            Latitud = position.Result.Latitude.ToString();
            Longitud = position.Result.Longitude.ToString();

            var output = "coordenadas: " + Latitud + ", " + Longitud;

            DependencyService.Get<IMessage>().ShortAlert("coordenadas: " + Latitud + ", " + Longitud);
            Debug.WriteLine(output);



        }
        
        private void StopGps()
        {
            CrossGeolocator.Current.StopListeningAsync();
        }
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
