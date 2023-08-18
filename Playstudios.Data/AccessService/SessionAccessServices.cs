namespace Playstudios.Data.AccessService
{
    using Microsoft.EntityFrameworkCore;
    using Playstudios.Data.Contracts;
    using Playstudios.Data.Models.Entities;

    public class SessionAccessServices : ISessionAccessServices
    {
        private readonly IDatabaseContext databaseContext;

        public SessionAccessServices(IDatabaseContext _databaseContext) 
        { 
            databaseContext= _databaseContext;
        }

        public SessionEntity? GetSessionById(Guid id) =>
            databaseContext.Sessions
                .Include(session => session.User)
                .FirstOrDefault(session => session.Id.Equals(id));

        public async Task<SessionEntity> CreateSession(Guid userId)
        {
            SessionEntity sessionEntity = new SessionEntity
            {
                Id = Guid.NewGuid(),
                UserEntityId = userId
            };

            await databaseContext.Sessions
                .AddAsync(sessionEntity)
                .ConfigureAwait(false);

            await databaseContext
                .SaveChangesAsync()
                .ConfigureAwait(false);

            return sessionEntity;
        }
    }
}
