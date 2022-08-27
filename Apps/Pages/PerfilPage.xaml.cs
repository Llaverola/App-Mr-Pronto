using Apps;
using Apps.Models;
using Apps.Pages;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {

        [Obsolete]
        public PerfilPage()
        {
            InitializeComponent();
            PageTitle_Label.Text = "Editar Perfil";
            Sub_PageTitle_Label.Text = "Pode alterar aqui os seus dados";
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
            Primeiro_Nome_Textbox.Text = App.DataModel.Utilizador.Nome.Split(' ')[0].ToString();
            Ultimo_Nome_Textbox.Text = App.DataModel.Utilizador.Nome.Split(' ')[1].ToString();
            Email_Textbox.Text = App.DataModel.Utilizador.Email;
            Telemovel_Textbox.Text = App.DataModel.Utilizador.Telemovel;
            BindDataUserOnline();
        }

        [Obsolete]
        private void BindDataUserOnline()
        {
            Person_Img_LoggedIn.IsVisible = false;
            Iniciais_Frame_LoggedIn.IsVisible = false;

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
                perfil_image.Source = ImageSource.FromFile("noimage.png");
            }
            else
            {
                Person_Img_LoggedIn.IsVisible = true;
                Person_Img_LoggedIn.Source = ImageSource.FromStream(() => General.ByteArrayToStream(App.DataModel.Utilizador.FotoByteArray));
                Person_Img_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                perfil_image.Source = ImageSource.FromStream(() => General.ByteArrayToStream(App.DataModel.Utilizador.FotoByteArray));
            }

            var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
            Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();

            Rua_Textbox.Text = App.DataModel.Utilizador.Morada;
            Andar_Textbox.Text = App.DataModel.Utilizador.Andar;
            Numero_da_porta_Textbox.Text = App.DataModel.Utilizador.NumeroDaPorta;
            Localidade_Textbox.Text = App.DataModel.Utilizador.Cidade;

            areas_picker.ItemsSource = new List<string>() { "Luanda Centro", "Talatona", "Morro Bento", "Nova Vida" };
            areas_picker.SelectedItem = App.DataModel.Utilizador.Area;
        }

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

        [Obsolete]
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
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
            string telemovel = Telemovel_Textbox.Text;
            string area = areas_picker.SelectedItem.ToString();
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
            else
            {
                LoadingPopupPage loadingpage = new LoadingPopupPage();
                await PopupNavigation.PushAsync(loadingpage);
                await Task.Delay(2000);

                Task<bool> pResult = Task.Run(() => App.UtilizadoresManager.UtilizadorEditarDadosPostAsync(new UtilizadorEditarDados
                {
                    email = email,
                    nome = primeiro_nome + " " + ultimo_nome,
                    id = App.DataModel.Utilizador.UmbracoMemberId,
                    telemovel = telemovel,
                    andar = Andar_Textbox.Text,
                    rua = Rua_Textbox.Text,
                    area = areas_picker.SelectedItem.ToString(),
                    cidade = Localidade_Textbox.Text,
                    porta = Numero_da_porta_Textbox.Text
                }));

                if (pResult.Result)
                {
                    DivSuccessMsg.IsVisible = true;
                    App.DataModel.Utilizador.Nome = primeiro_nome + " " + ultimo_nome;

                    if (!App.DataModel.Utilizador.Email.Equals(email))
                    {
                        App.DataModel.Utilizador.Email = email;
                    }
                    if (!App.DataModel.Utilizador.Telemovel.Equals(telemovel))
                    {
                        App.DataModel.Utilizador.Telemovel = telemovel;
                    }

                    App.DataModel.Utilizador.Morada = Rua_Textbox.Text;
                    App.DataModel.Utilizador.Andar = Andar_Textbox.Text;
                    App.DataModel.Utilizador.NumeroDaPorta = Numero_da_porta_Textbox.Text;
                    App.DataModel.Utilizador.Cidade = Localidade_Textbox.Text;
                    App.DataModel.Utilizador.Area = areas_picker.SelectedItem.ToString();
                    await PopupNavigation.RemovePageAsync(loadingpage);
                }
                else
                {
                    DivErrorMsg.IsVisible = true;
                    await PopupNavigation.RemovePageAsync(loadingpage);
                }
                ScrollToBottom();
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


        #region Tirar fotos, escolher foto da galeria, gravar video e escolher video

        [Obsolete]
        private async void Button_Upload_Clicked(object sender, EventArgs e)
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Camera);
                if (status != PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Permission.Camera))
                    {
                        await DisplayAlert("Permissão da Câmera", "Permitir que a aplicação aceda à sua câmera", "OK");
                    }

                    Dictionary<Permission, PermissionStatus> results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Permission.Camera });
                    status = results[Permission.Camera];
                }

                if (status == PermissionStatus.Granted)
                {
                    await CrossMedia.Current.Initialize();
                    if (!CrossMedia.Current.IsTakePhotoSupported || !CrossMedia.Current.IsCameraAvailable)
                    {
                        await DisplayAlert("Ops", "Nenhuma câmera detectada.", "OK");

                        return;
                    }

                    MediaFile file = await CrossMedia.Current.TakePhotoAsync(
                        new StoreCameraMediaOptions
                        {
                            SaveToAlbum = true,
                            Directory = "Pictures"
                        });

                    if (file == null)
                        return;
                    else
                    {
                        Stream file_stream = file.GetStream();
                        byte[] file_bytes_array = General.ReadFully(file_stream);
                        Task<bool> envia_foto_task = Task.Run(() => App.UtilizadoresManager.UtilizadorUploadFotoPostAsync(new UtilizadorUploadFoto() { id = App.DataModel.Utilizador.UmbracoMemberId, fotoArray = file_bytes_array }));
                        bool envia_foto_task_result = envia_foto_task.Result;
                        if (!envia_foto_task_result)
                            await DisplayAlert("Alerta!", "Ocorreu um erro. Por favor tente novamente.", "OK");
                        else
                        {
                            App.DataModel.Utilizador.FotoByteArray = file_bytes_array;
                            TapGestureRecognizer GoToDefinicoes = new TapGestureRecognizer();
                            GoToDefinicoes.Tapped += (s, et) =>
                            {
                                App.previousPage = this;
                                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
                            };
                            Person_Img_LoggedIn.IsVisible = true;
                            Iniciais_Frame_LoggedIn.IsVisible = false;
                            Person_Img_LoggedIn.Source = ImageSource.FromStream(() => General.ByteArrayToStream(App.DataModel.Utilizador.FotoByteArray));
                            perfil_image.Source = ImageSource.FromStream(() => General.ByteArrayToStream(App.DataModel.Utilizador.FotoByteArray));
                            Person_Img_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                            file.Dispose();

                        }
                    }
                }
                else if (status != PermissionStatus.Unknown)
                {
                    await DisplayAlert("Câmera Negada", "Não pode continuar, por favor tente novamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.ToString(), "OK");
            }
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

        private void areas_picker_arrow_Clicked(object sender, EventArgs e)
        {
            areas_picker.Focus();
        }
    }
}