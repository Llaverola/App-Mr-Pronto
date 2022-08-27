using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
[assembly: ExportFont("VisbyBold.otf", Alias = "MyCustomFont_Bold")]
[assembly: ExportFont("VisbyRegular.otf", Alias = "MyCustomFont_Regular")]
[assembly: ExportFont("VisbyMedium.otf", Alias = "MyCustomFont_Medium")]
[assembly: ExportFont("VisbyExtraBold.otf", Alias = "MyCustomFont_ExtraBold")]
[assembly: ExportFont("VisbySemibold.otf", Alias = "MyCustomFont_SemiBold")]