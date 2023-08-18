namespace Playstudios.Core.Contracts
{
    using Playstudios.Common.Models;
    using Playstudios.Common.Models.Dto;

    public interface IUserManager
    {
        Task<ManagerResult<UserDto>> CreateUser(
            string name,
            string lastName,
            string email,
            string password);

        Task<ManagerResult<ResponseDto>> CreateResetPasswordCode(string email);

        Task<ManagerResult<ResponseDto>> UpdatePasswordWithCode(
            string resetPasswordCode,
            string password);
    }
}
