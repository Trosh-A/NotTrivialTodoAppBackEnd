using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace TodoApp.Backend.Services.JwtGenerator;

public interface IJwtGenerator
{
  string CreateToken(IdentityUser<Guid> user, IEnumerable<Claim> claims);
}
