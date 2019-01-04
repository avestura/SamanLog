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
using Android.Telephony;
using Android.Provider;
using Android.Util;
using System.Threading.Tasks;
using SamanLog.DataAcessLayer;
using System.Text.RegularExpressions;
using SamanLog.Models;

namespace SamanLog
{
    [Service]
    public class SmsReciverService : Service
    {
        public override void OnCreate()
        {
            base.OnCreate();

            DbManager.Init();

            Log.Debug("CREATE", "Service Created!");

            var receiver = new SmsReciver();

            RegisterReceiver(receiver, new IntentFilter("android.provider.Telephony.SMS_RECEIVED"));
        }

        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.Sticky;
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "SMS Rec", Exported = true   )]
    [IntentFilter(new[] { "android.provider.Telephony.SMS_RECEIVED" }, Priority = 1000)]
    public class SmsReciver : BroadcastReceiver
    {
        private const string IntentAction = "android.provider.Telephony.SMS_RECEIVED";

        private static BankLogs LastItemAdded { get; set; }

        public override void OnReceive(Context context, Intent intent)
        {
            if (intent.Action != IntentAction) return;

            Log.Debug("SMS", "SMS Broadcase called!");

            SmsMessage[] messages = Telephony.Sms.Intents.GetMessagesFromIntent(intent);

            var body = "";

            var isFromSamanBank = messages[0].MessageBody.StartsWith(T.BankHeader);

            // Assemble Partial Messages in body variable of type string
            if (isFromSamanBank)
            {
                Log.Debug("SMS", "First part contains T.BankHeader");
                body += messages[0].MessageBody;
                for (var i = 1; i < messages.Length; i++)
                {
                    var lastPartOfMessage = messages[i].MessageBody.Split(new[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).Last();
                    if (Regex.Match(lastPartOfMessage, T.TimePattern).Success)
                    {
                        body += messages[i].MessageBody;
                        Log.Debug("SMS", "Adding part to body finished!");
                        break;
                    }
                    body += messages[i].MessageBody;
                    Log.Debug("SMS", "Part is still adding to body!");
                }
            }

            // Parse body
            var log = body.ParseSms();
            var haveData = ( log == null ) ? "is null!" : "contains value!";
            Log.Debug("SMS", $"body parsed to a SMS and {haveData}");

            // Add to db
            if (log != null)
            {
                if(LastItemAdded == null || !log.HaveSameValuesWith(LastItemAdded))
                {
                    DataAccessLayer.Data.AddLog(log);
                    LastItemAdded = log;
                }
                Log.Debug("SMS", $"Attemp to add to database...");
            }
        }
    }
}