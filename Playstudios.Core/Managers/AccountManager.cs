namespace Playstudios.Core.Managers
{
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Helpers;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Common.Models;
    using Playstudios.Data.Contracts;
    using Playstudios.Core.Helpers;
    using Playstudios.Core.Contracts;

    public class AccountManager : IAccountManager
    {
        private readonly IUserAccessServices userAccessServices;

        private readonly ISessionAccessServices sessionAccessServices;

        public AccountManager(IUserAccessServices _userAccessServices, 
            ISessionAccessServices _sessionAccessServices)
        {
            userAccessServices = _userAccessServices;
            sessionAccessServices = _sessionAccessServices;
        }

        public async Task<ManagerResult<SessionDto>> CreateUserSession(
            string email,
            string password)
        {
            var user = userAccessServices
                .GetUserByEmail(email);

            if (user == null)
                return ManagerResult<SessionDto>
                    .FromError(ErrorCodesEnum.UserEmailNotFound);

            string decryptedPassword = AESEncriptationHelper
                .Decrypt(user.Password);

            if (!decryptedPassword.Equals(password))
                return ManagerResult<SessionDto>
                    .FromError(ErrorCodesEnum.UserPasswordNotFound);

            var session = await sessionAccessServices
                .CreateSession(user.Id)
                .ConfigureAwait(false);

            return ManagerResult<SessionDto>
                .FromSuccess(new SessionDto
                {
                    Id = session.Id,
                    User = user.ToDto()
                });
        }

        public ManagerResult<UserDto> GetUserSession(
            Guid id)
        {
            var session = sessionAccessServices
                .GetSessionById(id);

            if (session == null)
                return ManagerResult<UserDto>
                    .FromError(ErrorCodesEnum.SessionNotFound);

            if (session.ExpirationDate < DateTime.UtcNow)
                return ManagerResult<UserDto>
                    .FromError(ErrorCodesEnum.SessionExpired);

            return ManagerResult<UserDto>
                .FromSuccess(session.User.ToDto());
        }
    }
}
