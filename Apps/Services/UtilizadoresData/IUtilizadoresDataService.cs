using Apps.Models;
using System.Threading.Tasks;

namespace Apps.SettingsData.Utilizadores
{
    public interface IUtilizadoresDataService
    {
        Task<Utilizador> LoginPostAsync(string username, string password);
        Task<bool> LogoutPostAsync();
        Task<Utilizador> LoginByDevicePostAsync(string deviceId);
        Task<string> ClienteChangePwdPostAsync(UtilizadorNovaPwd UtilizadorNovaPwd);
        Task<string> ClienteChangePwd2PostAsync(string nova_pwd);
        Task<bool> UtilizadorEditarDadosPostAsync(UtilizadorEditarDados m);
        Task<bool> UtilizadorUploadFotoPostAsync(UtilizadorUploadFoto m);
        Task<DataModel> SaveClientePostAsync(UtilizadorRegisto utilizador);
    }
}