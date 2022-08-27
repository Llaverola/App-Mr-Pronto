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
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xEffects = Xamarin.CommunityToolkit.Effects;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CartPage : ContentPage
    {
        public List<byte[]> Fotos { get; set; }
        public ICommand RowTappedCommand { get; private set; }

        [Obsolete]

        public CartPage()
        {
            InitializeComponent();
            if (!App.UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }

            BindDataUserOnline();
            Fotos = new List<byte[]>();
            PageTitle_Label.Text = "Carrinho de compras";
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        [Obsolete]
        private void BindDataUserOnline()
        {
            Person_Img_LoggedIn.IsVisible = false;
            Iniciais_Frame_LoggedIn.IsVisible = false;
            User_Label_Not_LoggedIn.IsVisible = false;
            if (!App.UserIsOnline)
            {
                User_Label_Not_LoggedIn.IsVisible = true;
                Itens_No_Carrinho_Label.Text = "0";
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
                if (res_itens_in_cart.Result == 0)
                {
                    All_Data.IsVisible = false;
                    btnFinalizar.IsVisible = false;
                    div_morada.IsVisible = false;
                    div_imagens.IsVisible = false;
                    div_informacoes.IsVisible = false;
                }
                GridSetData();
                Rua_Textbox.Text = App.DataModel.Utilizador.Morada;
                Andar_Textbox.Text = App.DataModel.Utilizador.Andar;
                Numero_da_porta_Textbox.Text = App.DataModel.Utilizador.NumeroDaPorta;
                Localidade_Textbox.Text = App.DataModel.Utilizador.Cidade;
                Area_Textbox.Text = App.DataModel.Utilizador.Area;
            }
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

        #region categorias

        [Obsolete]
        public void GridSetData()
        {
            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";

            MyCart data = Task.Run(() => App.CartDataManager.GetCart()).Result;

            if (data.servicos != null)
            {
                if (data.servicos.Count > 0)
                {
                    App.FinishOrder_Item = data;
                    total_agora_label.Text = Math.Round(data.perc_a_pagar, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
                    total_final_label.Text = Math.Round(data.total - data.perc_a_pagar, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
                    total_label.Text = data.total.ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
                    int linhas = data.servicos.Count;

                    var row_collection = new RowDefinitionCollection();
                    grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() { Width = 15 }, new ColumnDefinition() { Width = 150 }, new ColumnDefinition(), new ColumnDefinition(), new ColumnDefinition() { Width = 15 } };
                    row_collection.Add(new RowDefinition() { Height = 15 });
                    row_collection.Add(new RowDefinition() { Height = 2 });
                    for (int i = 0; i < linhas; i++)
                    {
                        row_collection.Add(new RowDefinition() { Height = 30 });
                    }

                    grid_data.RowDefinitions = row_collection;
                    Label lbl_header_1 = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = "#", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    Label lbl_header_2 = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = "Descrição", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    Label lbl_header_3 = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = "Qtd.", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    Label lbl_header_4 = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = "Preço", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                    BoxView lbl_box = new BoxView() { BackgroundColor = Color.FromHex("#1A1A1A"), HeightRequest = 1 };
                    grid_data.Children.Add(lbl_header_1, 0, 0);
                    grid_data.Children.Add(lbl_header_2, 1, 0);
                    grid_data.Children.Add(lbl_header_3, 2, 0);
                    grid_data.Children.Add(lbl_header_4, 3, 0);

                    grid_data.Children.Add(lbl_box, 0, 1);
                    Grid.SetColumnSpan(lbl_box, 5);


                    for (int i = 0; i < linhas; i++)
                    {
                        MyCart_Itens servico = data.servicos.ElementAt(i);

                        ImageButton icon = new ImageButton()
                        {
                            Source = ImageSource.FromFile("deleteFromCart"),
                            WidthRequest = 12,
                            HeightRequest = 12,
                            BackgroundColor = Color.Transparent,
                            Padding = 0,
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center
                        };

                        StackLayout stc_titulo_descricao = new StackLayout() { Orientation = StackOrientation.Vertical, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        Label lbl_counter = new Label() { FontSize = 10, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = (i + 1).ToString() + ".", HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        Label lbl_titulo = new Label() { LineHeight = 1.2, Padding = 0, FontSize = 10, FontFamily = "MyCustomFont_Medium", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = servico.nome, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        Label lbl_qtd = new Label() { FontSize = 10, HorizontalTextAlignment = TextAlignment.Center, FontFamily = "MyCustomFont_Medium", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = string.Format("{0} x\n{1} KZs", servico.qty, Math.Round(servico.preco / servico.qty, 0).ToString("#,0.00", nfi).Replace(".00", "")), HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        Label lbl_descricao = new Label() { LineHeight = 1.2, Padding = 0, FontSize = 10, FontFamily = "MyCustomFont_Regular", TextColor = Color.FromHex("#1A1A1A"), Margin = new Thickness(0, 0, 0, 0), TextTransform = TextTransform.None, Text = servico.descricao, HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center };
                        Label lbl_preco = new Label() { FontSize = 10, FontFamily = "MyCustomFont_Medium", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = Math.Round(servico.preco, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs", Margin = 0, HorizontalTextAlignment = TextAlignment.Start, VerticalOptions = LayoutOptions.Center };

                        stc_titulo_descricao.Children.Add(lbl_titulo);
                        stc_titulo_descricao.Children.Add(lbl_descricao);

                        grid_data.Children.Add(lbl_counter, 0, i + 2);

                        grid_data.Children.Add(stc_titulo_descricao, 1, i + 2);

                        grid_data.Children.Add(lbl_qtd, 2, i + 2);

                        grid_data.Children.Add(lbl_preco, 3, i + 2);

                        grid_data.Children.Add(icon, 4, i + 2);

                        var tapGestureRecognizer = new TapGestureRecognizer();
                        icon.Clicked += async (s, e) =>
                        {
                            Task<bool> remove_task = Task.Run(() => App.CartDataManager.RemoveFromCart(new DeleteFromCartPost() { servicoId = servico.id, memberId = App.DataModel.Utilizador.UmbracoMemberId }));
                            if (remove_task.Result)
                            {
                                var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                                Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                                SetPopup_Msg();
                                await Task.Delay(3000);
                                SetPopup_Msg();
                            }
                        };
                    }
                }
                else
                {
                    grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 40 } };
                    grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                    grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { };

                    Image img = new Image()
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Source = ImageSource.FromFile("sad"),
                        HeightRequest = 40,
                        WidthRequest = 40
                    };
                    Label lbl_no_data = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = "Carrinho de compras vazio.", HorizontalOptions = LayoutOptions.Center };
                    grid_data.Children.Add(img, 0, 0);
                    grid_data.Children.Add(lbl_no_data, 0, 1);
                    total_agora_label.IsVisible = false;
                    total_final_label.IsVisible = false;
                    total_agora_label.IsVisible = false;
                }
            }
            else
            {
                grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 40 } };
                grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { };

                Image img = new Image()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Source = ImageSource.FromFile("sad"),
                    HeightRequest = 40,
                    WidthRequest = 40
                };
                Label lbl_no_data = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = "Carrinho de compras vazio.", HorizontalOptions = LayoutOptions.Center };
                grid_data.Children.Add(img, 0, 0);
                grid_data.Children.Add(lbl_no_data, 0, 1);
                total_agora_label.IsVisible = false;
                total_final_label.IsVisible = false;
                total_agora_label.IsVisible = false;
            }
        }


        #endregion

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

        [Obsolete]
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
        }

        [Obsolete]
        private void tapcart_Tapped(object sender, EventArgs e)
        {
            if (!App.UserIsOnline)
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
            if (!App.UserIsOnline)
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
            if (!App.UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new FavoritosPage());
            }
        }

        [Obsolete]
        private async void btnFinalizar_Clicked(object sender, EventArgs e)
        {
            LoadingPopupPage loadingpage = new LoadingPopupPage();
            await PopupNavigation.PushAsync(loadingpage);
            await Task.Delay(2000);

            MoradaPostServico morada_servico = new MoradaPostServico()
            {
                rua = Rua_Textbox.Text,
                localidade = Localidade_Textbox.Text,
                porta = Numero_da_porta_Textbox.Text,
                andar = Andar_Textbox.Text,
                area = App.DataModel.Utilizador.Area,
                notas = Info_Textbox.Text,
                foto1 = null,
                foto2 = null,
                memberId = App.DataModel.Utilizador.UmbracoMemberId
            };

            if (Fotos.Count == 1)
            {
                morada_servico.foto1 = Fotos.First();
            }
            else if (Fotos.Count == 2)
            {
                morada_servico.foto1 = Fotos.First();
                morada_servico.foto2 = Fotos.ElementAt(1);
            }


            if (string.IsNullOrEmpty(morada_servico.rua) && string.IsNullOrEmpty(morada_servico.localidade))
            {
                await PopupNavigation.RemovePageAsync(loadingpage);
                await DisplayAlert("Alerta!", "Insira uma morada onde se realizará o serviço.", "OK");
            }
            else
            {
                Task<string> finalizar_task = Task.Run(() => App.CartDataManager.FinishOrder(morada_servico));
                if (!string.IsNullOrEmpty(finalizar_task.Result))
                {
                    App.Order_Number = finalizar_task.Result;
                    await PopupNavigation.RemovePageAsync(loadingpage);
                    App.MasterDetailPage.Detail = new NavigationPage(new FinishOrderPage());
                }
                else
                {
                    await PopupNavigation.RemovePageAsync(loadingpage);
                    await DisplayAlert("Alerta!", "Ocorreu um erro. Por favor, tente novamente.", "OK");
                }
            }
        }

        [Obsolete]
        private void voltar_button_Clicked(object sender, EventArgs e)
        {
            App.MasterDetailPage.Detail = new NavigationPage(App.previousPage);
        }

        [Obsolete]
        private void yellowBoxView_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                App.MasterDetailPage.Detail = new NavigationPage(new CartPage());
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private async void SetPopup_Msg()
        {
            if (!popuplayout_remover.IsVisible)
            {
                popuplayout_remover.IsVisible = !popuplayout_remover.IsVisible;
                popuplayout_remover.AnchorX = 1;
                popuplayout_remover.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => popuplayout_remover.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => popuplayout_remover.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(popuplayout_remover, "popupScaleAnimation", 250);
                fadeAnimation.Commit(popuplayout_remover, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    popuplayout_remover.FadeTo(0, 200, Easing.SinInOut)
                  );
                popuplayout_remover.IsVisible = !popuplayout_remover.IsVisible;
                App.NavigateTo(false, typeof(CartPage));
            }
        }


        #region Tirar fotos, escolher foto da galeria, gravar video e escolher video

        [Obsolete]
        private async void Button_Upload_Fotos_Clicked(object sender, EventArgs e)
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
                        if (Fotos.Count < 2)
                        {
                            Stream file_stream = file.GetStream();
                            byte[] file_bytes_array = General.ReadFully(file_stream);
                            Fotos.Add(file_bytes_array);
                            file.Dispose();
                        }
                        else
                        {
                            await DisplayAlert("Alerta!", "Só é permitido o envio de 2 fotos.", "OK");
                        }

                        if (Fotos.Count == 1)
                        {
                            foto_1.Source = ImageSource.FromStream(() => General.ByteArrayToStream(Fotos.First()));
                            foto_2.Source = ImageSource.FromFile("noimage");

                            foto_1_eliminar.IsVisible = true;
                            foto_2_eliminar.IsVisible = false;
                        }
                        else if (Fotos.Count == 2)
                        {
                            foto_1.Source = ImageSource.FromStream(() => General.ByteArrayToStream(Fotos.First()));
                            foto_2.Source = ImageSource.FromStream(() => General.ByteArrayToStream(Fotos.ElementAt(1)));
                            foto_1_eliminar.IsVisible = true;
                            foto_2_eliminar.IsVisible = true;
                        }
                        else if (Fotos.Count == 0)
                        {
                            foto_1.Source = ImageSource.FromFile("noimage");
                            foto_2.Source = ImageSource.FromFile("noimage");
                            foto_1_eliminar.IsVisible = false;
                            foto_2_eliminar.IsVisible = false;
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

        private void foto_1_eliminar_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                Fotos.Remove(Fotos.First());
                foto_1_eliminar.IsVisible = false;
                foto_1.Source = ImageSource.FromFile("noimage");
            }
        }

        private void foto_2_eliminar_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                if (Fotos.Count == 1)
                {
                    Fotos.Remove(Fotos.First());
                    foto_1.Source = ImageSource.FromFile("noimage");
                    foto_2.Source = ImageSource.FromFile("noimage");
                }
                else if (Fotos.Count == 2)
                {
                    Fotos.Remove(Fotos.ElementAt(1));
                    foto_2.Source = ImageSource.FromFile("noimage");
                }
                foto_2_eliminar.IsVisible = false;
            }
        }
    }
}