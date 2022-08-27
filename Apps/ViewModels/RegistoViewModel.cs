using Apps.Models;
using Apps.Pages;
using MasterDetailPageNavigation;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Apps.ViewModels
{
    public class RegistoViewModel : INotifyPropertyChanged
    {
        #region Variáveis e construtor

        public Action ExibirAvisoDeCamposObrigatorios;
        public Action GoToBottom;
        public Action ExibirBoasVindas;
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        private bool _loadingActivator;
        public bool LoadingActivator
        {
            get { return _loadingActivator; }
            set
            {
                _loadingActivator = value;
                PropertyChanged(this, new PropertyChangedEventArgs("LoadingActivator"));
            }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Password"));
            }
        }

        private List<string> _areas;
        public List<string> Areas
        {
            get { return _areas; }
            set
            {
                _areas = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Areas"));
            }
        }

        private string _area;
        public string Area
        {
            get { return _area; }
            set
            {
                _area = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Area"));
            }
        }

        private string _primeiroNome;
        public string PrimeiroNome
        {
            get { return _primeiroNome; }
            set
            {
                _primeiroNome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PrimeiroNome"));
            }
        }

        private string _ultimoNome;
        public string UltimoNome
        {
            get { return _ultimoNome; }
            set
            {
                _ultimoNome = value;
                PropertyChanged(this, new PropertyChangedEventArgs("UltimoNome"));
            }
        }

        private bool _showError;
        public bool ShowError
        {
            get { return _showError; }
            set
            {
                _showError = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowError"));
            }
        }

        private bool _showSuccess;
        public bool ShowSuccess
        {
            get { return _showSuccess; }
            set
            {
                _showSuccess = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ShowSuccess"));
            }
        }



        public ICommand SubmitCommand { protected set; get; }

        [Obsolete]
        public RegistoViewModel()
        {
            LoadingActivator = false;
            ShowSuccess = false;
            ShowError = false;
            Username = "";
            Password = "";
            Area = "";
            Areas = new List<string>() { "Luanda Centro", "Talatona", "Morro Bento", "Nova Vida" };
            SubmitCommand = new Command(async () => await OnSubmit());
            ShowIndicator();
        }

        private async void ShowIndicator()
        {
            LoadingActivator = true;
            await Task.Delay(3000);
            LoadingActivator = false;
        }

        #endregion

        #region events

        [Obsolete]
        public async Task OnSubmit()
        {
            ShowSuccess = false;
            ShowError = false;

            if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password) && string.IsNullOrEmpty(Area) && string.IsNullOrEmpty(PrimeiroNome) && string.IsNullOrEmpty(UltimoNome))
                ExibirAvisoDeCamposObrigatorios();
            else
            {
                LoadingPopupPage loadingpage = new LoadingPopupPage();
                await PopupNavigation.PushAsync(loadingpage);
                await Task.Delay(2000);
                DataModel dm = await App.UtilizadoresManager.SaveClientePostAsync(new UtilizadorRegisto() { deviceId = App.DeviceIdentifier, email = Username, password = Password, area = Area, nome = PrimeiroNome + " " + UltimoNome });
                if (dm.Utilizador.UmbracoMemberId == 0)
                {
                    ShowError = true;
                    GoToBottom();
                    await PopupNavigation.RemovePageAsync(loadingpage);
                }
                else
                {
                    App.UserIsOnline = true;
                    await PopupNavigation.RemovePageAsync(loadingpage);
                    ExibirBoasVindas();
                    Username = "";
                    Password = "";
                    Area = "";
                    //ShowSuccess = true;
                    GoToBottom();
                    await Task.Delay(2000);
                    App.DataModel = dm;
                    App.UpdateListView();
                    App.NavigateTo(false, typeof(PerfilPage));
                }
            }
        }
        #endregion
    }
}