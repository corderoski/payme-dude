using PayMe.Framework.Data.Entities;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace PayMe.Framework.Data.Context
{
    public interface IBaseDataContext : IDisposable
    {
        Task<int> SecureSaveChangesAsync();
    }

    public interface IDataContext : IDisposable, IBaseDataContext
    {
        /// <summary>
        /// Generates a new Id for the given entity based on a unmatch item.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="predicate">A predicate definif how to look for coincidences</param>
        /// <returns>a string representing the Id, otherwise, null</returns>
        string GenerateEntityId<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, ISyncEntity;

        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        Database Database { get; }


        DbSet<Contact> Contacts { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        DbSet<Tag> Tags { get; set; }

        DbSet<User> Users { get; set; }
        DbSet<UserProfileAuthorization> UserProfileAuthorizations { get; set; }
        DbSet<UserProfileDevice> UserProfileDevices { get; set; }
    }
}
