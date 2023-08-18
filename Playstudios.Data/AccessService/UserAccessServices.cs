namespace Playstudios.Data.AccessService
{
    using Playstudios.Data.Contracts;
    using Playstudios.Data.Models.Entities;

    public class UserAccessServices : IUserAccessServices
    {
        private readonly IDatabaseContext databaseContext;

        public UserAccessServices(IDatabaseContext _databaseContext) 
        { 
            databaseContext= _databaseContext;
        }

        public UserEntity? GetUserByEmail(string email) =>
            databaseContext.Users
                .FirstOrDefault(user => user.Email.Equals(email));

        public UserEntity? GetUserByResetPasswordCode(string resetPasswordCode) =>
            databaseContext.Users
                .FirstOrDefault(user => user.ResetPasswordCode.Equals(resetPasswordCode));

        public async Task<UserEntity> CreateUser(string name,
            string lastName,
            string email,
            string password)
        {
            UserEntity userEntity = new UserEntity
            {
                Email = email,
                Id = Guid.NewGuid(),
                LastName = lastName,
                Name = name,
                Password = password
            };

            await databaseContext.Users
                .AddAsync(userEntity)
                .ConfigureAwait(false);

            await databaseContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return userEntity;
        }

        public async Task<UserEntity> CreateResetPasswordCode(string email)
        {
            var userEntity = GetUserByEmail(email);

            userEntity.ResetPasswordCode = Guid.NewGuid().ToString();

            databaseContext.Users
                .Update(userEntity);

            await databaseContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return userEntity;
        }

        public async Task<UserEntity> UpdatePasswordCode(string resetPasswordCode, string password)
        {
            var userEntity = GetUserByResetPasswordCode(resetPasswordCode);

            userEntity.ResetPasswordCode = null;
            userEntity.Password = password;

            databaseContext.Users
                .Update(userEntity);

            await databaseContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return userEntity;
        }
    }
}
