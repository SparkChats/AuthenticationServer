namespace Sparkle.Identity
{
    public class IdentitySettings
    {
        public const string SectionName = "IdentityServer";
        public int AccessTokenLifetime { get; init; }
        public string ClientSecret { get; init; } = null!;
        public string ReturnUrl { get; init; } = null!;
        public ICollection<string> RedirectUris { get; init; } = null!;
        public ICollection<string> PostLogoutRedirectUris { get; init; } = null!;
    }
}