namespace Playstudios.Data.Contracts
{
    using Microsoft.EntityFrameworkCore;
    using Playstudios.Data.Models.Entities;

    public interface IDatabaseContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SessionEntity> Sessions { get; set; }
    }
}
