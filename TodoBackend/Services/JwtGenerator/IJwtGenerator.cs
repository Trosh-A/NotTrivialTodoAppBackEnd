using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace TodoApp.Backend.Services.JwtGenerator;

public interface IJwtGenerator
{
  string CreateToken(IdentityUser<Guid> user, IEnumerable<Claim> claims);
}
