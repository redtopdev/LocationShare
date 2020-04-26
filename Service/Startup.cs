/// <summary>
/// Developer: ShyamSk
/// </summary>
namespace ShareLocation.Service
{
    using Engaze.Core.Web;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Serilog;
    using ShareLocation.CacheManager;

    public class Startup : EngazeStartup
    {
        public Startup(IConfiguration configuration) : base(configuration) { }   

        public override void ConfigureComponentServices(IServiceCollection services)
        {           
            services.AddTransient<ILocationManager, LocationManager>();
            services.AddTransient<ILocationCacheManager, LocationCacheManager>();
        }

        public override void ConfigureComponent(IApplicationBuilder app)
        {
            app.UseAuthorization();
            app.UseSerilogRequestLogging();
        }
    }
}
