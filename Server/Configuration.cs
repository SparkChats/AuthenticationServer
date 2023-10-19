using IdentityModel;
using IdentityServer4;
using IdentityServer4.Configuration;
using IdentityServer4.Models;
using IdentityServer4.Test;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Sparkle.Identity
{
    public static class Configuration
    {
        private static IdentitySettings _identitySettings;

        /// <summary>
        /// Adds and configures IdentityServer
        /// </summary>
        public static IIdentityServerBuilder AddIdentityServer4(this IServiceCollection services,
             Action<IdentityServerOptions> options)
        {
            return services.AddIdentityServer(options);
        }
        public static IIdentityServerBuilder AddIdentityServer4(this IServiceCollection services)
        {
            return services.AddIdentityServer();
        }

        /// <summary>
        /// Adds and configures IdentityServer with in-memory configuration
        /// </summary>
        public static IIdentityServerBuilder AddIdentityServer4WithConfiguration(this IServiceCollection services,
            Action<IdentityServerOptions>? options = null)
        {
            _identitySettings = services.BuildServiceProvider()
            .GetRequiredService<IOptions<IdentitySettings>>().Value;

            IIdentityServerBuilder builder = options is null
                ? services.AddIdentityServer4()
                : services.AddIdentityServer4(options);

            builder.AddInMemoryIdentityResources(IdentityResources)
             .AddInMemoryApiScopes(ApiScopes)
             .AddInMemoryApiResources(ApiResources)
             .AddInMemoryClients(Clients)
             .AddTestUsers(TestUsers);

            return builder;
        }

        private static IEnumerable<IdentityResource> IdentityResources =>
            new[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResource("roles","User roles", new[]{"role"})
            };
        private static IEnumerable<ApiScope> ApiScopes =>
          new List<ApiScope>
          {
              {
                  new ApiScope
                  {
                      Name = "MessageApi",
                      DisplayName = "Message API"
                  }
              }
          };
        private static IEnumerable<ApiResource> ApiResources =>
          new List<ApiResource>
          {
                new ApiResource("MessageApi", "Message API")
                {
                     UserClaims = new []{ JwtClaimTypes.Name},
                    Scopes = { "MessageApi" }
                }
          };
        private static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new() {
                    ClientId = "react-client",
                    ClientSecrets = { new Secret("react-client-super-secret") },
                    AllowedGrantTypes = GrantTypes.Code,
                    RedirectUris = { "http://localhost:3000/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:3000/signout-oidc" },
                    AllowedCorsOrigins  = { "http://localhost:3000" },
                    AllowedScopes = {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "roles",
                        "MessageApi"
                    },
                    RequireConsent = false,
                    RequireClientSecret = false,
                    //infinitely long access token (not recommended in production)
                    AccessTokenLifetime = _identitySettings.AccessTokenLifetime
                }
            };
        private static List<TestUser> TestUsers =>
            new()
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "testuser",
                    Password = "testuser",
                    Claims = new List<Claim>
                    {
                        new("role", "admin"),
                        new("given_name", "Test"),
                        new("family_name", "User")
                    }
                }
            };
    }
}
