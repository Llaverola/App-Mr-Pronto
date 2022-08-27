using Apps.Models;
using System.Threading.Tasks;

namespace Apps.SettingsData.Utilizadores
{
    public class UtilizadoresManager
    {
        readonly IUtilizadoresDataService restService;

        public UtilizadoresManager(IUtilizadoresDataService service)
        {
            restService = service;
        }
        public Task<Utilizador> LoginPostAsync(string username, string password)
        {
            return Task.Run(() => restService.LoginPostAsync(username, password));
        }

        public Task<bool> LogoutPostAsync()
        {
            return Task.Run(() => restService.LogoutPostAsync());
        }
        public Task<DataModel> SaveClientePostAsync(UtilizadorRegisto utilizador)
        {
            return Task.Run(() => restService.SaveClientePostAsync(utilizador));
        }

        public Task<string> ClienteChangePwdPostAsync(UtilizadorNovaPwd UtilizadorNovaPwd)
        {
            return Task.Run(() => restService.ClienteChangePwdPostAsync(UtilizadorNovaPwd));
        }

        public Task<string> ClienteChangePwd2PostAsync(string nova_pwd)
        {
            return Task.Run(() => restService.ClienteChangePwd2PostAsync(nova_pwd));
        }

        public Task<bool> UtilizadorEditarDadosPostAsync(UtilizadorEditarDados m)
        {
            return Task.Run(() => restService.UtilizadorEditarDadosPostAsync(m));
        }

        public Task<bool> UtilizadorUploadFotoPostAsync(UtilizadorUploadFoto m)
        {
            return Task.Run(() => restService.UtilizadorUploadFotoPostAsync(m));
        }
    }
}