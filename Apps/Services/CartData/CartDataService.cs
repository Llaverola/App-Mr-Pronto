using Apps.Models;
using Apps.SettingsData.CartData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apps.Services.CartData
{
    public class CartDataService : ICartDataService
    {
        HttpClient Client { get; set; }
        public CartDataService()
        {
            Client = new HttpClient();
        }
        public async Task<MyCart> GetCart(int memberId)
        {
            MyCart d = new MyCart();
            try
            {
                string url = "https://mrpronto.vertigma.com/umbraco/api/CartApi/GetCart?memberId=" + memberId;
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<MyCart>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<List<MyOrder>> GetOrders(int memberId)
        {
            List<MyOrder> d = new List<MyOrder>();
            try
            {
                string url = "https://mrpronto.vertigma.com/umbraco/api/CartApi/GetOrders?memberId=" + memberId;
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<List<MyOrder>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<List<Servico>> GetFavoritos()
        {
            List<Servico> d = new List<Servico>();
            try
            {
                string url = "https://mrpronto.vertigma.com/umbraco/api/CartApi/GetFavoritos?memberId=" + App.DataModel.Utilizador.UmbracoMemberId.ToString();
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<List<Servico>>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<bool> DeleteFavorito(FavoritoPost m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "FavoritoDeletePost"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<bool> InsertFavorito(FavoritoPost m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "FavoritoInsertPost"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<int> NumberOfItemsInCart(int memberId)
        {
            int d = 0;
            try
            {
                string url = "https://mrpronto.vertigma.com/umbraco/api/CartApi/NumberOfItemsInCart?memberId=" + memberId;
                HttpResponseMessage response = await Client.GetAsync(url).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<int>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<bool> InsertIntoCart(InsertIntoCartPost m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "InsertIntoCart"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<bool> DeleteFromCart(DeleteFromCartPost m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "DeleteFromCart"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<string> FinishOrder(MoradaPostServico model)
        {
            string d = "";
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "FinishOrder"));
                var response = await Client.PostAsync(uri, model.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    d = JsonConvert.DeserializeObject<string>(content);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return d;
        }

        public async Task<bool> ComprovativoPost(ComprovativoPostModel m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/CartApi/{0}", "ComprovativoPost"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }
    }
}