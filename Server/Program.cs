using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Sparkle.Identity;
using Sparkle.Models;
using Sparkle.Server;
using Sparkle.Server.Common.Options;
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
    string connectionString = builder.Configuration.GetConnectionString("Postgres")
        ?? throw new Exception("No SqlServer connection string provided");
    options.UseNpgsql(connectionString);
});

services.Configure<IdentitySettings>(builder.Configuration
    .GetSection(IdentitySettings.SectionName));

services.Configure<ApiOptions>(builder.Configuration.GetSection(ApiOptions.SectionName));

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

services.AddIdentityServerWithConfiguration(options =>
{
    options.IssuerUri = "https://sparkle.net.ua/auth";
    options.Endpoints.EnableTokenEndpoint = true;
});

services.ConfigureApplicationCookie(options =>
{
    options.Cookie.Name = "Sparkle.Identity.Cookie";
    options.LoginPath = "/Authentication/Login";
    options.LogoutPath = "/Authentication/Logout";
});



WebApplication app = builder.Build();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(app.Environment.ContentRootPath, "wwwroot")),
    RequestPath = "/wwwroot"
});

app.UseHttpsRedirection();

app.UseIdentityServer();
app.MapControllers();

app.UseMvc();
ForwardedHeadersOptions fordwardedHeaderOptions = new()
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
};
fordwardedHeaderOptions.KnownNetworks.Clear();
fordwardedHeaderOptions.KnownProxies.Clear();

app.UseForwardedHeaders(fordwardedHeaderOptions);

app.Run();
