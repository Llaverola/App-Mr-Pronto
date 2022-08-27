using Apps.Models;
using Apps.ViewModels;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace Apps.SettingsData.Utilizadores
{
    public class UtilizadoresService : IUtilizadoresDataService
    {
        private HttpClient Client { get; set; }
        public UtilizadoresService()
        {
            Client = new HttpClient();
        }

        public async Task<Utilizador> LoginPostAsync(string username, string password)
        {
            Utilizador _u = new Utilizador();
            try
            {
                Uri uri = new Uri(string.Format(MyConstants.LOGIN_BASE_URL, "Login"));
                var model = new { UserName = username, Password = General.Encrypt(password), DeviceID = App.DeviceIdentifier };
                HttpResponseMessage response = await Client.PostAsync(uri, model.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retContent = await response.Content.ReadAsStringAsync();
                    _u = JsonConvert.DeserializeObject<Utilizador>(retContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return _u;
        }

        public async Task<bool> LogoutPostAsync()
        {
            bool logout = true;
            try
            {
                Uri uri = new Uri(string.Format(MyConstants.LOGIN_BASE_URL, "Logout"));
                var model = new { UserName = App.DataModel.Utilizador.Username };
                HttpResponseMessage response = await Client.PostAsync(uri, model.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    App.DataModel.Utilizador = new Utilizador() { DispositivoId = App.DeviceIdentifier };
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
                logout = false;
            }
            return logout;
        }

        public async Task<string> ClienteChangePwdPostAsync(UtilizadorNovaPwd UtilizadorNovaPwd)
        {
            string sucesso = "-1";
            try
            {
                Uri uri = new Uri(string.Format(MyConstants.LOGIN_BASE_URL, "ClienteChangePwdPostAsync"));
                HttpResponseMessage response = await Client.PostAsync(uri, UtilizadorNovaPwd.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retContent = await response.Content.ReadAsStringAsync();
                    var s = JsonConvert.DeserializeObject<bool>(retContent);
                    if (s)
                    {
                        sucesso = "1";
                    }
                    else
                    {
                        sucesso = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return sucesso;
        }

        public async Task<string> ClienteChangePwd2PostAsync(string nova_pwd)
        {
            string sucesso = "-1";
            try
            {
                Uri uri = new Uri(string.Format(MyConstants.LOGIN_BASE_URL, "ClienteChangePwd2PostAsync"));
                var m = new { 
                    username = App.DataModel.Utilizador.Username,
                    password = nova_pwd
                };
                HttpResponseMessage response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retContent = await response.Content.ReadAsStringAsync();
                    var s = JsonConvert.DeserializeObject<bool>(retContent);
                    if (s)
                    {
                        sucesso = "1";
                    }
                    else
                    {
                        sucesso = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return sucesso;
        }

        public async Task<Utilizador> LoginByDevicePostAsync(string deviceId)
        {
            Utilizador _u = new Utilizador();
            try
            {
                Uri uri = new Uri(string.Format(MyConstants.LOGIN_BASE_URL, "LoginByDevice"));
                MultipartFormDataContent content = new MultipartFormDataContent();
                content.Headers.ContentType.MediaType = "multipart/form-data";
                content.Add(new StringContent(deviceId), "DeviceID");
                HttpResponseMessage response = await Client.PostAsync(uri, content).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retContent = await response.Content.ReadAsStringAsync();
                    _u = JsonConvert.DeserializeObject<Utilizador>(retContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return _u;
        }

        public async Task<DataModel> SaveClientePostAsync(UtilizadorRegisto utilizador)
        {
            DataModel u = new DataModel();
            try
            {
                string url = string.Format(MyConstants.LOGIN_BASE_URL, "SaveUtilizadorPostAsync");
                Uri uri = new Uri(url);
                var response = await Client.PostAsync(uri, utilizador.AsJson()).ConfigureAwait(false);
                if (response.IsSuccessStatusCode)
                {
                    string retContent = await response.Content.ReadAsStringAsync();
                    u = JsonConvert.DeserializeObject<DataModel>(retContent);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return u;
        }

        public async Task<bool> UtilizadorEditarDadosPostAsync(UtilizadorEditarDados m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/MobileApi/{0}", "UtilizadorEditarDados"));
                var response = await Client.PostAsync(uri, m.AsJson()).ConfigureAwait(false);
                retValue = response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(@"\tERROR {0}", ex.Message);
            }
            return retValue;
        }

        public async Task<bool> UtilizadorUploadFotoPostAsync(UtilizadorUploadFoto m)
        {
            bool retValue = false;
            try
            {
                Uri uri = new Uri(string.Format("https://mrpronto.vertigma.com/umbraco/api/MobileApi/{0}", "UtilizadorUploadFoto"));
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