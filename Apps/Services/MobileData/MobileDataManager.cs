using Apps.Models;
using Apps.SettingsData.Notificacoes;
using System.Threading.Tasks;

namespace Apps.Services.NotificacoesData
{
    public class MobileDataManager
    {
        readonly IMobileDataService restService;

        public MobileDataManager(IMobileDataService service)
        {
            restService = service;
        }
        public Task<DataModel> DataGetAsync()
        {
            return Task.Run(() => restService.DataGetAsync());
        }
        public Task<bool> PedidoDeContactoPostAsync(PedidoDeContactoPostItem m)
        {
            return Task.Run(() => restService.PedidoDeContactoPostAsync(m));
        }

        public Task<bool> NotificacoesMarcarComoLidasPostAsync(int umbracoMemberId)
        {
            return Task.Run(() => restService.NotificacoesMarcarComoLidasPostAsync(umbracoMemberId));
        }

        public Task<bool> AlterarNotificacoesAsync(string text)
        {
            return Task.Run(() => restService.AlterarNotificacoesAsync(text));
        }

        public Task<bool> EliminarConta()
        {
            return Task.Run(() => restService.EliminarConta());
        }
    }
}