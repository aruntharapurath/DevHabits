using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DevHabit.Api.DTOs.Auth;
using DevHabit.Api.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DevHabit.Api.Services;

public sealed class TokenProvider(IOptions<JwtAuthOptions> options)
{
    private readonly JwtAuthOptions _jwtAuthOptions = options.Value;

    public AccessTokenDto Create(TokenRequest tokenRequest)
    {
        return new AccessTokenDto(GenerateAccessToken(tokenRequest),GenerateRefreshToken());
    }

    private string GenerateAccessToken(TokenRequest tokenRequest)
    {
        var securityKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_jwtAuthOptions.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        List<Claim> claims =
        [
            new(JwtRegisteredClaimNames.Sub, tokenRequest.UserId),
            new(JwtRegisteredClaimNames.Email, tokenRequest.Email)
        ];

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_jwtAuthOptions.ExpirationInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtAuthOptions.Issuer,
            Audience = _jwtAuthOptions.Audience
        };
        
        var handler = new JsonWebTokenHandler();

        var accessToken = handler.CreateToken(tokenDescriptor);
        return accessToken;
    }
    private string GenerateRefreshToken()
    {
        return string.Empty;
    }
}
