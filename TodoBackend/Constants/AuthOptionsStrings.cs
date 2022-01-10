#nullable disable

namespace TodoApp.Backend.Constants;

public class AuthOptionsStrings
{
  private const string AuthOptions = "AuthOptions";
  public const string Issuer = $"{AuthOptions}:Issuer";
  public const string Audience = $"{AuthOptions}:Audience";
  public const string LifetimeInMinutes = $"{AuthOptions}:LifetimeInMinutes";
}
