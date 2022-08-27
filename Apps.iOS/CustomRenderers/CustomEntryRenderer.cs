using CurvedEntry;
using CurvedEntry.iOS;
using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace CurvedEntry.iOS
{
    public class CustomEntryRenderer : EntryRenderer
    {
        //[Obsolete]
        //protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        //{
        //    base.OnElementChanged(e);

        //    if (e.NewElement != null)
        //    {
        //        var view = (CustomEntry)Element;

        //        Control.LeftView = new UIView(new CGRect(0f, 0f, 9f, 20f));
        //        Control.LeftViewMode = UITextFieldViewMode.Always;

        //        Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
        //        Control.ReturnKeyType = UIReturnKeyType.Done;

        //        Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
        //        Control.Layer.BorderColor = view.BorderColor.ToCGColor();
        //        Control.Layer.BorderWidth = view.BorderWidth;
        //        Control.ClipsToBounds = true;
        //    }
        //}
        [Obsolete]
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                var view = (CustomEntry)Element;
                if (view.ShowBorder)
                {
                    Control.LeftView = new UIView(new CoreGraphics.CGRect(0f, 0f, 9f, 20f));
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.KeyboardAppearance = UIKeyboardAppearance.Dark;
                    Control.ReturnKeyType = UIReturnKeyType.Done;

                    if (view.IsCurvedCornersEnabled)
                    {
                        // Radius for the curves  
                        Control.Layer.CornerRadius = Convert.ToSingle(view.CornerRadius);
                    }

                    // Thickness of the Border Color  
                    Control.Layer.BorderColor = view.BorderColor.ToCGColor();

                    // Thickness of the Border Width  
                    Control.Layer.BorderWidth = view.BorderWidth;

                    Control.ClipsToBounds = true;
                }
            }
        }
    }
}