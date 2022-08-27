using Apps;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using xEffects = Xamarin.CommunityToolkit.Effects;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoriasPage : ContentPage
    {
        public ICommand RowTappedCommand { get; private set; }
        public ICommand ServicoRowTappedCommand { get; private set; }
        public ICommand DeleteFromFavoritosCommand { get; private set; }
        public ICommand AddToFavoritosCommand { get; private set; }
        private SubCategoria sub_categoria_global { get; set; }
        [Obsolete]

        public CategoriasPage()
        {
            InitializeComponent();
            sub_categoria_global = new SubCategoria();
            BindDataUserOnline();
            RowTappedCommand = new Command<SubCategoria>(RowTapped);
            AddToFavoritosCommand = new Command<int>(AddToFavoritosRowTappedAsync);
            DeleteFromFavoritosCommand = new Command<int>(DeleteFromFavoritosRowTappedAsync);
            ServicoRowTappedCommand = new Command<ServicoTappedItem>(ServicoRowTappedAsync);
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);

            bool UserIsOnline =  App.UserIsOnline;

            Cat_Picker.ItemsSource = new List<string>() { "Casa", "Auto", "Delivery" };
            if (App.Categoria_Nivel_1_Selected.Nome == "Casa")
            {
                Cat_Picker.SelectedItem = Cat_Picker.Items[0];
            }
            else if (App.Categoria_Nivel_1_Selected.Nome == "Auto")
            {
                Cat_Picker.SelectedItem = Cat_Picker.Items[1];
            }
            else if (App.Categoria_Nivel_1_Selected.Nome == "Delivery")
            {
                Cat_Picker.SelectedItem = Cat_Picker.Items[2];
            }
            SubCategoriasSet(App.Categoria_Nivel_1_Selected.SubCategorias);

            //GOTO TO DEFINICOES
            TapGestureRecognizer GoToDefinicoes = new TapGestureRecognizer();
            GoToDefinicoes.Tapped += (s, e) =>
            {
                App.previousPage = this;
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
            };
        }

        [Obsolete]
        public async void AddToFavoritosRowTappedAsync(int servicoId)
        {
            if (App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertFavorito(
                                                                                new FavoritoPost()
                                                                                {
                                                                                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                    servicoId = servicoId
                                                                                }));
                if (res.Result)
                {
                    SetPopup_Msg();
                    sucesso_msg_label.Text = "Serviço adicionado aos seus favoritos.";
                    await Task.Delay(3000);
                    SetPopup_Msg();
                    RowTapped(sub_categoria_global);
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
            if (App.UserIsOnline)
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
                    RowTapped(sub_categoria_global);
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
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

        [Obsolete]
        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            App.MasterDetailPage.Detail = new NavigationPage(new HomePage());
        }

        #region categorias

        [Obsolete]
        public async void ServicoRowTappedAsync(ServicoTappedItem item)
        {
            if (App.UserIsOnline)
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
        public void RowTapped(SubCategoria sub_categoria)
        {
            App.previousPage = this;
            List<Servico> data_favoritos = new List<Servico>();
            sub_categoria_global = sub_categoria;
            Servico_Label.Text = sub_categoria.Nome;
            var data = sub_categoria.Servicos.List.OrderBy(x => x.Nome).ToList();
            if (data.Count > 0)
            {
                int linhas = data.Count;
                if (App.UserIsOnline)
                {
                    var data_favoritos_task = Task.Run(() => App.CartDataManager.GetFavoritos());
                    data_favoritos = data_favoritos_task.Result;
                }

                for (int i = 0; i < linhas; i++)
                {
                    grid_servicos.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 70 } };
                }
                grid_servicos.ColumnDefinitions = new ColumnDefinitionCollection() { /*new ColumnDefinition() { Width = 60 }, */new ColumnDefinition(), new ColumnDefinition() { Width = 50 } };

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
                    g.VerticalOptions = LayoutOptions.FillAndExpand;
                    g.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 13 }, new RowDefinition() };


                    Label lbl_titulo = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = servico.Nome, HorizontalOptions = LayoutOptions.Start };
                    Label lbl_descricao = new Label() { FontSize = 9, FontFamily = "MyCustomFont_Regular", TextColor = Color.Black, TextTransform = TextTransform.None, Text = servico.Descricao, Margin = 0 };
                    Label lbl_preco = new Label() { FontSize = 11, FontFamily = "MyCustomFont_Bold", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = servico.Preco, Margin = 0, HorizontalTextAlignment = TextAlignment.Start };
                    xEffects.ShadowEffect.SetColor(lbl_preco, Color.FromHex("#FAC321"));

                    StackLayout div = new StackLayout() { Orientation = StackOrientation.Vertical };
                    div.Children.Add(lbl_descricao);
                    div.Children.Add(lbl_preco);

                    g.Children.Add(lbl_titulo, 0, 0);
                    g.Children.Add(div, 0, 1);
                    icon.IsVisible = true;

     
                    if (App.UserIsOnline)
                    {
                        icon.Clicked += (s, e) =>
                        {
                            ServicoRowTappedAsync(new ServicoTappedItem()
                            {
                                Servico = servico,
                                ImageButton = icon
                            });
                        };
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

                        if (data_favoritos.Where(x => x.Id == servico.Id).FirstOrDefault() != null)
                        {
                            ImageButton icon_delete_from_whishlist = new ImageButton()
                            {
                                Source = ImageSource.FromFile("favorito"),
                                WidthRequest = 30,
                                HeightRequest = 30,
                                BackgroundColor = Color.Transparent,
                                Padding = 0
                            };
                            icon_delete_from_whishlist.Clicked += (s, e) =>
                            {
                                DeleteFromFavoritosRowTappedAsync(servico.Id);
                            };
                            buttons_grid.Children.Add(icon_delete_from_whishlist, 1, 0);
                        }
                        else
                        {
                            ImageButton icon_add_to_whishlist = new ImageButton()
                            {
                                Source = ImageSource.FromFile("nao_favorito"),
                                WidthRequest = 20,
                                HeightRequest = 20,
                                BackgroundColor = Color.Transparent,
                                Padding = 0
                            };
                            icon_add_to_whishlist.Clicked += (s, e) =>
                            {
                                AddToFavoritosRowTappedAsync(servico.Id);
                            };
                            buttons_grid.Children.Add(icon_add_to_whishlist, 1, 0);
                        }
                    }
                    else
                    {
                        icon.Clicked += (s, e) =>
                        {
                            App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
                        };
                        ImageButton icon_add_to_whishlist = new ImageButton()
                        {
                            Source = ImageSource.FromFile("nao_favorito"),
                            WidthRequest = 20,
                            HeightRequest = 20,
                            BackgroundColor = Color.Transparent,
                            Padding = 0
                        };
                        icon_add_to_whishlist.Clicked += (s, e) =>
                        {
                            App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
                        };
                        buttons_grid.Children.Add(icon_add_to_whishlist, 1, 0);
                    }

                    grid_servicos.Children.Add(icon, 1, i);
                    buttons_grid.Children.Add(icon, 0, 0);
                    grid_servicos.Children.Add(buttons_grid, 1, i);
                    grid_servicos.Children.Add(g, 0, i);
                }
            }
            else
            {
                int linhas = 1;
                for (int i = 0; i < linhas; i++)
                {
                    grid_servicos.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                }
                grid_servicos.ColumnDefinitions = new ColumnDefinitionCollection() { };
                Label lbl_no_data = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Medium", TextColor = Color.Black, TextTransform = TextTransform.None, Text = "Sem serviços para apresentar." };
                grid_servicos.Children.Add(lbl_no_data, 0, 0);
            }

            SetPopup();
        }

        private async void SetPopup()
        {
            if (!this.popuplayout.IsVisible)
            {
                this.popuplayout.IsVisible = !this.popuplayout.IsVisible;
                this.popuplayout.AnchorX = 1;
                this.popuplayout.AnchorY = 1;

                Animation scaleAnimation = new Animation(
                    f => this.popuplayout.Scale = f,
                    0.5,
                    1,
                    Easing.SinInOut);

                Animation fadeAnimation = new Animation(
                    f => this.popuplayout.Opacity = f,
                    0.2,
                    1,
                    Easing.SinInOut);

                scaleAnimation.Commit(this.popuplayout, "popupScaleAnimation", 250);
                fadeAnimation.Commit(this.popuplayout, "popupFadeAnimation", 250);
            }
            else
            {
                await Task.WhenAny<bool>
                  (
                    this.popuplayout.FadeTo(0, 200, Easing.SinInOut)
                  );

                this.popuplayout.IsVisible = !this.popuplayout.IsVisible;
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

        [Obsolete]
        private void SubCategoriasSet(SubCategorias sub_categorias)
        {
            grid_data.Children.Clear();
            var data = sub_categorias.List.OrderBy(x => x.Nome).ToList();
            if (data.Count > 0)
            {
                int linhas = data.Count;
                for (int i = 0; i < linhas; i++)
                {
                    grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 80 } };
                }
                grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() };

                for (int i = 0; i < linhas; i++)
                {
                    Grid sub_grid = new Grid() { ColumnSpacing = 20, RowSpacing = 15 };
                    sub_grid.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 80 } };
                    sub_grid.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() { Width = 100 }, new ColumnDefinition(), new ColumnDefinition() { Width = 35 } };

                    StackLayout clickableRow = new StackLayout
                    {
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand
                    };

                    SubCategoria sub_categoria = data.ElementAt(i);

                    Image icon = new Image()
                    {
                        Source = ImageSource.FromFile("arrow_right"),
                        WidthRequest = 35,
                        HeightRequest = 35
                    };

                    StackLayout stck_titulo = new StackLayout() { VerticalOptions = LayoutOptions.Center, HeightRequest = 25 };
                    Label lbl_titulo = new Label() { FontSize = 18, FontFamily = "MyCustomFont_Medium", TextColor = Color.Black, TextTransform = TextTransform.None, Text = sub_categoria.Nome };
                    stck_titulo.Children.Add(lbl_titulo);

                    if (!string.IsNullOrEmpty(sub_categoria.ImagemUrl))
                    {
                        Image img_sub_categoria = new Image()
                        {
                            Source = ImageSource.FromUri(new Uri(sub_categoria.ImagemUrl)),
                            Aspect = Aspect.AspectFill
                        };
                        xEffects.ShadowEffect.SetColor(img_sub_categoria, Color.FromHex("#666666"));
                        sub_grid.Children.Add(img_sub_categoria, 0, 0);
                    }
                    else
                    {
                        Image img_empty = new Image()
                        {
                            Source = ImageSource.FromFile("noimage.png"),
                            Aspect = Aspect.AspectFit
                        };
                        sub_grid.Children.Add(img_empty, 0, 0);
                    }


                    sub_grid.Children.Add(stck_titulo, 1, 0);
                    sub_grid.Children.Add(icon, 2, 0);

                    clickableRow.Children.Add(sub_grid);

                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.NumberOfTapsRequired = 1;
                    tapGestureRecognizer.Tapped += (s, e) =>
                    {
                        if (grid_servicos.Children.Count() > 0)
                        {
                            grid_servicos.Children.Clear();
                        }
                        RowTapped(sub_categoria);
                    };
                    clickableRow.GestureRecognizers.Add(tapGestureRecognizer);
                    grid_data.Children.Add(clickableRow, 0, i);

                }
            }
            else
            {
                int linhas = 1;
                for (int i = 0; i < linhas; i++)
                {
                    grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                }
                grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { };
                Label lbl_no_data = new Label() { FontSize = 13, FontFamily = "MyCustomFont_Medium", TextColor = Color.FromHex("#1A1A1A"), TextTransform = TextTransform.None, Text = "Sem categorias para apresentar." };
                grid_data.Children.Add(lbl_no_data, 0, 0);
            }
        }

        #endregion

        [Obsolete]
        private void Cat_Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            string cat_str = Cat_Picker.Items[Cat_Picker.SelectedIndex].ToString();
            SubCategorias cat_model = App.DataModel.Categorias.List.First(x => x.Nome == cat_str).SubCategorias;
            SubCategoriasSet(cat_model);
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

        [Obsolete]
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            SetPopup();
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

        private void cat_picker_arrow_Clicked(object sender, EventArgs e)
        {
            Cat_Picker.Focus();
        }
    }

    public class ServicoTappedItem
    {
        public Servico Servico { get; set; }
        public ImageButton ImageButton { get; set; }
    }
}