using Apps.Models;
using Apps.SettingsData.Notificacoes;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apps.Services.NotificacoesData
{
    public class MobileDataService : IMobileDataService
    {
        HttpClient Client { get; set; }
        public MobileDataService()
        {
            Client = new HttpClient();
        }

        public async Task<bool> EliminarConta()
        {
            bool d = true;
            try
            {
                Uri uri = new Uri("https://mrpronto.vertigma.com/umbraco/api/loginapi/EliminarConta");
                var p = new
                {
                    username = App.DataModel.Utilizador.Username
                };
                HttpResponseMessage response = await Client.PostAsync(uri, p.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    //do nothing
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<DataModel> DataGetAsync()
        {
            DataModel d = new DataModel();
            try
            {
                string url = "https://mrpronto.vertigma.com/umbraco/api/MobileApi/GetData?DispositivoId=" + App.DeviceIdentifier;
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<DataModel>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<bool> PedidoDeContactoPostAsync(PedidoDeContactoPostItem m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/MobileApi/{0}", "PedidoDeContacto"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<bool> NotificacoesMarcarComoLidasPostAsync(int memberId)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/MobileApi/{0}", "MarcarNotificacoesComoLidas"));
                var m = new
                {
                    umbracoMemberId = memberId
                };
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<bool> AlterarNotificacoesAsync(string text)
        {
            bool sucesso = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/MobileApi/{0}", "AlterarNotificacoes"));
                var m = new
                {
                    memberId = App.DataModel.Utilizador.UmbracoMemberId,
                    option = text
                };
                HttpResponseMessage response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                sucesso = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return sucesso;
        }
    }
}