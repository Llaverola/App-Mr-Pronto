using Apps.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Apps.SettingsData.Notificacoes
{
    public interface IMobileDataService
    {
        Task<DataModel> DataGetAsync();
        Task<bool> PedidoDeContactoPostAsync(PedidoDeContactoPostItem m);
        Task<bool> NotificacoesMarcarComoLidasPostAsync(int umbracoMemberId);
        Task<bool> AlterarNotificacoesAsync(string text);
        Task<bool> EliminarConta();
    }
}