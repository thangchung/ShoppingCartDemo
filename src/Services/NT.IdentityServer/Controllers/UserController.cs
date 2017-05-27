using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NT.IdentityServer.Infrastructure;

namespace NT.IdentityServer.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{id}")]
        public async Task<AppUser> Get(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }
    }
}