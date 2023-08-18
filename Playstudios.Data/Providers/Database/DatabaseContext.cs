namespace Playstudios.Data.Providers.Database
{
    using Microsoft.EntityFrameworkCore;
    using Playstudios.Data.Contracts;
    using Playstudios.Data.Models.Entities;

    public class DatabaseContext : DbContext, IDatabaseContext
    {
        private const string SCHEMA = "Account";

        private const string MIGRATION_TABLE_NAME = "_Account_MigrationHistory";

        private const string SQL_SERVER_DEFAULT_CONNECTION = "Server=USER-PC\\ALFEM;Database=playstudio-dev-db;Integrated Security=True;TrustServerCertificate=True;";

        public DbSet<UserEntity> Users { get; set; }

        public DbSet<SessionEntity> Sessions { get; set; }

        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
       : base(options)
        { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(SQL_SERVER_DEFAULT_CONNECTION,
                        x => x.MigrationsHistoryTable(MIGRATION_TABLE_NAME, SCHEMA));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(SCHEMA);
        }
    }
}
