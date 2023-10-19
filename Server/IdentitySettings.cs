namespace Sparkle.Identity
{
  public class IdentitySettings
  {
    public const string SectionName = "IdentityServer";
    public int AccessTokenLifetime { get; init; }
    public string ClientSecret { get; init; }

  }
}