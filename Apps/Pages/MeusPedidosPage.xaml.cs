using Apps;
using Apps.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusPedidosPage : ContentPage
    {
        private List<ImageModelItem> ImagensList { get; set; }
        private List<MyOrder> MyOrders { get; set; }
        //IDownloader downloader = DependencyService.Get<IDownloader>();
        [Obsolete]
        public MeusPedidosPage()
        {
            InitializeComponent();
            BindDataUserOnline();
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
            var pedidos_task = Task.Run(() => App.CartDataManager.GetOrders());
            MyOrders = pedidos_task.Result;
            Orders_A_Decorrer_Get();
            Orders_Em_Analise_Get();
            Orders_Terminados_Get();
            BindingContext = this;
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

        private void Orders_A_Decorrer_Get()
        {
            List<MyOrder> pedidos = MyOrders.Where(x => x.pagamento_validado == true && x.terminado == false).ToList();
            if (pedidos.Count == 0)
            {
                grid_no_data_em_aberto.IsVisible = true;
                pedidos_em_aberto_grid.HeightRequest = 0;
                pedidos_em_aberto_grid.IsVisible = false;
            }
            else
            {
                pedidos_em_aberto_grid.ItemsSource = pedidos;
                pedidos_em_aberto_grid.HeightRequest = pedidos.Count * 40 + 35;
            }
        }

        private void Orders_Em_Analise_Get()
        {
            List<MyOrder> pedidos = MyOrders.Where(x => x.pagamento_validado == false && x.terminado == false).ToList();
            foreach (var p in pedidos)
            {
                if (string.IsNullOrEmpty(p.comprovativo_de_pagamento_entrada_url))
                {
                    p.estado = "A aguardar comprovativo de pagamento";
                }
                else
                {
                    p.estado = "Em análise";
                }
            }

            if (pedidos.Count == 0)
            {
                grid_no_data_em_analise.IsVisible = true;
                pedidos_em_analise_grid.HeightRequest = 0;
                pedidos_em_analise_grid.IsVisible = false;
            }
            else
            {
                pedidos_em_analise_grid.ItemsSource = pedidos;
                pedidos_em_analise_grid.HeightRequest = pedidos.Count * 40 + 35;
            }
        }

        private void Orders_Terminados_Get()
        {
            List<MyOrder> pedidos = MyOrders.Where(x => x.terminado == true).ToList();

            if (pedidos.Count == 0)
            {
                grid_no_data_terminados.IsVisible = true;
                pedidos_terminados_grid.HeightRequest = 0;
                pedidos_terminados_grid.IsVisible = false;
            }
            else
            {
                pedidos_terminados_grid.ItemsSource = pedidos;
                pedidos_terminados_grid.HeightRequest = pedidos.Count * 40 + 35;
            }
        }

        [Obsolete]
        protected void pedidos_OnItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        {
            App.MyOrder_Selected = e.SelectedItem as MyOrder;
            App.NavigateTo(false, typeof(PedidoPage));
        }

        //protected void pedidos_em_aberto_grid_OnItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        //{
        //    App.MyOrder_Selected = e.SelectedItem as MyOrder;
        //    App.NavigateTo(false, typeof(PedidoPage));
        //}

        //[Obsolete]
        //protected void pedidos_terminados_grid_OnItemSelectedAsync(object sender, SelectedItemChangedEventArgs e)
        //{
        //    App.MyOrder_Selected = e.SelectedItem as MyOrder;
        //    App.NavigateTo(false, typeof(PedidoPage));
        //}

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
                                stream = file.GetStream()
                            };
                            ImagensList.Add(_newImagem);
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
            App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
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
    }
}