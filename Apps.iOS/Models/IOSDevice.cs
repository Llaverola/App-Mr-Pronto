using Apps.Models;
using UIKit;

namespace Apps.iOS.Models
{
    public class IOSDevice : IDevice
    {
        public string GetIdentifier()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.ToString();
        }
    }
}