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
using Android.Support.V4.App;
using Android;
using Android.Support.V7.App;

namespace SamanLog
{
    public static class Utilities
    {
        public static MainActivity MainActivity { get; set; }
        public static Context MainActivityContext { get; set; }

        public static bool IsRunningService(this Type type)
        {
            var manager = (ActivityManager)MainActivityContext.GetSystemService(Context.ActivityService);

            foreach (var service in manager.GetRunningServices(int.MaxValue))
            {
                if (service.Service.ClassName.Equals(Java.Lang.Class.FromType(type).CanonicalName))
                {
                    return true;
                }
            }
            return false;
        }

        public static string[] GetLines(this string str) {
            string[] lines = str.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);
            return lines;
        }

        public static BankLogs ParseSms(this string sms)
        {
            if (!sms.Contains(T.BankHeader)) return null;

            var dataModel = new BankLogs();

            string[] lines = sms.Split(new[] { System.Environment.NewLine }, StringSplitOptions.None);

            try
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    var text = lines[i];
                    var lineType = text.GetStatementType();

                    if (lineType == StatementType.TransactionInfo)
                    {
                        var textModified = text.Replace(T.Mablagh, "").Replace(T.Be, "");
                        string[] words = textModified.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (words[0] == T.Variz)
                        {
                            dataModel.TransactionType = TransactionTypes.Variz;

                            if (words[1] == T.Sood)
                            {
                                dataModel.Reason = T.Sood;
                                dataModel.Amount = words[2].Replace(T.Rial, "");
                            } else
                            {
                                dataModel.Amount = words[1].Replace(T.Rial, "");
                            }
                        } else if (words[0] == T.Bardasht)
                        {
                            dataModel.TransactionType = TransactionTypes.Bardasht;
                            dataModel.Amount = words[1];
                            dataModel.Reason = string.Join(" ", words.Skip(2));
                        } else if (words[0] == T.Vorod)
                        {
                            dataModel.TransactionType = TransactionTypes.Login;
                        }
                    } else if (lineType == StatementType.AccountInfo)
                    {
                        string[] words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        dataModel.CardNumber = words[1];
                    } else if (lineType == StatementType.RemainingInfo)
                    {
                        string[] words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        dataModel.AmountLeft = words[1];
                    } else if (lineType == StatementType.DateInfo)
                    {
                        dataModel.Date = text;
                    } else if (lineType == StatementType.TimeInfo)
                    {
                        dataModel.Time = text;
                    }
                }
            } catch
            {
                return null;
            }

            return dataModel;
        }

        public static void RequestPermissonIfNeeded(this string permission, Activity activity = null)
        {
            if (activity == null)
                activity = MainActivity;

            var permissionCheck =
                Android.Support.V4.Content.ContextCompat.CheckSelfPermission(MainActivityContext, permission);

            if (permissionCheck != Android.Content.PM.Permission.Granted)
            {
                ActivityCompat.RequestPermissions(activity, new string[] { permission }, 1);
            }
        }

        public static bool IsProvided(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }

        public static bool HaveSameValuesWith(this BankLogs first, BankLogs second)
        {
            return first.CardNumber == second.CardNumber &&
                first.Date == second.Date &&
                first.Reason == second.Reason &&
                first.Time == second.Time &&
                first.Amount == second.Amount &&
                first.AmountLeft == second.AmountLeft &&
                first.TransactionType == second.TransactionType)
        }
    }
}