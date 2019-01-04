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
using System.Threading.Tasks;
using System.Threading;

namespace SamanLog
{
    [Activity(MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash")]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Thread.Sleep(1000); // Simulate a bit of startup work.
            StartActivity(typeof(MainActivity));
        }
    }
}