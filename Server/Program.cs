using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Sparkle.Identity;
using Sparkle.Models;
using Sparkle.Server;
using System.Reflection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

services.AddControllers();
services.AddMvc(options =>
{
    options.EnableEndpointRouting = false;
});

services.AddMediatR(options =>
{
    options.RegisterServicesFromAssembly(Assembly
        .GetExecutingAssembly());
});

services.AddDbContext<AuthenticationDbContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("SqlServer")
        ?? throw new Exception("No SqlServer connection string provided");
    options.UseSqlServer(connectionString);
});

services.Configure<IdentitySettings>(builder.Configuration
    .GetSection(IdentitySettings.SectionName));

services.AddIdentity<User, IdentityRole<Guid>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Lockout.MaxFailedAccessAttempts = 5;

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;

    options.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<AuthenticationDbContext>()
    .AddDefaultTokenProviders();

services.AddIdentityServer4WithConfiguration()
    .AddDeveloperSigningCredential();

services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Sparkle.Identity.Cookie";
    config.LoginPath = "/Authentication/Login";
    config.LogoutPath = "/Authentication/Logout";
});

WebApplication app = builder.Build();

app.MapControllers();
app.UseMvc();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/wwwroot"
});

app.UseIdentityServer();

app.Run();
