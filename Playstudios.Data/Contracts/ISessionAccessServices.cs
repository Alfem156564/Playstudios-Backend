using Playstudios.Data.Models.Entities;

namespace Playstudios.Data.Contracts
{
    public interface ISessionAccessServices
    {
        SessionEntity? GetSessionById(Guid id);

        Task<SessionEntity> CreateSession(Guid userId);
    }
}
