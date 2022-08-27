using System.IO;
using Xamarin.Forms;

namespace Apps.Models
{
    class ImageModelItem
    {
        public ImageSource imageSource { get; set; }
        public Stream stream { get; set; }
        public string extension { get; set; }
    }
}
