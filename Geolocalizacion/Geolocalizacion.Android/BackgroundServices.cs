using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

namespace Geolocalizacion.Droid
{
    [Service(Enabled = true, Label ="BackgroundServices" )]
    public class BackgroundServices: Service
    {
        bool isRun;
        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            MessagingCenter.Send<string>("start", "value");
            isRun = true;
            return StartCommandResult.Sticky;
           // return base.OnStartCommand(intent, flags, startId);
        }
        public override void OnDestroy()
        {
            StopSelf();
            isRun = false;
            base.OnDestroy();
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }
    }
}