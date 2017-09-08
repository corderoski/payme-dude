using Microsoft.Azure.Mobile.Server.Tables;
using PayMe.Framework.Data.DTO;
using PayMe.Framework.Data.Entities;
using System.Threading.Tasks;

namespace PayMe.Framework.Services
{
    public interface IAuthManagerService
    {
        Task<AuthResult> RegisterAuthAsync(IDomainManager<User> domainManager, RegisterAuth model);
        Task<AuthResult> VerifyAuthAsync(string deviceUniqueIdOrUserId);

    }
}
