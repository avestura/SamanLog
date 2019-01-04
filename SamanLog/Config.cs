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

namespace SamanLog
{
    public class Config
    {
        public const string SharedPreferencesName = "SamanLogConfig";

        public string IsNumberLimited {
            get { return RetrieveSetting("IsNumberLimited", "On"); }
            set { SaveSetting("IsNumberLimited", value); }
        }

        public string LimitNumber
        {
            get { return RetrieveSetting("LimitNumber", "20000"); }
            set { SaveSetting("LimitNumber", value); }
        }

        private void SaveSetting(string key, string value)
        {
            var prefs = Application.Context.GetSharedPreferences(SharedPreferencesName, FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(key, value);
            prefEditor.Commit();
        }

        protected string RetrieveSetting(string key, string defaultValue = null)
        {
            var prefs = Application.Context.GetSharedPreferences(SharedPreferencesName, FileCreationMode.Private);
            var prefValue = prefs.GetString(key, defaultValue);

            return prefValue;
        }
    }
}