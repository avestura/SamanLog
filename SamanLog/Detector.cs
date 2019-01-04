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
using System.Text.RegularExpressions;

namespace SamanLog
{
    public enum StatementType
    {
        Header, TransactionInfo, AccountInfo, RemainingInfo, DateInfo, TimeInfo, Undefined
    }

    public static class Detector
    {
        public static StatementType GetStatementType(this string statement)
        {
            if (statement.StartsWith(T.BankHeader))
                return StatementType.Header;

            if (statement.StartsWith(T.Variz) ||
                statement.StartsWith(T.Bardasht) ||
                statement.StartsWith(T.Vorod))
                return StatementType.TransactionInfo;

            if (statement.StartsWith(T.Be) || statement.StartsWith(T.Az)) return StatementType.AccountInfo;

            if (statement.StartsWith(T.Mande)) return StatementType.RemainingInfo;

            if (Regex.Match(statement, T.DatePattern).Success) return StatementType.DateInfo;

            if (Regex.Match(statement, T.TimePattern).Success) return StatementType.TimeInfo;

            return StatementType.Undefined;
        }
    }

    public static class T
    {
        public const string BankHeader = "بانك سامان";
        public const string Variz = "واريز";
        public const string Bardasht = "برداشت";
        public const string Enteghal = "انتقال وجه";
        public const string Vorod = "ورود";
        public const string Ghabz = "پرداخت قبض";
        public const string Az = "از";
        public const string Be = "به";
        public const string Mande = "مانده";
        public const string Khodpardaz = "خودپرداز";
        public const string Paya = "پایا";
        public const string Sood = "سود";
        public const string Mablagh = "مبلغ";
        public const string Rial = "ريال";

        public const string DatePattern = @"1[34]\d+\/\d+\/\d+";
        public const string TimePattern = @"\d+:\d+(:\d+)?";
    }
}