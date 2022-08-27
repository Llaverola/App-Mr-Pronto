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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PedidoPage : ContentPage
    {
        private List<ImageModelItem> ImagensList { get; set; }
        [Obsolete]
        public PedidoPage()
        {
            InitializeComponent();
            BindDataUserOnline();
            ShowIndicator();
            ImagensList = new List<ImageModelItem>();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = this;
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            var item = App.MyOrder_Selected;
            order_number_label.Text = item.order_number;
            if (item.terminado)
            {
                Estado_Do_Pedido_Frame.BackgroundColor = Color.FromHex("#3BB538");
                Estado_Do_Pedido_Label.Text = "Pedido Terminado";
                falta_pagar_label.Text = "-";
            }
            else if (item.terminado == false && item.pagamento_validado == true)
            {
                Estado_Do_Pedido_Frame.BackgroundColor = Color.Yellow;
                Estado_Do_Pedido_Label.Text = "Pedido validado";
                falta_pagar_label.Text = Math.Round((double)item.total - (double)item.total * 0.15).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            }
            else if (item.terminado == false && item.pagamento_validado == false && !string.IsNullOrEmpty(item.comprovativo_de_pagamento_entrada_url))
            {
                Estado_Do_Pedido_Frame.BackgroundColor = Color.Red;
                Estado_Do_Pedido_Label.Text = "Em análise e validação";
                falta_pagar_label.Text =  "Pendente";
            }
            else if (item.terminado == false && item.pagamento_validado == false && string.IsNullOrEmpty(item.comprovativo_de_pagamento_entrada_url))
            {
                Estado_Do_Pedido_Frame.BackgroundColor = Color.Red;
                Estado_Do_Pedido_Label.Text = "A aguardar comprovativo de pagamento";
                falta_pagar_label.Text = item.total_str;
            }

            string servicos_str = "";
            if (item.servicos.Count == 1)
            {
                servicos_contratados_label.Text = item.servicos.First().nome + "\n" + item.servicos.First().descricao;
            }
            else
            {
                foreach (var servs in item.servicos)
                {
                    servicos_str += string.Format("{0} -> {1}\n", servs.nome, servs.descricao) + "\n\n";
                }
            }

            total_agora_label.Text = Math.Round((double)item.total * 0.15, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            total_label.Text = item.total_str;
           
            Envia_Comprovativo_Div.IsVisible = true;
            if (!string.IsNullOrEmpty(item.comprovativo_de_pagamento_entrada_url))
            {
                Envia_Comprovativo_Div_Ficheiro.IsVisible = false;
            }
            else
            {

                Envia_Comprovativo_Div_Ficheiro.IsVisible = true;
            }

            if (!string.IsNullOrEmpty(item.dataDoServico_String))
                data_servico_label.Text = item.dataDoServico_String;
            else
                data_servico_label.Text = "Por definir.";

            morada_servico_label.Text = item.morada;       
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

        [Obsolete]
        private void Ver_Comprovativo_tapped(object sender, EventArgs e)
        {
            bool UserIsOnline =  App.UserIsOnline;
            if (!UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new RegistoPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
            }
        }

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
                        if (ImagensList.Count == 0)
                        {
                            ImageModelItem _newImagem = new ImageModelItem()
                            {
                                imageSource = ImageSource.FromStream(() =>
                                {
                                    Stream stream = file.GetStream();
                                    file.Dispose();
                                    return stream;
                                }),
                                stream = file.GetStream(),
                                extension = "jpg"

                            };
                            ImagensList.Add(_newImagem);
                            Carregar_Ficheiro_Span.Text = " Comprovativo selecionado.";
                        }
                        else
                        {
                            await DisplayAlert("Limite de Fotos", "Apenas pode adicionar um ficheiro.", "OK");
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


        #region menu e botão voltar e loading indicator


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
        private void Voltar_Button_Clicked(object sender, EventArgs e)
        {
            App.MasterDetailPage.Detail = new NavigationPage(new MeusPedidosPage());
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
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
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
        private async void Button_Submit_Clicked(object sender, EventArgs e)
        {
            if (ImagensList.Count > 0)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.ComprovativoPost(
                                                                               new ComprovativoPostModel()
                                                                               {
                                                                                   order_number = App.MyOrder_Selected.order_number,
                                                                                   file = General.ReadFully(ImagensList.First().stream),
                                                                                   extension = "jpg"
                                                                               }));
                if (res.Result)
                {
                    LoadingPopupPage loadingpage = new LoadingPopupPage();
                    await PopupNavigation.PushAsync(loadingpage);
                    await Task.Delay(2000);
                    Task<List<MyOrder>> pedidos_task = Task.Run(() => App.CartDataManager.GetOrders());
                    List<MyOrder> MyOrders = pedidos_task.Result;
                    MyOrder order = MyOrders.Where(x => x.order_number == App.MyOrder_Selected.order_number).FirstOrDefault();
                    await PopupNavigation.RemovePageAsync(loadingpage);
                    if (order != null)
                    {
                        SetPopup_Msg();
                        sucesso_msg_label.Text = "Ficheiro enviado com sucesso!";
                        await Task.Delay(3000);
                        SetPopup_Msg();
                        App.MyOrder_Selected = order;
                        App.NavigateTo(false, typeof(MeusPedidosPage));
                    }
                    else
                    {
                        Carregar_Ficheiro_Span.Text = " Carregar ficheiro";
                        ImagensList = new List<ImageModelItem>();
                        await DisplayAlert("Alerta!", "Ocorreu um erro. Por favor, tente novamente.", "Ok");
                    }
                }
            }
            else
            {
                await DisplayAlert("Alerta!", "Por favor, tire uma fotografia ao comprovativo de pagamento.", "Ok");
            }
        }

        private async void SetPopup_Msg()
        {
            if (!popuplayout_sucesso_msg.IsVisible)
            {
                popuplayout_sucesso_msg.IsVisible = !popuplayout_sucesso_msg.IsVisible;
                popuplayout_sucesso_msg.AnchorX = 1;
                popuplayout_sucesso_msg.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => popuplayout_sucesso_msg.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => popuplayout_sucesso_msg.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(popuplayout_sucesso_msg, "popupScaleAnimation", 250);
                fadeAnimation.Commit(popuplayout_sucesso_msg, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    popuplayout_sucesso_msg.FadeTo(0, 200, Easing.SinInOut)
                  );

                popuplayout_sucesso_msg.IsVisible = !popuplayout_sucesso_msg.IsVisible;
            }
        }

        private void Limpar_Ficheiro_Button_Clicked(object sender, EventArgs e)
        {
            Carregar_Ficheiro_Span.Text = " Carregar ficheiro";
            ImagensList = new List<ImageModelItem>();
        }
    }
}