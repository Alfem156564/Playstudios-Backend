namespace Playstudios.Core.Managers
{
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Helpers;
    using Playstudios.Common.Models;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Core.Contracts;
    using Playstudios.Core.Helpers;
    using Playstudios.Data.Contracts;

    public class UserManager : IUserManager
    {
        private readonly IUserAccessServices userAccessServices;
        private readonly ISendinblue emailProvider;

        public UserManager(IUserAccessServices _userAccessServices, 
            ISendinblue _emailProvider)
        {
            userAccessServices = _userAccessServices;
            emailProvider = _emailProvider;
        }

        public async Task<ManagerResult<UserDto>> CreateUser(
            string name,
            string lastName,
            string email,
            string password)
        {
            var user = userAccessServices
                .GetUserByEmail(email);

            if (user != null)
                return ManagerResult<UserDto>
                    .FromError(ErrorCodesEnum.UserEmailAllreadyUsed);

            string encryptedPassword = AESEncriptationHelper
                .Encrypt(password);

            user = await userAccessServices
                .CreateUser(name, 
                    lastName, 
                    email, 
                    encryptedPassword)
                .ConfigureAwait(false);

            return ManagerResult<UserDto>
                .FromSuccess(user.ToDto());
        }

        public async Task<ManagerResult<ResponseDto>> CreateResetPasswordCode(string email)
        {
            var user = userAccessServices
                .GetUserByEmail(email);

            if (user == null)
                return ManagerResult<ResponseDto>
                    .FromError(ErrorCodesEnum.UserEmailNotFound);

            user = await userAccessServices
                .CreateResetPasswordCode(email)
                .ConfigureAwait(false);

            if (!emailProvider.SendResetPasswordCode(user.Email, user.Name, user.ResetPasswordCode))
                return ManagerResult<ResponseDto>
                    .FromError(ErrorCodesEnum.SendEmailError);

            return ManagerResult<ResponseDto>
                .FromSuccess(new ResponseDto
                {
                    Message = "Reset password email send."
                });
        }

        public async Task<ManagerResult<ResponseDto>> UpdatePasswordWithCode(
            string resetPasswordCode,
            string password)
        {
            var user = userAccessServices
                .GetUserByResetPasswordCode(resetPasswordCode);

            if (user == null)
                return ManagerResult<ResponseDto>
                    .FromError(ErrorCodesEnum.UserNotFound);

            string encryptedPassword = AESEncriptationHelper
                .Encrypt(password);

            await userAccessServices
                .UpdatePasswordCode(resetPasswordCode, encryptedPassword)
                .ConfigureAwait(false);

            return ManagerResult<ResponseDto>
                .FromSuccess(new ResponseDto
                {
                    Message = "Password was updated."
                });
        }
    }
}
