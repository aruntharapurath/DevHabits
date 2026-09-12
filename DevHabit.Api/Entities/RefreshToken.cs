using Microsoft.AspNetCore.Identity;

namespace DevHabit.Api.Entities;

public class RefreshToken
{
    public Guid Id { get; set; }
    public required string userId { get; set; }
    public required string Token { get; set; }
    public required DateTime ExpiresAtUtc { get; set; }
    public IdentityUser User { get; set; }
}
