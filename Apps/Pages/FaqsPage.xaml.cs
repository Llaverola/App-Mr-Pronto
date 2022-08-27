using Apps;
using Apps.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MasterDetailPageNavigation
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FaqsPage : ContentPage
    {
        private bool grid_menu_was_calculated { get; set; }
        [Obsolete]
        public FaqsPage()
        {
            InitializeComponent();
            grid_menu_was_calculated = false;
            BindDataUserOnline();
            ShowIndicator();
            NavigationPage.SetHasNavigationBar(this, false);
            DataGet();
            BindingContext = this;
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    base.OnSizeAllocated(width, height);
        //    if (!grid_menu_was_calculated)
        //    {
        //        grid_menu_was_calculated = true;
        //        grid_data.HeightRequest = width / 3;
        //    }
        //}

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
            var data = App.DataModel.Faqs.list;
            int linhas = data.Count;
            for (int i = 0; i < linhas; i++)
            {
                grid_data.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() };
            }
            grid_data.ColumnDefinitions = new ColumnDefinitionCollection() { new ColumnDefinition() };

            for (int i = 0; i < linhas; i++)
            {
                Grid sub_grid = new Grid() { ColumnSpacing = 20, RowSpacing = 15 };
                sub_grid.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = 35 } };
                sub_grid.RowDefinitions = new RowDefinitionCollection() { new RowDefinition() { Height = GridLength.Auto } };

                Faq f = data.ElementAt(i);
                Label lbl_pergunta = new Label() { FontSize = 16, LineHeight = 1.2, FontFamily = "MyCustomFont_Bold", TextColor = Color.Black, TextTransform = TextTransform.None, Text = f.pergunta };
                Label lbl_resposta = new Label() { FontSize = 16, LineHeight = 1.2, FontFamily = "MyCustomFont_Regular", TextColor = Color.Black, TextTransform = TextTransform.None, Text = f.resposta };
                sub_grid.Children.Add(lbl_pergunta, 0, 0);
                sub_grid.Children.Add(lbl_resposta, 0, 1);
                grid_data.Children.Add(sub_grid, 0, i);
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
            App.MasterDetailPage.Detail = new NavigationPage(new HomePage());
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
        private void yellowBoxView_Clicked(object sender, EventArgs e)
        {
            if (!App.UserIsOnline)
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