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
using SamanLog.Models;
using SamanLog.DataAcessLayer;
using SQLite.Net;

namespace SamanLog.DataAccessLayer
{
    public static class Data
    {
        public static SQLiteConnection GetConnection() => new SQLiteConnection(DbManager.Platform, DbManager.PathToDatabase);
        public static TableQuery<BankLogs> GetTable() => GetConnection().Table<BankLogs>();

        public static string AddLog(BankLogs log)
        {
            try
            {
                var db = GetConnection();
                if (db.Insert(log) != 0)
                    db.Update(log);

                if (Utilities.MainActivity?.ItemAdapter != null)
                {
                    Utilities.MainActivity.ItemAdapter.NotifyDataSetChanged();
                }

                return "Ok";
            }
            catch (SQLiteException e)
            {
                return e.Message;
            }
        }
    }
}