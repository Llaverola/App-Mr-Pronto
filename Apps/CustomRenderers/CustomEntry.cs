using Xamarin.Forms;

namespace CurvedEntry
{
    public class CustomEntry : Entry
    {
        //public static readonly BindableProperty BorderColorProperty =
        //BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEntry), Color.Gray);
        //// Gets or sets BorderColor value  
        //public Color BorderColor
        //{
        //    get => (Color)GetValue(BorderColorProperty);
        //    set => SetValue(BorderColorProperty, value);
        //}

        //public static readonly BindableProperty BorderWidthProperty =
        //BindableProperty.Create("BorderWidth", typeof(int), typeof(CustomEntry));

        //// Gets or sets BorderWidth value  
        //public int BorderWidth
        //{
        //    get => (int)GetValue(BorderWidthProperty);
        //    set => SetValue(BorderWidthProperty, value);
        //}

        //public static readonly BindableProperty CornerRadiusProperty =
        //BindableProperty.Create("CornerRadius", typeof(double), typeof(CustomEntry), Device.RuntimePlatform == Device.iOS ? 6 : 7);

        //// Gets or sets CornerRadius value  
        //public double CornerRadius
        //{
        //    get => (double)GetValue(CornerRadiusProperty);
        //    set => SetValue(CornerRadiusProperty, value);
        //}
        //public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        //BindableProperty.Create(nameof(IsCurvedCornersEnabled),
        //    typeof(bool), typeof(CustomEntry), true);
        //// Gets or sets IsCurvedCornersEnabled value  
        //public bool IsCurvedCornersEnabled
        //{
        //    get => (bool)GetValue(IsCurvedCornersEnabledProperty);
        //    set => SetValue(IsCurvedCornersEnabledProperty, value);
        //}

        public CustomEntry()
        {
            FontSize = 20;
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(Entry), Color.Black);
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        [System.Obsolete]
        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(Entry), Device.OnPlatform<int>(1, 2, 2));

        [System.Obsolete]
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        [System.Obsolete]
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(Entry), Device.OnPlatform<double>(6, 7, 7));

        [System.Obsolete]
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty = BindableProperty.Create(nameof(IsCurvedCornersEnabled), typeof(bool), typeof(Entry), false);
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }

        public static readonly BindableProperty ShowBorderProperty = BindableProperty.Create("ShowBorder", typeof(bool), typeof(Entry), false);
        public bool ShowBorder
        {
            get { return (bool)GetValue(ShowBorderProperty); }
            set { SetValue(ShowBorderProperty, value); }
        }
    }
}


namespace CurvedEditor
{
    public class CustomEditor : Editor
    {
        public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(CustomEditor), Color.FromHex("#F0F0F0"));
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty =
        BindableProperty.Create("BorderWidth", typeof(int), typeof(CustomEditor));

        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create("CornerRadius", typeof(double), typeof(CustomEditor), Device.RuntimePlatform == Device.iOS ? (double)6 : (double)7);

        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCurvedCornersEnabledProperty =
        BindableProperty.Create(nameof(IsCurvedCornersEnabled),
            typeof(bool), typeof(CustomEditor), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCurvedCornersEnabled
        {
            get => (bool)GetValue(IsCurvedCornersEnabledProperty);
            set => SetValue(IsCurvedCornersEnabledProperty, value);
        }
    }
}
