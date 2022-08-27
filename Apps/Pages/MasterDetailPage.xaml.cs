using Apps.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public MasterPage()
        {
            InitializeComponent();
            MenuModel menu = new MenuModel();
            listView.ItemsSource = menu.Items;
            BindingContext = this;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ViewCell_Appearing(object sender, EventArgs e) { var viewCell = sender as ViewCell; UpdateListViewHeight(viewCell); }

        private void UpdateListViewHeight(ViewCell viewCell)
        {
            try
            {
                double height = 0;
                foreach (var cell in listView.ItemsSource)
                {
                    height += 30;
                }
                listView.HeightRequest = height;
            }
            catch { }
        }
    }
}