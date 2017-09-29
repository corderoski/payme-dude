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
using PayMe.Apps.Services;
using Android.Net;
using Xamarin.Forms;
using PayMe.Apps.Models;
using System.Net.Http;
using System.Threading.Tasks;
using Java.Net;

[assembly: Dependency(typeof(PayMe.Apps.Droid.Helpers.DeviceConnectionService))]
namespace PayMe.Apps.Droid.Helpers
{

    public class DeviceConnectionService : IDeviceConnectionHelper
    {
        public async Task<bool> HasServerConnectionAsync(string hostServerUrl = "")
        {
            try
            {
                var httpClient = new HttpClient();
                httpClient.DefaultRequestHeaders.Add("X-ZUMO-AUTH", "2.0.0");
                httpClient.DefaultRequestHeaders.ConnectionClose = true;

                var response = await httpClient.GetAsync("http://google.com");
                return response.IsSuccessStatusCode;
            }
            catch (Exception) { return false; }
        }

        public Task<bool> IsNetworkConnectedAsync()
        {
            return Task.Run(() =>
            {
                bool isNetworkActive = false;
                ConnectivityManager cm = (ConnectivityManager)Xamarin.Forms.Forms.Context.GetSystemService(Context.ConnectivityService);

                try
                {
                    NetworkInfo activeConnection = cm.ActiveNetworkInfo;
                    isNetworkActive = (activeConnection != null) && activeConnection.IsConnected;
                }
                catch (Java.Lang.SecurityException)
                {
                    throw new DevicePermissionNotLocatedException();
                }

                return isNetworkActive;
            });
        }
    }
}