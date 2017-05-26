using System;
using System.Threading.Tasks;

namespace NT.IdentityServer.Infrastructure
{
    public interface IUserRepository
    {
        Task<AppUser> GetByIdAsync(Guid id);
    }
}