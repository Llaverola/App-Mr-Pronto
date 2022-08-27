using Apps;
using Apps.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            ShowIndicator();
            LoginViewModel vm = (LoginViewModel)Activator.CreateInstance(typeof(LoginViewModel));
            BindingContext = vm;
            vm.ExibirAvisoDeLoginInvalido += () => DisplayAlert("Erro", "Login Inválido ou sem permissão, tente novamente.", "OK");
            vm.ExibirAvisoDeLoginValido += () => DisplayAlert("Bem-Vindo", "Login efetuado com sucesso.", "OK");
            vm.ExibirAvisoDeCamposObrigatorios += () => DisplayAlert("Erro", "Insira o username e password.", "OK");       
            vm.GoToBottom += () =>
            {
                ScrollToBottom();
            };

            Username.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            Password.Completed += (object sender, EventArgs e) =>
            {
                vm.SubmitCommand.Execute(null);
            };
            NavigationPage.SetHasNavigationBar(this, false);

            Registo_Label.Text = "Ainda não tem uma conta?\nRegiste-se aqui";
        }

        private void Username_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (Username.Text != null)
            {
                if (Username.Text.Length > 0)
                {
                    imgUsername.IsVisible = true;
                }
                else
                {
                    imgUsername.IsVisible = false;
                }
            }
        }

        private void Password_PropertyChanging(object sender, PropertyChangingEventArgs e)
        {
            if (Password.Text != null)
            {
                if (Password.Text.Length > 0)
                {
                    imgPassword.IsVisible = true;
                }
                else
                {
                    imgPassword.IsVisible = false;
                }
            }
        }

        private void imgPassword_Clicked(object sender, EventArgs e)
        {
            Password.Text = "";
            imgPassword.IsVisible = false;
        }

        private void cb_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (Password.IsPassword)
            {
                Password.IsPassword = false;
            }
            else
            {
                Password.IsPassword = true;
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
        private void VoltarButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
        }

        [Obsolete]
        private void Recuperacao_Pwd_Tapped(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                App.NavigateTo(false, typeof(PasswordRecoverPage));
            }
        }

        [Obsolete]
        private void Esquecime_da_pwd_tapped(object sender, EventArgs e)
        {
            App.previousPage = this;
            App.MasterDetailPage.Detail = new NavigationPage(new PasswordRecoverPage());
        }

        [Obsolete]
        private void Registo_tapped(object sender, EventArgs e)
        {
            App.previousPage = this;
            App.MasterDetailPage.Detail = new NavigationPage(new RegistoPage());
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