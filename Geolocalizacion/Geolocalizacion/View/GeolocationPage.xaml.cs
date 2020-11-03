using Geolocalizacion.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Geolocalizacion.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GeolocationPage : ContentPage
    {
        GeolocationViewModel geolocationViewModel;
        public GeolocationPage()
        {
            InitializeComponent();
            geolocationViewModel = new GeolocationViewModel(this);
        }

    }
}