using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using SamanLog.DataAcessLayer;
using SamanLog.Models;
using System;
using Android.Support.V7.App;
using Android;
using SamanLog.Controls;
using Android.Util;
using Android.Support.V7.Widget;
using Android.Views;
using System.Collections.Generic;
using SQLite;
using SamanLog.DataAccessLayer;

namespace SamanLog
{
    [Activity(Label = "تراکنش سامان", MainLauncher = false, Theme = "@style/SamanTheme")]
    public class MainActivity : AppCompatActivity
    {
        public BankLogAdapter ItemAdapter { get; set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Main);

            Utilities.MainActivity = this;
            Utilities.MainActivityContext = this;

            ApplicationInfo.Flags = Android.Content.PM.ApplicationInfoFlags.SupportsRtl;
            Window.DecorView.LayoutDirection = Android.Views.LayoutDirection.Rtl;

            // suffered too long to find out that as of Android 6.0 (API 23), you will have to register permissions in the main activity java class as well.
            Manifest.Permission.ReadSms.RequestPermissonIfNeeded();
            Manifest.Permission.ReceiveSms.RequestPermissonIfNeeded();

            // Set our view from the "main" layout resource

            DbManager.Init();
            if (!typeof(SmsReciverService).IsRunningService())
            {
                StartService(new Intent(this, typeof(SmsReciverService)));
            }

            var itemContainer = FindViewById<RecyclerView>(Resource.Id.samanMainItemContainer);
            itemContainer.HasFixedSize = true;
            itemContainer.SetLayoutManager(new LinearLayoutManager(this));
            itemContainer.SetItemAnimator(new DefaultItemAnimator());

            ItemAdapter = new BankLogAdapter();
            itemContainer.SetAdapter(ItemAdapter);
        }
    }
}