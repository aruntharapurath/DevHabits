namespace DevHabit.Api.Settings;

public class JwtAuthOptions
{
    public const string SectionName = "Jwt";
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Key { get; set; }
    public required int ExpirationInMinutes { get; set; }
    public required int RefreshTokenExpirationInDays { get; set; }
}
