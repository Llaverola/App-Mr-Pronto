using Apps;
using Apps.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistoPage : ContentPage
    {
        public RegistoPage()
        {
            InitializeComponent();
            ShowIndicator();
            RegistoViewModel vm = (RegistoViewModel)Activator.CreateInstance(typeof(RegistoViewModel));
            BindingContext = vm;
            vm.ExibirAvisoDeCamposObrigatorios += () => DisplayAlert("Alerta!", "Todos os campos são de preenchimento obrigatório.", "OK");
            vm.ExibirBoasVindas += () => DisplayAlert("Bem-vindo!", "Registo efetuado com sucesso.", "OK");
            vm.GoToBottom += () =>
            {
                ScrollToBottom();
            };

            PrimeiroNome.Completed += (object sender, EventArgs e) =>
            {
                UltimoNome.Focus();
            };

            UltimoNome.Completed += (object sender, EventArgs e) =>
            {
                Username.Focus();
            };

            Username.Completed += (object sender, EventArgs e) =>
            {
                Password.Focus();
            };

            //Password.Completed += (object sender, EventArgs e) =>
            //{
            //    vm.SubmitCommand.Execute(null);
            //};
            NavigationPage.SetHasNavigationBar(this, false);

            areas_picker.ItemsSource = new List<string>() { "Luanda Centro", "Talatona", "Morro Bento", "Nova Vida" };
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

        private void areas_picker_arrow_Clicked(object sender, EventArgs e)
        {
            areas_picker.Focus();
        }
    }
}