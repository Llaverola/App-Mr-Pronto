using Apps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.SettingsData.CartData
{
    public interface ICartDataService
    {
        Task<List<Servico>> GetFavoritos();
        Task<MyCart> GetCart(int memberId);
        Task<List<MyOrder>> GetOrders(int memberId);
        Task<bool> InsertIntoCart(InsertIntoCartPost m);
        Task<string> FinishOrder(MoradaPostServico m);
        Task<bool> ComprovativoPost(ComprovativoPostModel m);
        Task<int> NumberOfItemsInCart(int memberId);
        Task<bool> DeleteFromCart(DeleteFromCartPost m);
        Task<bool> DeleteFavorito(FavoritoPost m);
        Task<bool> InsertFavorito(FavoritoPost m);
    }
}