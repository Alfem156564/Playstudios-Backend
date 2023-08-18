namespace Playstudios.Core.Contracts
{
    using Playstudios.Common.Models.Dto;
    using Playstudios.Common.Models;

    public interface IAccountManager
    {
        Task<ManagerResult<SessionDto>> CreateUserSession(
            string email,
            string password);

        ManagerResult<UserDto> GetUserSession(
            Guid id);
    }
}
