using Android.App;
using Android.Content.PM;
using Android.OS;

using Microsoft.WindowsAzure.MobileServices;

using PayMe.Apps.Services;

using System.Threading.Tasks;

namespace PayMe.Apps.Droid
{
    [Activity(Label = "@string/app_name", Theme = "@style/splashscreen", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, IAuthentication
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.SetTheme(Resource.Style.MyTheme);
            base.OnCreate(bundle);

            SQLitePCL.Batteries_V2.Init();
            CurrentPlatform.Init();
            Xamarin.Forms.Forms.Init(this, bundle);

            App.Init((IAuthentication)this);
            LoadApplication(new App());
        }

        public async Task<AuthenticationResult> Authenticate(MobileServiceAuthenticationProvider authenticationProvider)
        {
            AuthenticationResult authenticationResult = null;
            try
            {
                var user = await Data.PayMeDataStore.DefaultDataStore.CurrentClient.LoginAsync(this, authenticationProvider);
                if (user != null)
                {
                    System.Diagnostics.Debug.WriteLine($"SIGNNED IN: {user.UserId} | {user.MobileServiceAuthenticationToken}");
                    authenticationResult = new AuthenticationResult
                    {
                        Success = true,
                        UserId = user.UserId,
                    };
                }
            }
            catch (System.Exception ex)
            {
                AlertDialog.Builder builder = new AlertDialog.Builder(this);
                builder.SetMessage(ex.Message);
                builder.SetTitle("Bad Signin");
                var alert = builder.Create();
                alert.Show();

                authenticationResult = new AuthenticationResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }

            return authenticationResult;
        }

    }
}