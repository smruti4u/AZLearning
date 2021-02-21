using System.Threading.Tasks;

namespace Azwebapp.Services
{
    public interface IKeyVaultService
    {
        Task<string> GetValue();
    }
}