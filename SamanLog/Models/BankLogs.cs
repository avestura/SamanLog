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
using SQLite;
using SQLite.Net.Attributes;

namespace SamanLog.Models
{
    public enum TransactionTypes
    {
        Variz, Bardasht, Login
    }

    public class BankLogs
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public TransactionTypes TransactionType { get; set; }

        public string Amount { get; set; }

        public string CardNumber { get; set; }

        public string AmountLeft { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public string Reason { get; set; }

        public string Notes { get; set; }
    }
}