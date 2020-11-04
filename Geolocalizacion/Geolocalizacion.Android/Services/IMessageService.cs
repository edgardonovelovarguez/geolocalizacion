using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Geolocalizacion.Droid.Services;
using Geolocalizacion.Services;

[assembly: Xamarin.Forms.Dependency(typeof(IMessageService))]
namespace Geolocalizacion.Droid.Services
{
    public class IMessageService : IMessage
    {
        public void ShortAlert(string message)
        {
            Toast.MakeText(Android.App.Application.Context, message, ToastLength.Short).Show();
        }
    }
}