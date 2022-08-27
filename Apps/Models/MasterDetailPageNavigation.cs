using System;
using Xamarin.Forms;

namespace MasterDetailPageNavigation
{
    public class MasterPageItem
    {
        public int Index { get; set; }
        public string Title { get; set; }
        public string IconSource { get; set; }
        public FontAttributes FontAttributes { get; set; }
        public Color TextColor { get; set; }
        public Color BackgroundColor { get; set; }

        public bool RatingIsVisible { get; set; }
        public double RowHeight { get; set; }
        public double PaddingLeft { get; set; }
    }
}
