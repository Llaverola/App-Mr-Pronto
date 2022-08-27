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
    public partial class ChangePasswordPage : ContentPage
    {
        [Obsolete]
        public ChangePasswordPage()
        {
            InitializeComponent();
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
            BindDataUserOnline();
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
                    string[] nome = App.DataModel.Utilizador.Nome.Split(' ');
                    Iniciais_Label.Text = nome[0].ToCharArray()[0].ToString() + "" + nome[1].ToCharArray()[0].ToString();
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
                App.MasterDetailPage.Detail = new NavigationPage(new FavoritosPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
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


        #region menu e botão voltar

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
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
        }

        #endregion

        [Obsolete]
        private async void Submit_Button_Clicked(object sender, EventArgs e)
        {
            DivSuccessMsg.IsVisible = false;
            DivErrorMsg.IsVisible = false;
            string text = Button_Submit.Text;
            string current_pwd = Password_Atual_Textbox.Text;
            string pwd = Nova_Password_Textbox.Text;
            string pwd_confirm = Nova_Password_Confirm_Textbox.Text;

            if (!current_pwd.Equals(Apps.Models.General.Decrypt(App.DataModel.Utilizador.PasswordEncrypted)))
            {
                await DisplayAlert("Alerta", "A password atual inserida não está correta.Tente novamente.", "OK");
            }
            else if (string.IsNullOrEmpty(pwd))
            {
                await DisplayAlert("Alerta", "Insira a nova password.", "OK");
            }
            else if (string.IsNullOrEmpty(pwd_confirm))
            {
                await DisplayAlert("Alerta", "Confirme a sua nova password.", "OK");
            }
            else if (!pwd.Equals(pwd_confirm))
            {
                await DisplayAlert("Alerta", "A password e confirmação da mesma têm de coincidir.", "OK");
            }
            else
            {
                LoadingPopupPage loadingpage = new LoadingPopupPage();
                await PopupNavigation.PushAsync(loadingpage);
                await Task.Delay(2000);

                Task<string> pResult = Task.Run(() => App.UtilizadoresManager.ClienteChangePwd2PostAsync(pwd));
                Password_Atual_Textbox.Text = "";
                Nova_Password_Textbox.Text = "";
                Nova_Password_Confirm_Textbox.Text = "";
                if (pResult.Result == "1")
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