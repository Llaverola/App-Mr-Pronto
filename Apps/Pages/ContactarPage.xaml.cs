using Apps;
using Apps.Models;
using Apps.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ContactarPage : ContentPage
    {
        [Obsolete]
        public ContactarPage()
        {
            InitializeComponent();
            PageTitle_Label.Text = "Chat";
            Envie_Mensagem_Label.Text = "Envie uma mensagem e a nossa equipa irá responder assim que for possível";
            ShowIndicator();
            BindDataUserOnline();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        [Obsolete]
        private void icon_menu_button_Clicked(object sender, EventArgs e)
        {
            if (App.MasterDetailPage.IsPresented)
            {
                App.HideMenu();
            }
            else
            {
                App.ShowMenu();
            }
        }

        [Obsolete]
        private void BindDataUserOnline()
        {
            Person_Img_LoggedIn.IsVisible = false;
            Iniciais_Frame_LoggedIn.IsVisible = false;
            User_Label_Not_LoggedIn.IsVisible = false;

            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                User_Label_Not_LoggedIn.IsVisible = true;
            }
            else
            {
                TapGestureRecognizer GoToDefinicoes = new TapGestureRecognizer();
                GoToDefinicoes.Tapped += (s, e) =>
                {
                    App.previousPage = this;
                    App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
                };

                if (App.DataModel.Utilizador.FotoByteArray == null)
                {
                    Iniciais_Frame_LoggedIn.IsVisible = true;
                    Iniciais_Label.Text = App.DataModel.Utilizador.Nome.ToCharArray()[0].ToString();
                    Iniciais_Frame_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                }
                else
                {
                    Person_Img_LoggedIn.IsVisible = true;
                    Person_Img_LoggedIn.Source = ImageSource.FromStream(() => General.ByteArrayToStream(App.DataModel.Utilizador.FotoByteArray));
                    Person_Img_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                }

                var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
            }
        }

        #region menu

        [Obsolete]
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            App.MasterDetailPage.Detail = new NavigationPage(App.previousPage);
        }

        [Obsolete]
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
        }

        [Obsolete]
        private void tapcart_Tapped(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new CartPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
            }
        }

        [Obsolete]
        private void User_Tapped(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
            }
        }

        [Obsolete]
        private void Wishlist_Tapped(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new FavoritosPage());
            }
        }

        [Obsolete]
        private void yellowBoxView_Clicked(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new CartPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        #endregion


        [Obsolete]
        private async void Submit_Button_Clicked(object sender, EventArgs e)
        {
            DivSuccessMsg.IsVisible = false;
            DivErrorMsg.IsVisible = false;
            string primeiro_nome = Primeiro_Nome_Textbox.Text;
            string ultimo_nome = Ultimo_Nome_Textbox.Text;
            string email = Email_Textbox.Text;
            string msg = Msg_Textbox.Text;

            if (string.IsNullOrEmpty(primeiro_nome))
            {
                await DisplayAlert("Alerta", "Insira o seu primeiro nome.", "OK");
            }
            else if (string.IsNullOrEmpty(ultimo_nome))
            {
                await DisplayAlert("Alerta", "Insira o seu último nome.", "OK");
            }
            else if (string.IsNullOrEmpty(email))
            {
                await DisplayAlert("Alerta", "Insira o seu email.", "OK");
            }
            else if (string.IsNullOrEmpty(msg))
            {
                await DisplayAlert("Alerta", "Escreva a sua mensagem.", "OK");
            }
            else
            {
                LoadingPopupPage loadingpage = new LoadingPopupPage();
                await PopupNavigation.PushAsync(loadingpage);
                await Task.Delay(2000);

                Task<bool> pResult = Task.Run(() => App.MobileDataManager.PedidoDeContactoPostAsync(new PedidoDeContactoPostItem()
                {
                    email = email,
                    primeiro_nome = primeiro_nome,
                    ultimo_nome = ultimo_nome,
                    mensagem = msg
                }));

                Primeiro_Nome_Textbox.Text = "";
                Ultimo_Nome_Textbox.Text = "";
                Email_Textbox.Text = "";
                if (pResult.Result)
                {
                    DivSuccessMsg.IsVisible = true;
                }
                else
                {
                    DivErrorMsg.IsVisible = true;
                }
                ScrollToBottom();
                await PopupNavigation.RemovePageAsync(loadingpage);
            }
        }

        #region Indicator loading
        public async void ShowIndicator()
        {
            try
            {
                Show();
                await Task.Delay(2000);
                Hide();
            }
            catch (Exception)
            {
                Hide();
            }
        }

        public void Show()
        {
            loadingActivator.IsRunning = true;
            loadingActivator.IsVisible = true;
        }

        public void Hide()
        {
            loadingActivator.IsRunning = false;
            loadingActivator.IsVisible = false;
        }

        #endregion

        private void ScrollToBottom()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    await scrollView.ScrollToAsync(0, stkMain.Height, true);
                }
                else
                {
                    await Task.Delay(10); //UI will be updated by Xamarin
                    await scrollView.ScrollToAsync(stkMain, ScrollToPosition.End, true);
                }

            });
        }
    }
}