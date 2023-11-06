using System.Security.Claims;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };


    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
            new ApiResource()
            {
                Name = "api1",
                DisplayName = "BrainWave Api",
                ApiSecrets = { new Secret("api1_secret".Sha256()) },
                Scopes = new List<string>() { "api1" },
                UserClaims = { ClaimTypes.Name, ClaimTypes.Email, ClaimTypes.NameIdentifier, ClaimTypes.GivenName },
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("scope1"),
            new ApiScope("scope2"),
            new ApiScope("api1"),
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "api1",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,
                // scopes that client has access to
                AllowedScopes = { "api1" },
                AlwaysIncludeUserClaimsInIdToken = true
            },
            new Client
            {
                ClientId = "webui",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.Code,

                RedirectUris = { "https://localhost:5002/signin-oidc" },
                FrontChannelLogoutUri = "https://localhost:5002/signout-oidc",
                PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },
                
                RequirePkce = true,
                AllowOfflineAccess = true,
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                },
                AlwaysIncludeUserClaimsInIdToken = true
            },
        };
}
