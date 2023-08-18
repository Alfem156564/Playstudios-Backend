namespace Playstudios.Data.Contracts
{
    using Playstudios.Data.Models.Entities;

    public interface IUserAccessServices
    {
        UserEntity? GetUserByEmail(string email);

        UserEntity? GetUserByResetPasswordCode(string resetPasswordCode);

        Task<UserEntity> CreateUser(string name,
            string lastName,
            string email,
            string password);

        Task<UserEntity> CreateResetPasswordCode(string email);

        Task<UserEntity> UpdatePasswordCode(string resetPasswordCode, string password);
    }
}
