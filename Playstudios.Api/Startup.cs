using Microsoft.Azure.Functions.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Playstudios.Api.Startup))]
namespace Playstudios.Api
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Playstudios.Common.Configuration;
    using Playstudios.Core.Contracts;
    using Playstudios.Core.Managers;
    using Playstudios.Data.AccessService;
    using Playstudios.Data.Contracts;
    using Playstudios.Data.Providers.Database;
    using Playstudios.Data.Providers.Email;

    public class Startup : FunctionsStartup
    {
        private readonly IPlaystudiosConfiguration configuration;

        public Startup()
        {
            this.configuration = new PlaystudiosConfiguration();
        }

        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services
                .AddSingleton(x => configuration);

            AddDataProviders(builder.Services);
            AddAccessService(builder.Services);
            AddManagers(builder.Services);
        }

        private void AddDataProviders(IServiceCollection services)
        {
            services
                .AddDbContext<IDatabaseContext, DatabaseContext>(options =>
                    options.UseSqlServer(configuration.DatabaseConnectionString))
                .AddScoped<ISendinblue, Sendinblue>();
        }

        private void AddAccessService(IServiceCollection services)
        {
            services
                .AddScoped<IUserAccessServices, UserAccessServices>()
                .AddScoped<ISessionAccessServices, SessionAccessServices>();
        }

        private void AddManagers(IServiceCollection services)
        {
            services
                .AddScoped<IUserManager, UserManager>()
                .AddScoped<IAccountManager, AccountManager>();
        }
    }
}
