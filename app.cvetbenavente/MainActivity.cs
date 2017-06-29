using Android.App;
using Android.Widget;
using Android.OS;

namespace app.cvetbenavente
{
    [Activity(Label = "CVETBENAVENTE", MainLauncher = true, Theme = "@android:style/Theme.Material.Light.NoActionBar", Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            EditText username = FindViewById<EditText>(Resource.Id.username);
            EditText password = FindViewById<EditText>(Resource.Id.password);

            Button login = FindViewById<Button>(Resource.Id.LoginButton);


            login.Click += (sender, e) =>
            {
                if (string.IsNullOrWhiteSpace(username.Text) || (string.IsNullOrWhiteSpace(password.Text)))
                {
                    Toast.MakeText(this, "Ambos os campos são obrigatórios.", ToastLength.Short).Show();
                }
                else
                {
                    login.Enabled = false;
                    string ak = Login.GetAccessKey(username.Text, password.Text);
                    login.Enabled = true;

                    if (string.IsNullOrWhiteSpace(ak))
                    {
                        AlertDialog.Builder builder = new AlertDialog.Builder(this);
                        builder.SetMessage("Login sem sucesso. Confirme as credenciais e certifique-se de que tem acesso à internet.")
                               .SetNeutralButton("OK", (c, ev) => { })
                               .Show();
                        
                    }
                }
            };
        }
    }
}

