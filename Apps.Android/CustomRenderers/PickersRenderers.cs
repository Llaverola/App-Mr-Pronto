using Android.Content;
using CurvedPicker;
using CurvedPicker.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(PickersRenderers))]
namespace CurvedPicker.Droid
{
    public class PickersRenderers : Xamarin.Forms.Platform.Android.AppCompat.PickerRenderer
    {
        public PickersRenderers(Context context) : base(context)
        {

        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.SetBackground(null);
            }
        }  

    }
}