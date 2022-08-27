using CurvedPicker;
using CurvedPicker.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace CurvedPicker.iOS
{
    public class CustomPickerRenderer : PickerRenderer
    {
        private const UITextBorderStyle none = UITextBorderStyle.None;

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (Control == null)
            {
                return;
            }
            Control.Layer.BorderWidth = 0;
            Control.BorderStyle = none;
        }
    }
}