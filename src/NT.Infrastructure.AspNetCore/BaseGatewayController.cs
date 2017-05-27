using System;
using Microsoft.AspNetCore.Mvc;

namespace NT.Infrastructure.AspNetCore
{
    // [Authorize]
    public abstract class BaseGatewayController : Controller
    {
        protected readonly RestClient RestClient;

        protected BaseGatewayController(RestClient restClient)
        {
            RestClient = restClient;
        }

        ~BaseGatewayController()
        {
            GC.SuppressFinalize(RestClient);
        }
    }
}