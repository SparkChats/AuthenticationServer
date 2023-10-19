using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sparkle.Models;

namespace Sparkle.Server
{
    public class AuthenticationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {

        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options) : base(options)
        {
        }
    }
}
