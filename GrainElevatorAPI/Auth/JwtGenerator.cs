using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace GrainElevatorAPI.Auth;

public static class JwtGenerator
{
    public static string GenerateJwt(Employee employee, string roleTitle, string tokenKey, DateTime expiryDate)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, employee.Id.ToString()),
            new Claim(ClaimTypes.Role, roleTitle)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
        var jwtToken = new JwtSecurityToken(
            claims: claims,
            expires: expiryDate,
            signingCredentials: creds);
        
        return new JwtSecurityTokenHandler().WriteToken(jwtToken);
    }
}