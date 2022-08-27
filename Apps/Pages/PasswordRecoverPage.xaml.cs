using Apps;
using Apps.Models;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PasswordRecoverPage : ContentPage
    {
        public PasswordRecoverPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            PageTitle_Label.Text = "Recuperar a\nSua Password";
            ShowIndicator();
        }

        [Obsolete]
        private void Submit_Button_Clicked(object sender, EventArgs e)
        {
            DivSuccessMsg.IsVisible = false;
            DivErrorMsg.IsVisible = false;
            string email = Username.Text;
            if (string.IsNullOrEmpty(email))
            {
                DisplayAlert("Alerta", "Por favor, insira o seu email/username.", "OK");
            }
            else
            {
                Random r = new Random();
                int codigo = r.Next(100000, 999999);
                string novaPwd = codigo.ToString();
                ShowIndicator();

                Task<string> pResult = Task.Run(() => App.UtilizadoresManager.ClienteChangePwdPostAsync(new UtilizadorNovaPwd()
                {
                    email = email.ToLower(),
                    password = novaPwd
                }));

                if (pResult.Result == "1")
                {
                    Username.Text = "";
                    DivSuccessMsg.IsVisible = true;
                    ScrollToBottom();
                }
                else if (pResult.Result == "0")
                {
                    Username.Text = "";
                    DivErrorMsg.IsVisible = true;
                    ScrollToBottom();
                }
                else
                {
                    DisplayAlert("Alerta", "Ocorreu um erro. Tente novamente.", "OK");
                }
            }
        }

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

        [Obsolete]
        private void voltar_tapped(object sender, EventArgs e)
        {
            App.previousPage = this;
            App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
        }

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