using Apps;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        public Servico serv_1 { get; set; }
        public Servico serv_2 { get; set; }
        public Servico serv_3 { get; set; }
        public Servico serv_4 { get; set; }

        public bool grid_menu_was_calculated { get; set; }

        [Obsolete]
        public HomePage()
        {
            InitializeComponent();
            BindDataUserOnline();
            grid_menu_was_calculated = false;
            App.previousPage = this;
            ShowIndicator();
            BindingContext = this;

            NavigationPage.SetHasNavigationBar(this, false);

            //GOTO TO DEFINICOES
            var GoToDefinicoes = new TapGestureRecognizer();
            GoToDefinicoes.Tapped += (s, e) =>
            {
                App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
            };

            //Person_Img.GestureRecognizers.Add(GoToDefinicoes);
            //Iniciais_Frame.GestureRecognizers.Add(GoToDefinicoes);

            //GOTO TO CATEGORIAS
            var GoToCategorias_1 = new TapGestureRecognizer();
            var GoToCategorias_2 = new TapGestureRecognizer();
            var GoToCategorias_3 = new TapGestureRecognizer();
            GoToCategorias_1.Tapped += (s, e) =>
            {
                App.Categoria_Nivel_1_Selected = App.DataModel.Categorias.List.First(x => x.Nome == "Casa");
                App.MasterDetailPage.Detail = new NavigationPage(new CategoriasPage());
            };
            GoToCategorias_2.Tapped += (s, e) =>
            {
                App.Categoria_Nivel_1_Selected = App.DataModel.Categorias.List.First(x => x.Nome == "Auto");
                App.MasterDetailPage.Detail = new NavigationPage(new CategoriasPage());
            };
            GoToCategorias_3.Tapped += (s, e) =>
            {
                App.Categoria_Nivel_1_Selected = App.DataModel.Categorias.List.First(x => x.Nome == "Delivery");
                App.MasterDetailPage.Detail = new NavigationPage(new CategoriasPage());
            };
            Frame_Menu_1.GestureRecognizers.Add(GoToCategorias_1);
            Frame_Menu_2.GestureRecognizers.Add(GoToCategorias_2);
            Frame_Menu_3.GestureRecognizers.Add(GoToCategorias_3);

            //random servicos
            List<Servico> servicos_random = new List<Servico>();
            var cat1 = App.DataModel.Categorias.List.ElementAt(0);
            var cat3 = App.DataModel.Categorias.List.ElementAt(1);

            serv_1 = cat1.SubCategorias.List.Where(x => x.Id == 1142).First().Servicos.List.ElementAt(0);
            serv_2 = cat1.SubCategorias.List.Where(x => x.Id == 1143).First().Servicos.List.ElementAt(0);
            serv_3 = cat3.SubCategorias.List.Where(x => x.Id == 1163).First().Servicos.List.ElementAt(0);
            serv_4 = cat3.SubCategorias.List.Where(x => x.Id == 1171).First().Servicos.List.ElementAt(0);

            var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
            nfi.NumberGroupSeparator = " ";
            ImageSource img_source = ImageSource.FromFile("noimage.png");
            pop_img_1.Source = !string.IsNullOrEmpty(serv_1.ImagemUrl) ? ImageSource.FromUri(new Uri(serv_1.ImagemUrl)) : img_source;
            pop_preco_1.Text = Math.Round(serv_1.PrecoDbl, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            pop_titulo_1.Text = cat1.SubCategorias.List.Where(x => x.Id == 1142).First().Nome + "\n" + serv_1.Nome;

            pop_img_2.Source = !string.IsNullOrEmpty(serv_2.ImagemUrl) ? ImageSource.FromUri(new Uri(serv_2.ImagemUrl)) : img_source;
            pop_preco_2.Text = Math.Round(serv_2.PrecoDbl, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            pop_titulo_2.Text = cat1.SubCategorias.List.Where(x => x.Id == 1143).First().Nome + "\n" + serv_2.Nome;

            pop_img_3.Source = !string.IsNullOrEmpty(serv_3.ImagemUrl) ? ImageSource.FromUri(new Uri(serv_3.ImagemUrl)) : img_source;
            pop_preco_3.Text = Math.Round(serv_3.PrecoDbl, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            pop_titulo_3.Text = cat3.SubCategorias.List.Where(x => x.Id == 1163).First().Nome + "\n" + serv_3.Nome;

            pop_img_4.Source = !string.IsNullOrEmpty(serv_4.ImagemUrl) ? ImageSource.FromUri(new Uri(serv_4.ImagemUrl)) : img_source;
            pop_preco_4.Text = Math.Round(serv_4.PrecoDbl, 0).ToString("#,0.00", nfi).Replace(".00", "") + " KZs";
            pop_titulo_4.Text = cat3.SubCategorias.List.Where(x => x.Id == 1171).First().Nome + "\n" + serv_4.Nome;

            if (App.UserIsOnline)
            {
                var data_favoritos_task = Task.Run(() => App.CartDataManager.GetFavoritos());
                List<Servico> data_favoritos = data_favoritos_task.Result;

                if (data_favoritos.Where(x => x.Id == serv_1.Id).FirstOrDefault() != null)
                {
                    pop_remove_favorito_btn_1.IsVisible = true;
                    pop_add_favorito_btn_1.IsVisible = false;
                }
                else
                {
                    pop_remove_favorito_btn_1.IsVisible = false;
                    pop_add_favorito_btn_1.IsVisible = true;
                }

                if (data_favoritos.Where(x => x.Id == serv_2.Id).FirstOrDefault() != null)
                {
                    pop_remove_favorito_btn_2.IsVisible = true;
                    pop_add_favorito_btn_2.IsVisible = false;
                }
                else
                {
                    pop_remove_favorito_btn_2.IsVisible = false;
                    pop_add_favorito_btn_2.IsVisible = true;
                }

                if (data_favoritos.Where(x => x.Id == serv_3.Id).FirstOrDefault() != null)
                {
                    pop_remove_favorito_btn_3.IsVisible = true;
                    pop_add_favorito_btn_3.IsVisible = false;
                }
                else
                {
                    pop_remove_favorito_btn_3.IsVisible = false;
                    pop_add_favorito_btn_3.IsVisible = true;
                }

                if (data_favoritos.Where(x => x.Id == serv_4.Id).FirstOrDefault() != null)
                {
                    pop_remove_favorito_btn_4.IsVisible = true;
                    pop_add_favorito_btn_4.IsVisible = false;
                }
                else
                {
                    pop_remove_favorito_btn_4.IsVisible = false;
                    pop_add_favorito_btn_4.IsVisible = true;
                }
            }
        }

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            row1.Height = width / 3;
            if (!grid_menu_was_calculated)
            {
                grid_menu_was_calculated = true;
                grid_menu.HeightRequest = width / 3 - 30;

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
                Nome_Label.Text = "Seja bem-vindo";
                Itens_No_Carrinho_Label.Text = "0";
            }
            else
            {
                Nome_Label.Text = App.DataModel.Utilizador.Nome;
                TapGestureRecognizer GoToDefinicoes = new TapGestureRecognizer();
                GoToDefinicoes.Tapped += (s, e) =>
                {
                    App.previousPage = this;
                    App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
                };

                if (App.DataModel.Utilizador.FotoByteArray == null)
                {
                    Iniciais_Frame_LoggedIn.IsVisible = true;
                    if(App.DataModel.Utilizador.Nome.Contains(" "))
                    {
                        string[] nome = App.DataModel.Utilizador.Nome.Split(' ');
                        Iniciais_Label.Text = nome[0].ToCharArray()[0].ToString() + "" + nome[1].ToCharArray()[0].ToString();
                        Iniciais_Frame_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                    }
                    else
                    {
                        Iniciais_Label.Text = App.DataModel.Utilizador.Nome.ToCharArray()[0].ToString() + "" + App.DataModel.Utilizador.Nome.ToCharArray()[1].ToString();
                        Iniciais_Frame_LoggedIn.GestureRecognizers.Add(GoToDefinicoes);
                    }
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

            if (Device.Idiom != TargetIdiom.Phone)
            {
                cat_1_icon.FontSize = 30;
                cat_1_label.FontSize = 26;
                cat_1_grid.HeightRequest = 80;
                cat_1_grid_first_row.Height = 40;


                cat_2_icon.FontSize = 30;
                cat_2_label.FontSize = 26;
                cat_2_grid.HeightRequest = 80;
                cat_2_grid_first_row.Height = 40;

                cat_3_icon.FontSize = 30;
                cat_3_label.FontSize = 26;
                cat_3_grid.HeightRequest = 80;
                cat_3_grid_first_row.Height = 40;
            }
        }



        #region menu e loading
        public async void ShowIndicator()
        {
            Show();
            await Task.Delay(2000);
            Hide();
        }

        public void Show()
        {
            LoadingActivator.IsRunning = true;
            LoadingActivator.IsVisible = true;
        }

        public void Hide()
        {
            LoadingActivator.IsRunning = false;
            LoadingActivator.IsVisible = false;
        }

        [Obsolete]
        private void menu_definicoes_button_Clicked(object sender, EventArgs e)
        {
            App.previousPage = this;
            App.MasterDetailPage.Detail = new NavigationPage(new DefinicoesPage());
        }

        [Obsolete]
        private void ajuda_button_Clicked(object sender, EventArgs e)
        {
            App.previousPage = this;
            App.MasterDetailPage.Detail = new NavigationPage(new ContactarPage());
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

        #endregion
        [Obsolete]
        private void LogoImageButton_Clicked(object sender, EventArgs e)
        {
            App.NavigateTo(false, typeof(HomePage));
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
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço adicionado aos seus favoritos.";
                    await Task.Delay(3000);
                    SetPopup();
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
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço eliminado dos seus favoritos.";
                    await Task.Delay(3000);
                    SetPopup();
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
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
        private void pop_add_favorito_btn_1_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                AddToFavoritosRowTappedAsync(serv_1.Id);
                pop_remove_favorito_btn_1.IsVisible = true;
                pop_add_favorito_btn_1.IsVisible = false;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_remove_favorito_btn_1_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                DeleteFromFavoritosRowTappedAsync(serv_1.Id);
                pop_remove_favorito_btn_1.IsVisible = false;
                pop_add_favorito_btn_1.IsVisible = true;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private async void addTocartbtn1_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertIntoCart(
                                                                                new InsertIntoCartPost()
                                                                                {
                                                                                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                    servicoId = serv_1.Id,
                                                                                    preco = serv_1.PrecoDbl
                                                                                }));
                if (res.Result)
                {

                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                    foreach (var c in App.DataModel.Categorias.List)
                    {
                        foreach (var sc in c.SubCategorias.List)
                        {
                            var serv_t = sc.Servicos.List.Where(x => x.Id == serv_1.Id).FirstOrDefault();
                            if (serv_t != null)
                            {
                                serv_t.isInCart = true;
                                break;
                            }
                        }
                    }
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço adicionado ao carrinho de compras.";
                    await Task.Delay(3000);
                    SetPopup();
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private async void pop_add_to_cart_btn_2_Clicked(object sender, EventArgs e)
        {

            if (App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertIntoCart(
                                                                                    new InsertIntoCartPost()
                                                                                    {
                                                                                        memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                        servicoId = serv_2.Id,
                                                                                        preco = serv_2.PrecoDbl
                                                                                    }));
                if (res.Result)
                {
                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                    foreach (var c in App.DataModel.Categorias.List)
                    {
                        foreach (var sc in c.SubCategorias.List)
                        {
                            var serv_t = sc.Servicos.List.Where(x => x.Id == serv_2.Id).FirstOrDefault();
                            if (serv_t != null)
                            {
                                serv_t.isInCart = true;
                                break;
                            }
                        }
                    }
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço adicionado ao carrinho de compras.";
                    await Task.Delay(3000);
                    SetPopup();

                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private async void pop_add_to_cart_btn_3_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertIntoCart(
                                                                                new InsertIntoCartPost()
                                                                                {
                                                                                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                    servicoId = serv_3.Id,
                                                                                    preco = serv_3.PrecoDbl
                                                                                }));
                if (res.Result)
                {

                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                    foreach (var c in App.DataModel.Categorias.List)
                    {
                        foreach (var sc in c.SubCategorias.List)
                        {
                            var serv_t = sc.Servicos.List.Where(x => x.Id == serv_3.Id).FirstOrDefault();
                            if (serv_t != null)
                            {
                                serv_t.isInCart = true;
                                break;
                            }
                        }
                    }
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço adicionado ao carrinho de compras.";
                    await Task.Delay(3000);
                    SetPopup();
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private async void pop_add_to_cart_btn_4_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                Task<bool> res = Task.Run(() => App.CartDataManager.InsertIntoCart(
                                                                              new InsertIntoCartPost()
                                                                              {
                                                                                  memberId = App.DataModel.Utilizador.UmbracoMemberId,
                                                                                  servicoId = serv_4.Id,
                                                                                  preco = serv_4.PrecoDbl
                                                                              }));
                if (res.Result)
                {
                    var res_itens_in_cart = Task.Run(() => App.CartDataManager.NumberOfItensInCart());
                    Itens_No_Carrinho_Label.Text = res_itens_in_cart.Result.ToString();
                    foreach (var c in App.DataModel.Categorias.List)
                    {
                        foreach (var sc in c.SubCategorias.List)
                        {
                            var serv_t = sc.Servicos.List.Where(x => x.Id == serv_4.Id).FirstOrDefault();
                            if (serv_t != null)
                            {
                                serv_t.isInCart = true;
                                break;
                            }
                        }
                    }
                    SetPopup();
                    sucesso_msg_label.Text = "Serviço adicionado ao carrinho de compras.";
                    await Task.Delay(3000);
                    SetPopup();
                }
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
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

        [Obsolete]
        private void pop_add_favorito_btn_2_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                AddToFavoritosRowTappedAsync(serv_2.Id);
                pop_remove_favorito_btn_2.IsVisible = true;
                pop_add_favorito_btn_2.IsVisible = false;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_add_favorito_btn_3_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                AddToFavoritosRowTappedAsync(serv_3.Id);
                pop_remove_favorito_btn_3.IsVisible = true;
                pop_add_favorito_btn_3.IsVisible = false;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_add_favorito_btn_4_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                AddToFavoritosRowTappedAsync(serv_4.Id);
                pop_remove_favorito_btn_4.IsVisible = true;
                pop_add_favorito_btn_4.IsVisible = false;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_remove_favorito_btn_2_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                DeleteFromFavoritosRowTappedAsync(serv_2.Id);
                pop_remove_favorito_btn_2.IsVisible = false;
                pop_add_favorito_btn_2.IsVisible = true;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_remove_favorito_btn_3_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                DeleteFromFavoritosRowTappedAsync(serv_3.Id);
                pop_remove_favorito_btn_3.IsVisible = false;
                pop_add_favorito_btn_3.IsVisible = true;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
        }

        [Obsolete]
        private void pop_remove_favorito_btn_4_Clicked(object sender, EventArgs e)
        {
            if (App.UserIsOnline)
            {
                DeleteFromFavoritosRowTappedAsync(serv_4.Id);
                pop_remove_favorito_btn_4.IsVisible = false;
                pop_add_favorito_btn_4.IsVisible = true;
            }
            else
            {
                App.MasterDetailPage.Detail = new NavigationPage(new LoginPage());
            }
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
    }
}