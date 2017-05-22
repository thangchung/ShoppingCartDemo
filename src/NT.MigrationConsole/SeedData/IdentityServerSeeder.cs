using System.Collections.Generic;
using IdentityServer4.Models;

namespace BlogCore.MigrationConsole.SeedData
{
    public static class IdentityServerSeeder
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("shoppingcart_identity_scope",
                    new[]
                    {
                        "role",
                        "admin",
                        "user",
                        "shoppingcart",
                        "shoppingcart__admin",
                        "shoppingcart__user"
                    })
            };
        }

        /// <summary>
        /// https://leastprivilege.com/2016/12/01/new-in-identityserver4-resource-based-configuration/
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "customer_api_resource",
                    DisplayName = "Customer API Resource",
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    Scopes =
                    {
                        new Scope
                        {
                            Name = "customer_service",
                            DisplayName = "The Customer API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user",
                                "customer_service_customers",
                                "customer_service_customers__admin",
                                "customer_service_customers__user"
                            }
                        }
                    }
                }
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new[]
            {
                // swagger UI
                new Client
                {
                    ClientId = "swagger",
                    ClientName = "swagger",
                    ClientSecrets = new List<Secret> {new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:8888/swagger/o2c.html"
                    },
                    PostLogoutRedirectUris = {"http://localhost:8888/swagger"},
                    AllowedCorsOrigins = {"http://localhost:8888"},
                    AccessTokenLifetime = 300,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "shoppingcart_identity_scope",
                        "customer_service"
                    }
                }
            };
        }
    }
}