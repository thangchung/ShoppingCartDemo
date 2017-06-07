using System.Collections.Generic;
using IdentityServer4.Models;

namespace NT.IdentityServer.Migrator.DataSeeder
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
        ///     https://leastprivilege.com/2016/12/01/new-in-identityserver4-resource-based-configuration/
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new[]
            {
                new ApiResource
                {
                    Name = "shopping_cart_api_resource",
                    DisplayName = "Shopping Cart API Resource",
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
                                "customer_service",
                                "customer_service__admin",
                                "customer_service__user"
                            }
                        },
                        new Scope
                        {
                            Name = "order_service",
                            DisplayName = "The Order API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user",
                                "order_service",
                                "order_service__admin",
                                "order_service__user"
                            }
                        },
                        new Scope
                        {
                            Name = "catalog_service",
                            DisplayName = "The Catalog API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user",
                                "catalog_service",
                                "catalog_service__admin",
                                "catalog_service__user"
                            }
                        },
                        new Scope
                        {
                            Name = "payment_service",
                            DisplayName = "The Payment API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user",
                                "payment_service",
                                "payment_service__admin",
                                "payment_service__user"
                            }
                        },
                        new Scope
                        {
                            Name = "user_service",
                            DisplayName = "The User API scope.",
                            UserClaims =
                            {
                                "role",
                                "admin",
                                "user",
                                "user_service",
                                "user_service__admin",
                                "user_service__user"
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
                        "customer_service",
                        "order_service",
                        "catalog_service",
                        "payment_service",
                        "user_service"
                    }
                }, // client web application
                new Client
                {
                    ClientId = "web_app",
                    ClientName = "web_app",
                    ClientUri = "http://localhost:3000",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowAccessTokensViaBrowser = true,
                    RedirectUris =
                    {
                        "http://localhost:3000/callback"
                    },
                    PostLogoutRedirectUris = {"http://localhost:3000"},
                    AllowedCorsOrigins = {"http://localhost:3000"},
                    AccessTokenLifetime = 300,
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "role",
                        "admin",
                        "shoppingcart_identity_scope",
                        "shopping_cart_api_resource",
                        "customer_service",
                        "order_service",
                        "catalog_service",
                        "payment_service",
                        "user_service"
                    }
                }
            };
        }
    }
}