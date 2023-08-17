using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Mango.Services.Identity
{
    /// <summary>
    /// Static Details Class
    /// </summary>
    public static class SD
    {
        public const string Admin = nameof(Admin);
        public const string Customer = nameof(Customer);

        /// <summary>
        /// things that we want to protect in our IDs
        /// </summary>
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
            };

        /// <summary>
        /// identifiers for resources that the CLIENT want to access
        /// there are two types of scopes:
        /// - Identity Scope: could contain FirstName, LastName, Profile, etc
        /// - Resource Scope: 
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("mango", "Mango Server"),
                new ApiScope("read", "Read Db Data"),
                new ApiScope("write", "Write into Db"),
                new ApiScope("delete", "Delete from Db"),
            };

        /// <summary>
        /// CLIENT piece of software that request the token from IDs
        /// it can be for authenticating user or for accessing resource
        /// example: web app, mobile app, etc.
        /// </summary>
        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId = "MangoWeb",
                    ClientSecrets = { new Secret("secret".Sha256()) },
                    AllowedGrantTypes = GrantTypes.Code,
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "mango"
                    },
                    RedirectUris = { "https://localhost:7253/signin-oidc" },
                    PostLogoutRedirectUris = { "https://localhost:7253/signout-callback-oidc" }
                }
            };
    }
}
