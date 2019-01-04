using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace SamanLog.Controls
{
    public class LogView : FrameLayout
    {
        public string Title { get; set; }

        public LogView(Context context, IAttributeSet attrs) :
            base(context, attrs)
        {
            Initialize(context, attrs, 0);
        }

        public LogView(Context context, IAttributeSet attrs, int defStyle) :
            base(context, attrs, defStyle)
        {
            Initialize(context, attrs, defStyle);
        }

        private void Initialize(Context context, IAttributeSet attrs, int defStyle)
        {
            var view = (LogView)Inflate(context, Resource.Layout.LogView, null);

            view.FindViewById<TextView>(Resource.Id.widget_logview_title).Text = Title;

            AddView(view);
        }
    }
}