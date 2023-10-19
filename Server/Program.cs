using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace Sparkle.Identity
{
    public class Program
    {
        public static void Main(string[] args)
        {
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


            services.Configure<IdentitySettings>(builder.Configuration
                .GetSection(IdentitySettings.SectionName));

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
        }
    }
}