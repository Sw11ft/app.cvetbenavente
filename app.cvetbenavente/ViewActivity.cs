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
using Newtonsoft.Json;
using Android.Telephony;

namespace app.cvetbenavente
{
    [Activity(Label = "CVETBENAVENTE", Theme = "@android:style/Theme.Material.Light")]
    public class ViewActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.View);

            string data = Intent.GetStringExtra("data");
            var dataObj = JsonConvert.DeserializeObject<dynamic>(data);

            Button sendButton = FindViewById<Button>(Resource.Id.SendButton);

            List<string> items = new List<string>();
            ListView msgListView = FindViewById<ListView>(Resource.Id.msgListView);
            ArrayAdapter ListAdapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, items);
            msgListView.Adapter = ListAdapter;
            
            foreach (var item in dataObj)
            {
                //item["nr"].Value;
                ListAdapter.Add(item["nr"].Value + " | " + item["cl"].Value + ": " + item["a"].Value);
            }
            ListAdapter.NotifyDataSetChanged();

            sendButton.Click += (sender, e) =>
            {
                using (SmsManager smsManager = SmsManager.Default)
                foreach (var item in dataObj)
                {
                    var nr = item["nr"].Value.ToString();
                    var msg = Helpers.RemoveDiacritics(item["msg"].Value.ToString());
                    SmsManager.Default.SendTextMessage(nr, null, msg, null, null);
                }

                Toast.MakeText(this, "Todas as mensagens foram enviadas com sucesso.", ToastLength.Long).Show();
            };
        }
    }
}