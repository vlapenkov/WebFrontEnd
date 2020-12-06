using IdentityServer4;
using IdentityServer4.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace AspNetCoreIdentityServer.Configurations
{
    public class Config
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
{
                // в access_token для mvc (Api1.Scope) получу клеймы first и second
        new ApiResource("Api1", "API1") {
            Scopes=new[] {new Scope("Api1.Scope") },    UserClaims=new[]{"first","second" } },

        new ApiResource("Api2", "API2") {
            Scopes=new[] {new Scope("Api2.Scope") },    UserClaims=new[]{"third","forth" } }

            };
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {

            var customProfile = new IdentityResource(
        name: "custom.profile",
        displayName: "Custom profile",
        claimTypes: new[] { "first" });


            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
               new IdentityResources.Profile(),
                new IdentityResources.Phone(),
                customProfile
             
            };
        }



        public static IEnumerable<Client> GetClients(IConfiguration configuration)
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = {GrantType.Hybrid ,GrantType.ResourceOwnerPassword },

                    RequireConsent = false,

                    ClientSecrets =
                    {
                        new Secret("secret".Sha256())
                    },

                  //  RedirectUris = {"http://192.168.0.106:5008/signin-oidc"},
                  //  PostLogoutRedirectUris = {"http://192.168.0.106:5008/signout-callback-oidc"},
                    RedirectUris = {$"{configuration["Services:Webfrontend"]}/signin-oidc"},
                    PostLogoutRedirectUris = {$"{configuration["Services:Webfrontend"]}/signout-callback-oidc"},
                   
                    AllowedScopes =
                    {
                        "openid",
                        "profile",
                        "Api1.Scope",
                        "Api2.Scope",
                        IdentityServerConstants.StandardScopes.Phone,
                        "custom.profile"
                    },
                    AllowOfflineAccess = true,
                    AlwaysSendClientClaims = true,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    
                }
                
            };
        }
    }
}
