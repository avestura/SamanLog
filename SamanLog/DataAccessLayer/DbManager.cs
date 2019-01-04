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
using System.IO;
using Android.Util;
using SamanLog.Models;
using SQLite.Net;
using SQLite.Net.Interop;

namespace SamanLog.DataAcessLayer
{
    public class DbManager
    {
        public static string PathToDatabase { get; } = System.IO.Path.Combine(
            System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
            "db_SamanLogs.db"
            );

        public static void Init()
        {
            Log.Debug("DATABASE", PathToDatabase);
            CreateDatabase(PathToDatabase);
        }

        public static readonly ISQLitePlatform Platform;

        static DbManager()
        {
            if( ((int)Android.OS.Build.VERSION.SdkInt) >= (int)BuildVersionCodes.N)
            {
                Platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroidN();
            } else
            {
                Platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            }
        }

        public static string CreateDatabase(string path)
        {
            try
            {
                var connection = new SQLiteConnection(Platform, path);
                connection.CreateTable<BankLogs>();
                Log.Debug("Create/DB", "Data Base Created!");
                return "Ok";
            } catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        private string InsertUpdateData(BankLogs data, string path)
        {
            try
            {
                var db = new SQLiteConnection(Platform, path);
                if (db.Insert(data) != 0)
                    db.Update(data);
                return "Ok";
            } catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }

        private string InsertUpdateAllData(IEnumerable<BankLogs> data, string path)
        {
            try
            {
                var db = new SQLiteConnection(Platform, path);
                if (db.InsertAll(data) != 0)
                    db.UpdateAll(data);
                return "Ok";
            } catch (SQLiteException ex)
            {
                return ex.Message;
            }
        }
    }
}