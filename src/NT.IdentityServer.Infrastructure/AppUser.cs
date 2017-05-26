using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NT.Core.SharedKernel;

namespace NT.IdentityServer.Infrastructure
{
    public class AppUser : IdentityUser
    {
        public Guid NofiticationInfoId { get; set; }

        public virtual AddressInfo ShippingAddress { get; set; }
    }
}
