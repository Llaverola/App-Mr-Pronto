using MasterDetailPageNavigation;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Apps.Models
{
    public class ConnectivityTest
    {
        public ConnectivityTest()
        {
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            var access = e.NetworkAccess;
            //var profiles = e.ConnectionProfiles;
            if (access != NetworkAccess.Internet)
            {
                //Page t = (Page)Activator.CreateInstance(typeof(NoInternet));
                //await Application.Current.MainPage.Navigation.PushAsync(t, true);
            }
            else
            {
                Page t = (Page)Activator.CreateInstance(typeof(LoginPage));
                await Application.Current.MainPage.Navigation.PushAsync(t, true);
            }
        }
    }
}
