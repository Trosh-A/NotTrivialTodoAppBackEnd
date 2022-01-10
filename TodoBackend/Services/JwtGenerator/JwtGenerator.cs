using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TodoApp.Backend.Constants;

namespace TodoApp.Backend.Services.JwtGenerator;

public class JwtGenerator : IJwtGenerator
{
  private readonly SymmetricSecurityKey _key;
  private readonly int _TokenLifeTimeInMinutes;

  public JwtGenerator(IConfiguration config, IOptions<AuthOptionsStrings> options)
  {
    _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSecretKey"]));
    _TokenLifeTimeInMinutes = Int32.Parse(config["LifetimeInMinutes"]);
  }

  public string CreateToken(IdentityUser<Guid> user, IEnumerable<Claim> claims)
  {
    var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

    var tokenDescriptor = new SecurityTokenDescriptor
    {
      Subject = new ClaimsIdentity(claims),
      Expires = DateTime.Now.AddMinutes(_TokenLifeTimeInMinutes),
      SigningCredentials = credentials
    };
    var tokenHandler = new JwtSecurityTokenHandler();

    var token = tokenHandler.CreateToken(tokenDescriptor);

    return tokenHandler.WriteToken(token);
  }
}
