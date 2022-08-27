using Apps.Models;
using Apps.SettingsData.CartData;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.Services.CartData
{
    public class CartDataManager
    {
        readonly ICartDataService restService;

        public CartDataManager(ICartDataService service)
        {
            restService = service;
        }
        public Task<MyCart> GetCart()
        {
            return Task.Run(() => restService.GetCart(App.DataModel.Utilizador.UmbracoMemberId));
        }
        public Task<List<MyOrder>> GetOrders()
        {
            return Task.Run(() => restService.GetOrders(App.DataModel.Utilizador.UmbracoMemberId));
        }

        public Task<bool> InsertIntoCart(InsertIntoCartPost m)
        {
            return Task.Run(() => restService.InsertIntoCart(m));
        }

        public Task<bool> RemoveFromCart(DeleteFromCartPost m)
        {
            return Task.Run(() => restService.DeleteFromCart(m));
        }

        public Task<string> FinishOrder(MoradaPostServico m)
        {
            return Task.Run(() => restService.FinishOrder(m));
        }

        public Task<bool> ComprovativoPost(ComprovativoPostModel m)
        {
            return Task.Run(() => restService.ComprovativoPost(m));
        }

        public Task<int> NumberOfItensInCart()
        {
            return Task.Run(() => restService.NumberOfItemsInCart(App.DataModel.Utilizador.UmbracoMemberId));
        }

        public Task<List<Servico>> GetFavoritos()
        {
            return Task.Run(() => restService.GetFavoritos());
        }

        public Task<bool> DeleteFavorito(FavoritoPost m)
        {
            return Task.Run(() => restService.DeleteFavorito(m));
        }

        public Task<bool> InsertFavorito(FavoritoPost m)
        {
            return Task.Run(() => restService.InsertFavorito(m));
        }
    }
}