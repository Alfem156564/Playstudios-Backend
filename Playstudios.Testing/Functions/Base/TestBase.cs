namespace Playstudios.Testing.Functions.Base
{
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Playstudios.Data.AccessService;
    using Playstudios.Data.Contracts;

    public class TestBase
    {
        private ServiceProvider serviceProvider;

        private ServiceCollection services = new ServiceCollection();


        #region DataProviders

        protected Mock<IDatabaseContext> databaseContextMock;

        protected IDatabaseContext? databaseContextProvider => serviceProvider?.GetService<IDatabaseContext>();

        protected Mock<ISendinblue> sendinblueMock;

        protected ISendinblue? sendinblueProvider => serviceProvider?.GetService<ISendinblue>();

        #endregion

        #region DataProviders

        protected IUserAccessServices? userAccessServices => serviceProvider?.GetService<IUserAccessServices>();
        protected ISessionAccessServices? sessionAccessServices => serviceProvider?.GetService<ISessionAccessServices>();
        #endregion

        public TestBase()
        {
            InitializeDatabaseContextMock();
            InitializeServices();
        }

        public TService? GetService<TService>() where TService : class
        {
            return serviceProvider?
                .GetService<TService>();
        }

        public void RegisterService<TService, TImplementation>() where TService : class
            where TImplementation : class, TService
        {
            services.AddScoped<TService, TImplementation>();
        }

        public void RegisterService<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            services.AddScoped(implementationFactory);
        }

        public void RegisterService<TService, TImplementation>(Func<IServiceProvider, TService> implementationFactory) where TService : class
        {
            services.AddScoped(implementationFactory);
        }

        protected virtual void InitializeServices()
        {

            InitializeDataProviders(services);
            InitializeDataAccessProviders(services);

            serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void InitializeDatabaseContextMock()
        {
            databaseContextMock = new Mock<IDatabaseContext>();
            sendinblueMock = new Mock<ISendinblue>();
        }

        private void InitializeDataProviders(ServiceCollection services)
        {
            if (databaseContextProvider == null) services.AddScoped(x => databaseContextMock.Object);
            if (sendinblueProvider == null) services.AddScoped(x => sendinblueMock.Object);
        }

        private void InitializeDataAccessProviders(ServiceCollection services)
        {
            if (userAccessServices == null) services.AddScoped<IUserAccessServices, UserAccessServices>();
            if (sessionAccessServices == null) services.AddScoped<ISessionAccessServices, SessionAccessServices>();
        }
    }
}
