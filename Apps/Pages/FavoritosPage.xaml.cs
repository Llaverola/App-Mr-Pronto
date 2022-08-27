using Apps;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavoritosPage : ContentPage
    {
        public ICommand ServicoRowTappedCommand { get; private set; }
        public ICommand DeleteFromFavoritosCommand { get; private set; }
        public ICommand AddToFavoritosCommand { get; private set; }
        [Obsolete]
        public FavoritosPage()
        {
            InitializeComponent();
            ServicoRowTappedCommand = new Command<ServicoTappedItem>(ServicoRowTappedAsync);
            DeleteFromFavoritosCommand = new Command<int>(DeleteFromFavoritosRowTappedAsync);

            BindDataUserOnline();
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
            DataGet();
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

        [Obsolete]
        private void DataGet()
        {
            var data_task = Task.Run(() => App.CartDataManager.GetFavoritos());
            List<Servico> data = data_task.Result;
            if (data.Count > 0)
            {
                int linhas = data.Count;

                for (int i = 0; i < linhas; i++)
                {
                    grid_servicos.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 50 } };
                }
                grid_servicos.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition(), new ColumnDefinition() { Width = 50 } };

                for (int i = 0; i < linhas; i++)
                {
                    Servico servico = data.ElementAt(i);

                    ImageButton icon = new ImageButton()
                    {
                        Source = ImageSource.FromFile("addtocart"),
                        WidthRequest = 20,
                        HeightRequest = 20,
                        BackgroundColor = Color.Transparent,
                        Padding = 0
                    };

                    Grid buttons_grid = new Grid() { HorizontalOptions = LayoutOptions.End, VerticalOptions = LayoutOptions.Center };
                    buttons_grid.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 20 } };
                    buttons_grid.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() { Width = 20 }, new ColumnDefinition() { Width = 20 } };

                    StackLayout stck_all = new StackLayout()
                    {
                        Orientation = StackOrientation.Vertical
                    };

                    Grid g = new Grid();
                    g.VerticalOptions = LayoutOptions.Start;
                    g.RowDefinitions = new RowDefinitionCollection() { new RowDefinition(), new RowDefinition(), new RowDefinition() };

                    Label lbl_titulo = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = servico.Nome, HorizontalOptions = LayoutOptions.Start };
                    Label lbl_descricao = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Medium", TextColor = Color.Black, TextTransform = TextTransform.None, Text = servico.Descricao, Margin = 0 };
                    Label lbl_preco = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = servico.Preco, Margin = 0, HorizontalTextAlignment = TextAlignment.Start };
                    //xEffects.ShadowEffect.SetColor(lbl_preco, Color.FromHex("#FAC321"));

                    g.Children.Add(lbl_titulo, 0, 0);
                    g.Children.Add(lbl_descricao, 0, 1);
                    g.Children.Add(lbl_preco, 0, 2);


                    icon.Clicked += (s, e) =>
                    {
                        ServicoRowTappedAsync(new ServicoTappedItem()
                        {
                            Servico = servico,
                            ImageButton = icon
                        });
                    };
                    grid_servicos.Children.Add(icon, 1, i);

                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();

                    foreach (var c in App.DataModel.Categorias.List)
                    {
                        foreach (var sc in c.SubCategorias.List)
                        {
                            var serv_t = sc.Servicos.List.Where(x => x.Id == servico.Id).FirstOrDefault();
                            if (serv_t != null)
                            {
                                serv_t.isInCart = true;
                                break;
                            }
                        }
                    }

                    buttons_grid.Children.Add(icon, 0, 0);


                    ImageButton icon_delete_from_whishlist = new ImageButton()
                    {
                        Source = ImageSource.FromFile("favorito"),
                        WidthRequest = 20,
                        HeightRequest = 20,
                        BackgroundColor = Color.Transparent,
                        Padding = 0
                    };
                    icon_delete_from_whishlist.Clicked += (s, e) =>
                    {
                        DeleteFromFavoritosRowTappedAsync(servico.Id);
                    };
                    buttons_grid.Children.Add(icon_delete_from_whishlist, 1, 0);


                    grid_servicos.Children.Add(buttons_grid, 1, i);
                    grid_servicos.Children.Add(g, 0, i);
                }
            }
            else
            {
                grid_servicos.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 40 } };
                grid_servicos.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                grid_servicos.ColumnDefinitions = new ColumnDefinitionCollection() { };

                Image img = new Image()
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Source = ImageSource.FromFile("sad"),
                    HeightRequest = 40,
                    WidthRequest = 40
                };
                Label lbl_no_data = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = "Ainda não tem serviços favoritos.", HorizontalOptions = LayoutOptions.Center };
                grid_servicos.Children.Add(img, 0, 0);
                grid_servicos.Children.Add(lbl_no_data, 0, 1);
            }
        }

        [Obsolete]
        public async void ServicoRowTappedAsync(ServicoTappedItem item)
        {
            if ( App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertIntoCart(
                                                                                new InsertIntoCartPost()
                                                                                {
                                                                                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                    servicoId = item.Servico.Id,
                                                                                    preco = item.Servico.PrecoDbl
                                                                                }));
                if (res.Result)
                {
                    SetPopup_Msg();
                    sucesso_msg_label.Text = "Serviço adicionado ao carrinho de compras.";
                    await Task.Delay(3000);
                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                    SetPopup_Msg();
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }



        [Obsolete]
        public async void DeleteFromFavoritosRowTappedAsync(int servicoId)
        {
            if ( App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.DeleteFavorito(
                                                                                new FavoritoPost()
                                                                                {
                                                                                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                    servicoId = servicoId
                                                                                }));
                if (res.Result)
                {
                    SetPopup_Msg();
                    sucesso_msg_label.Text = "Serviço eliminado dos seus favoritos.";
                    await Task.Delay(3000);
                    SetPopup_Msg();
                    App.NavigateTo(false, typeof(FavoritosPage));
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
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

    }
}