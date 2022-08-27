using Android.Content;
using Android.Views;
using Apps.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
[assembly: ExportRenderer(typeof(ViewCell), typeof(ViewCellRendererForAndroid))]
namespace Apps.Droid
{
    public class ViewCellRendererForAndroid : ViewCellRenderer
    {
        private Android.Views.View _cellCore;
        private bool _selected = false;

        protected override Android.Views.View GetCellCore(Cell item, Android.Views.View convertView, ViewGroup parent, Context context)
        {
            _cellCore = base.GetCellCore(item, convertView, parent, context);
            return _cellCore;
        }

        protected override void OnCellPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs args)
        {
            base.OnCellPropertyChanged(sender, args);
            if (args.PropertyName == "IsSelected")
            {
                _selected = !_selected;
                var extendedViewCell = sender as ViewCell;
                if (_selected)
                    _cellCore.SetBackgroundColor(Android.Graphics.Color.LightGray);
                else
                    _cellCore.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}