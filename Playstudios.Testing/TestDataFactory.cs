namespace Playstudios.Testing
{
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Playstudios.Data.Models.Entities;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal static class TestDataFactory
    {
        public static Guid userId = Guid.NewGuid();
        public static Guid activeSessionId = Guid.NewGuid();
        public static Guid expiredSessionId = Guid.NewGuid();
        public static string resetPasswordCode = Guid.NewGuid().ToString();

        public static Mock<DbSet<UserEntity>> GetUserMock()
        {
            return new List<UserEntity>
            {
                new UserEntity
                    {
                        Id = userId,
                        Name = "Alan",
                        LastName = "Meza",
                        Email = "darciaitachi@hotmail.com",
                        Password = "wEno7uMszBg8zG0MF+KGBQ==",
                        ResetPasswordCode = null
                    },
                new UserEntity
                    {
                        Id = Guid.NewGuid(),
                        Name = "Alan",
                        LastName = "Meza",
                        Email = "alanfma@hotmail.com",
                        Password = "wEno7uMszBg8zG0MF+KGBQ==",
                        ResetPasswordCode = resetPasswordCode
                    }
            }.AsDbSetMock();
        }

        public static Mock<DbSet<SessionEntity>> GetSessionMock()
        {
            return new List<SessionEntity>
            {
                new SessionEntity
                    {
                        Id = activeSessionId,
                        ExpirationDate = DateTime.UtcNow.AddDays(10),
                        UserEntityId = userId,
                        User = new UserEntity
                        {
                            Id = userId,
                            Name = "Alan",
                            LastName = "Meza",
                            Email = "darciaitachi@hotmail.com",
                            Password = "wEno7uMszBg8zG0MF+KGBQ=="
                        }
                    },
                new SessionEntity
                    {
                        Id = expiredSessionId,
                        ExpirationDate = DateTime.UtcNow.AddDays(-10),
                        UserEntityId = userId,
                        User = new UserEntity
                        {
                            Id = userId,
                            Name = "Alan",
                            LastName = "Meza",
                            Email = "darciaitachi@hotmail.com",
                            Password = "wEno7uMszBg8zG0MF+KGBQ=="
                        }
                    },
            }.AsDbSetMock();
        }

        public static Mock<DbSet<T>> AsDbSetMock<T>(this IEnumerable<T> data) where T : class
        {
            var queryable = (data ?? Enumerable.Empty<T>()).AsQueryable();
            var dbSetMock = new Mock<DbSet<T>>();

            dbSetMock
           .As<IQueryable<T>>()
           .Setup(dbSet => dbSet.Provider)
           .Returns(new TestAsyncQueryProvider<T>(queryable.Provider));

            dbSetMock
           .As<IQueryable<T>>()
           .Setup(dbSet => dbSet.Expression)
           .Returns(queryable.Expression);

            dbSetMock
           .As<IQueryable<T>>()
           .Setup(dbSet => dbSet.ElementType)
           .Returns(queryable.ElementType);

            dbSetMock
           .As<IQueryable<T>>()
           .Setup(dbSet => dbSet.GetEnumerator())
           .Returns(queryable.GetEnumerator());

            dbSetMock
           .As<IAsyncEnumerable<T>>()
           .Setup(dbSet => dbSet.GetAsyncEnumerator(CancellationToken.None))
           .Returns(new TestAsyncEnumerator<T>(queryable.GetEnumerator()));

            return dbSetMock;
        }

        public static Mock<DbSet<T>> CreateDbSetMock<T>() where T : class =>
         AsDbSetMock(Enumerable.Empty<T>());
    }
}
