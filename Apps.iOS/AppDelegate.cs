using FFImageLoading.Forms.Platform;
using Foundation;
using ImageCircle.Forms.Plugin.iOS;
using System;
using UIKit;

namespace Apps.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //UIWindow window;
        //public static UIStoryboard Storyboard = UIStoryboard.FromName("LaunchScreen", null);
        //public static UIViewController initialViewController;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            //window = new UIWindow(UIScreen.MainScreen.Bounds);
            //initialViewController = Storyboard.InstantiateInitialViewController() as UIViewController;
            //window.RootViewController = initialViewController;
            //window.AddSubview(initialViewController.View);
            //window.MakeKeyAndVisible();
            ImageCircleRenderer.Init();
            CachedImageRenderer.Init();
            Rg.Plugins.Popup.Popup.Init();
            LoadApplication((App)Activator.CreateInstance(typeof(App)));
            return base.FinishedLaunching(app, options);
        }
    }
}
