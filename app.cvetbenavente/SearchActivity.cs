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

namespace app.cvetbenavente
{
    [Activity(Label = "CVETBENAVENTE", Theme = "@android:style/Theme.Material.Light")]
    public class SearchActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Search);

            string username = Intent.GetStringExtra("username");
            string password = Intent.GetStringExtra("password");

            DateTime from = new DateTime();
            DateTime to = new DateTime();

            bool issetFrom = false;
            bool issetTo = false;

            Button dataInicialButton = FindViewById<Button>(Resource.Id.dataInicialButton);
            Button dataFinalButton = FindViewById<Button>(Resource.Id.dataFinalButton);

            Button searchButton = FindViewById<Button>(Resource.Id.SearchButton);

            //dataInicialButton click
            dataInicialButton.Click += (sender, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    dataInicialButton.Text = time.ToLongDateString();
                    from = time;
                    issetFrom = true;
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            //dataFinalButton click
            dataFinalButton.Click += (sender, e) =>
            {
                DatePickerFragment frag = DatePickerFragment.NewInstance(delegate (DateTime time)
                {
                    dataFinalButton.Text = time.ToLongDateString();
                    to = time;
                    issetTo = true;
                });
                frag.Show(FragmentManager, DatePickerFragment.TAG);
            };

            //PESQUISAR
            searchButton.Click += (sender, e) =>
            {
                if (issetFrom == false || issetTo == false)
                {
                    Toast.MakeText(this, "Ambos os campos são obrigatórios.", ToastLength.Short).Show();
                }
                else
                {
                    searchButton.Enabled = false;
                    var ak = Login.GetAccessKey(username, password);

                    if (string.IsNullOrWhiteSpace(ak))
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetMessage("Ocorreu um erro ao efetuar a autenticação. Certifique-se que tem acesso à internet.")
                               .SetNeutralButton("OK", (c, ev) => { })
                               .Show();
                        searchButton.Enabled = true;
                    }
                    else
                    {
                        var result = Login.GetEvents(ak, from, to);

                        if (string.IsNullOrWhiteSpace(result))
                        {
                            AlertDialog.Builder builder = new AlertDialog.Builder(this);
                            builder.SetMessage("Não foram recebidos dados. Por favor tente novamente.")
                                   .SetNeutralButton("OK", (c, ev) => { })
                                   .Show();

                            searchButton.Enabled = true;
                        } else
                        {
                            var _viewActivity = new Intent(this, typeof(ViewActivity));
                            _viewActivity.PutExtra("data", result);

                            searchButton.Enabled = true;

                            StartActivity(_viewActivity);
                        }
                    }
                }
            };
        }
    }
}