using Apps;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MasterDetailPageNavigation
{
    [DesignTimeVisible(false)]
    [Obsolete]
    public partial class MainPage : MasterDetailPage
    {
        public MainPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            masterPage.listView.ItemSelected += OnItemSelected;
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterPageItem;
            if (item != null)
            {
                if (item.Index > -1)
                {
                    ((ListView)sender).SelectedItem = null;
                    if (item.Index == 0)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage))); //homepage
                    else if (item.Index == 1)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(DefinicoesPage))); //homepage
                    else if (item.Index == 2)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CartPage))); //DefinicoesPage
                    else if (item.Index == 3)
                    {
                        App.UserIsOnline = false;
                        App.DataModel.Definicoes = new Apps.Models.Definicoes();
                        App.DataModel.Utilizador = null;
                        App.UpdateListView();
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
                    }
                    else if (item.Index == 4)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(RegistoPage)));
                    else if (item.Index == 5)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(LoginPage))); //DefinicoesPage
                    else if (item.Index == 6)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(OutrosContactosPage)));
                    else if (item.Index == 7)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(ContactarPage)));
                    else if (item.Index == 8)
                        Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(FaqsPage)));
                    else if (item.Index == 9)
                    {
                        Device.BeginInvokeOnMainThread(async () => {
                            var result = await this.DisplayAlert("Atenção !", "Tem a certeza que pretende eliminar a sua conta de forma permanente?", "Sim", "Não");
                            if (result)
                            {
                                Task<bool> EliminarContaTask = Task.Run(() => App.MobileDataManager.EliminarConta());
                                App.UserIsOnline = false;
                                App.DataModel.Definicoes = new Apps.Models.Definicoes();
                                App.DataModel.Utilizador = null;
                                App.UpdateListView();
                                Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(HomePage)));
                            }
                        });
                    }
                    //Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(FaqsPage)));
                    IsPresented = false;

                }
            }
        }
    }
}