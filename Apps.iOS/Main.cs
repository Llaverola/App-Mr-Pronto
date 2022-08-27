using System;
using UIKit;

namespace Apps.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        [Obsolete]
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
                UIApplication.SharedApplication.SetStatusBarHidden(true, true);
            }
            catch (Exception)
            {

            }
        }
    }
}
