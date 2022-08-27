using MasterDetailPageNavigation;
using System.Collections.Generic;

namespace Apps.Models
{
    public class MenuModel
    {
        public List<MasterPageItem> Items { get; set; }
        public MenuModel()
        {
            Items = new List<MasterPageItem>();
            Items.Add(new MasterPageItem() { Index = 0, Title = "Home", RowHeight = 1 });
            Items.Add(new MasterPageItem() { Index = 8, Title = "Como funciona", RowHeight = 1 });
            Items.Add(new MasterPageItem() { Index = 6, Title = "Contactos", RowHeight = 1 });
            Items.Add(new MasterPageItem() { Index = 7, Title = "Envie-nos uma mensagem", RowHeight = 1 });
            if ( App.UserIsOnline)
            {
                Items.Add(new MasterPageItem() { Index = 1, Title = "A minha conta", RowHeight = 1 });
                Items.Add(new MasterPageItem() { Index = 2, Title = "O meu carrinho de compras", RowHeight = 1 });
                Items.Add(new MasterPageItem() { Index = 9, Title = "Eliminar Conta", RowHeight = 1 });
                Items.Add(new MasterPageItem() { Index = 3, Title = "Logout", RowHeight = 1 });
            }
            else
            {
                Items.Add(new MasterPageItem() { Index = 4, Title = "Registo", RowHeight = 1 });
                Items.Add(new MasterPageItem() { Index = 5, Title = "Login", RowHeight = 1 });
            }
        }
    }
}
