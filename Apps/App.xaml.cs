using Apps.Models;
using Apps.Services.CartData;
using Apps.Services.NotificacoesData;
using Apps.SettingsData.Utilizadores;
using MasterDetailPageNavigation;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Apps
{
    public partial class App : Application
    {
        #region Variáveis globais da app e construtor
        public static MobileDataManager MobileDataManager { get; private set; }
        public static MyCart FinishOrder_Item { get; set; }
        public static MyOrder MyOrder_Selected { get; set; }
        public static CartDataManager CartDataManager { get; private set; }
        public static UtilizadoresManager UtilizadoresManager { get; private set; }
        //public static ConnectivityTest ConnectivityTest { get; set; }
        public static string DeviceIdentifier { get; set; }
        [Obsolete]
        public static MasterDetailPage MasterDetailPage { get; set; }
        public static DataModel DataModel { get; set; }
        public static int SubCategoriaIdSelected { get; set; }

        public static string Order_Number { get; set; }
        public static Notificacao NotificacaoSelected { get; set; }
        public static Categoria Categoria_Nivel_1_Selected { get; set; }
        public static Categoria Categoria_Nivel_2_Selected { get; set; }
        public static Categoria Categoria_Nivel_3_Selected { get; set; }
        public static SubCategoria SubCategoria_Selected { get; set; }
        public static Page previousPage { get; set; }

        public static bool UserIsOnline { get; set; }

        [Obsolete]
        public App()
        {
            InitializeComponent();
            Order_Number = "";
            FinishOrder_Item = new MyCart();
            MyOrder_Selected = new MyOrder();
            NotificacaoSelected = new Notificacao();
            MasterDetailPage = new MasterDetailPage();
            MobileDataManager = (MobileDataManager)Activator.CreateInstance(typeof(MobileDataManager), new MobileDataService());
            CartDataManager = (CartDataManager)Activator.CreateInstance(typeof(CartDataManager), new CartDataService());
            UtilizadoresManager = (UtilizadoresManager)Activator.CreateInstance(typeof(UtilizadoresManager), new UtilizadoresService());
            DeviceIdentifier = Preferences.Get("my_deviceId", string.Empty);
            if (string.IsNullOrWhiteSpace(DeviceIdentifier))
            {
                DeviceIdentifier = Guid.NewGuid().ToString();
                Preferences.Set("my_deviceId", DeviceIdentifier);
            }
            DataModel = GetData();
            UserIsOnline = false;
            if (DataModel.Utilizador != null)
            {
                if(DataModel.Utilizador.UmbracoMemberId > 0)
                {
                    UserIsOnline = true;
                }
            }
            MasterDetailPage = new MainPage();
            Categoria_Nivel_1_Selected = new Categoria();
            Categoria_Nivel_2_Selected = new Categoria();
            Categoria_Nivel_3_Selected = new Categoria();
            SubCategoria_Selected = new SubCategoria();
            MainPage = (NavigationPage)Activator.CreateInstance(typeof(NavigationPage), MasterDetailPage);
        }

        #endregion

        #region Funções auxiliares

        [Obsolete]
        public static void NavigateTo(string Title, bool isPresented, Type type)
        {
            MasterDetailPage.Title = Title;
            MasterDetailPage.Detail = new NavigationPage((Page)Activator.CreateInstance(type));
            MasterDetailPage.IsPresented = isPresented;
        }
        [Obsolete]
        public static void NavigateTo(bool isPresented, Type type)
        {
            MasterDetailPage.Detail = new NavigationPage((Page)Activator.CreateInstance(type));
            MasterDetailPage.IsPresented = isPresented;
        }
        [Obsolete]
        public static void ShowMenu()
        {
            MasterDetailPage.IsPresented = true;
        }
        [Obsolete]
        public static void HideMenu()
        {
            MasterDetailPage.IsPresented = false;
        }
        [Obsolete]
        public static void NavigateToSubCategoria(bool isPresented, Type type, int parameter)
        {
            SubCategoriaIdSelected = parameter;
            MasterDetailPage.Detail = new NavigationPage((Page)Activator.CreateInstance(type));
            MasterDetailPage.IsPresented = isPresented;
        }

        [Obsolete]
        public static void UpdateListView()
        {
            MenuModel menu = new MenuModel();
            MasterPage masterPage = (MasterPage)MasterDetailPage.FindByName("masterPage");
            ListView listView = (ListView)masterPage.FindByName("listView");
            listView.ItemsSource = menu.Items;
        }

        private static DataModel GetData()
        {
            Task<DataModel> p = Task.Run(() => MobileDataManager.DataGetAsync());
            return p.Result;
        }

        #endregion

        #region Funções Nativas
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
        #endregion
    }
}