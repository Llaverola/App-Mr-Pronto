using Apps.Models;
using Apps.Pages;
using MasterDetailPageNavigation;
using Rg.Plugins.Popup.Services;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
namespace Apps.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        #region Variáveis e construtor

        public Action ExibirAvisoDeLoginInvalido;
        public Action ExibirAvisoDeLoginValido;
        public Action GoToBottom;
        public Action ExibirAvisoDeCamposObrigatorios;
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
                //OnPropertyChanging("Username");
                PropertyChanged(this, new PropertyChangedEventArgs("Username"));
                //PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs("Username"));
            }
        }

        private bool _imgUsernameIsVisible;
        public bool ImgUsernameIsVisible
        {
            get { return _imgUsernameIsVisible; }
            set
            {
                _imgUsernameIsVisible = value;
                PropertyChanged(this, new PropertyChangedEventArgs("ImgUsernameIsVisible"));
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

        public ICommand SubmitCommand { protected set; get; }
        public ICommand ImgUsernameCommand { protected set; get; }

        [Obsolete]
        public LoginViewModel()
        {
            SubmitCommand = new Command(async () => await OnSubmit());
            ImgUsernameCommand = new Command(imgUsername_Clicked);
            LoadingActivator = false;
            ImgUsernameIsVisible = false;
        }

        #endregion

        #region events
        private void imgUsername_Clicked()
        {
            Username = "";
            ImgUsernameIsVisible = false;
        }

        [Obsolete]
        public async Task OnSubmit()
        {
            try
            {
                if (string.IsNullOrEmpty(Username) && string.IsNullOrEmpty(Password))
                    ExibirAvisoDeCamposObrigatorios();
                else
                {
                    LoadingPopupPage loadingpage = new LoadingPopupPage();
                    await PopupNavigation.PushAsync(loadingpage);
                    await Task.Delay(2000);

                    Utilizador dm = await App.UtilizadoresManager.LoginPostAsync(Username, Password);
                    if (dm.UmbracoMemberId == 0)
                    {
                        LoadingActivator = false;
                        ExibirAvisoDeLoginInvalido();
                        GoToBottom();
                        await PopupNavigation.RemovePageAsync(loadingpage);
                    }
                    else
                    {
                        App.UserIsOnline = true;
                        App.DataModel.Utilizador = dm;
                        App.UpdateListView();
                        await PopupNavigation.RemovePageAsync(loadingpage);
                        App.NavigateTo(false, typeof(DefinicoesPage));
                    }
                }
            }
            catch(Exception)
            {

            }
          
        }
        #endregion
    }
}