using Apps.Models;
using Xamarin.Forms;
using XFUniqueIdentifier.Droid;

[assembly: Dependency(typeof(AndroidDevice))]
namespace XFUniqueIdentifier.Droid
{
    public class AndroidDevice : IDevice
    {
        public string GetIdentifier()
        {
            return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
    }
}