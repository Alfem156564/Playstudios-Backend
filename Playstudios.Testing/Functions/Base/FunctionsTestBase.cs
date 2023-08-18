namespace Playstudios.Testing.Functions.Base
{
    using Playstudios.Core.Contracts;
    using Playstudios.Core.Managers;

    public class FunctionsTestBase : TestBase
    {

        #region Managers
        protected IUserManager? userManager => GetService<IUserManager>();
        protected IAccountManager? accountManager => GetService<IAccountManager>();

        #endregion

        protected override void InitializeServices()
        {
            InitializeManagers();
            base.InitializeServices();
        }

        private void InitializeManagers()
        {
            if (userManager == null) RegisterService<IUserManager, UserManager>(x => new UserManager(userAccessServices, sendinblueProvider));
            if (accountManager == null) RegisterService<IAccountManager, AccountManager>(x => new AccountManager(userAccessServices, sessionAccessServices));
        }
    }
}
