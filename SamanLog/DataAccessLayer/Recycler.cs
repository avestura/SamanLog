using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using SamanLog.Models;
using SQLite;

namespace SamanLog.DataAccessLayer
{
    public class BankLogAdapter : RecyclerView.Adapter
    {
        private IEnumerable<BankLogs> Logs => DataAccessLayer.Data.GetTable().Reverse();

        public override int ItemCount
        {
            get
            {
                if(Logs != null)
                {
                    try
                    {
                        return Logs.Count();
                    } catch { return 0; }
                }
                return 0;
            }
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var bankLog = Logs.ElementAt(position);
            var myView = (MyViewHolder)holder;

            myView.title.Text = ( bankLog.TransactionType == TransactionTypes.Bardasht ) ? T.Bardasht :
                           ( bankLog.TransactionType == TransactionTypes.Variz ) ? T.Variz :
                           T.Vorod;

            myView.remaining.Text = (bankLog.AmountLeft.IsProvided()) ? bankLog.AmountLeft : "(بدون مقدار)";
            myView.reason.Text = (bankLog.Reason.IsProvided()) ? bankLog.Reason : "(بدون دلیل)";
            myView.money.Text = (bankLog.Amount.IsProvided()) ? bankLog.Amount : "(بدون مقدار)";
            myView.notes.Text = (bankLog.Notes.IsProvided()) ? bankLog.Notes : "(بدون یادداشت)";
            myView.cardNumber.Text = (bankLog.CardNumber.IsProvided()) ? bankLog.CardNumber : "(بدون شماره کارت)";
            myView.dateTime.Text = $"{bankLog.Date} {bankLog.Time}";
            myView.elementRoot.Tag = bankLog.ID;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View itemView = LayoutInflater.From(parent.Context)
                .Inflate(Resource.Layout.LogView, parent, false);

            return new MyViewHolder(itemView);
        }
    }

    public class MyViewHolder : RecyclerView.ViewHolder
    {
        public TextView money, remaining, dateTime, reason, cardNumber, title, notes;
        public View elementRoot;

        public MyViewHolder(View view) : base(view)
        {
            title = (TextView)view.FindViewById(Resource.Id.widget_logview_title);
            money = (TextView)view.FindViewById(Resource.Id.widget_logview_money);
            remaining = (TextView)view.FindViewById(Resource.Id.widget_logview_remaining);
            dateTime = (TextView)view.FindViewById(Resource.Id.widget_logview_dateTime);
            reason = (TextView)view.FindViewById(Resource.Id.widget_logview_reason);
            cardNumber = (TextView)view.FindViewById(Resource.Id.widget_logview_cardNumber);
            notes = (TextView)view.FindViewById(Resource.Id.widget_logview_notes);

            elementRoot = view.FindViewById(Resource.Id.widget_logview_elemntRoot);

            view.Click += View_Click;
            view.LongClick += View_LongClick;
        }

        private void View_LongClick(object sender, View.LongClickEventArgs e)
        {
            var builder = new AlertDialog.Builder(Utilities.MainActivityContext, Resource.Style.LightDialogTheme);
            builder.SetTitle("آیتم حذف شود؟");
            var id = int.Parse((string)elementRoot.Tag);

            var table = DataAccessLayer.Data.GetTable();

            var item = table.Where(x => x.ID == id).SingleOrDefault();

            builder.SetPositiveButton("حذف", (obj, args) =>
            {
                var connection = DataAccessLayer.Data.GetConnection();
                connection.Delete(item);
                if (Utilities.MainActivity != null)
                {
                    Utilities.MainActivity.ItemAdapter.NotifyDataSetChanged();
                }
            });
            builder.SetNegativeButton("بیخیال", (obj, args) =>
            {
            });

            builder.Create().Show();
        }

        private void View_Click(object sender, EventArgs e)
        {
            var builder = new AlertDialog.Builder(Utilities.MainActivity, Resource.Style.LightDialogTheme);
            builder.SetTitle("افزودن یادداشت");
            var editText = new EditText(Utilities.MainActivity)
            {
                InputType = Android.Text.InputTypes.ClassText
            };
            builder.SetView(editText);

            var id = int.Parse((string)elementRoot.Tag);
            var table = DataAccessLayer.Data.GetTable();

            var item = table.Where(x => x.ID == id).SingleOrDefault();

            editText.Text = (item.Notes.IsProvided()) ? item.Notes : "";

            builder.SetPositiveButton("ایجاد یادداشت", (obj, args) =>
            {
                var connection = DataAccessLayer.Data.GetConnection();
                item.Notes = editText.Text;
                connection.Update(item);
                if (Utilities.MainActivity != null)
                {
                    Utilities.MainActivity.ItemAdapter.NotifyDataSetChanged();
                }
            });

            builder.Create().Show();
        }
    }
}