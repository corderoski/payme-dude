using Microsoft.Azure.Mobile.Server.Tables;
using PayMe.Exceptions;
using PayMe.Framework.Data.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Threading.Tasks;

namespace PayMe.Framework.Data.Context
{

    public class DataContextCustomDbConfiguration : DbConfiguration
    {
        public DataContextCustomDbConfiguration()
        {
            SetExecutionStrategy("System.Data.SqlClient",
                () => new SqlAzureExecutionStrategy(MAX_RETRY_TIMES_NUMBER, TimeSpan.FromSeconds(MAX_RETRY_DELAY_SECONDS)));

        }

        private const int MAX_RETRY_TIMES_NUMBER = 2;
        private const int MAX_RETRY_DELAY_SECONDS = 3;
    }

    [DbConfigurationType(typeof(DataContextCustomDbConfiguration))]
    public partial class DataContext : DbContext, IDataContext
    {

        private readonly string _connectionString;

        public DataContext(string connectionString) : base(connectionString)
        {
            _connectionString = connectionString;

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Add(
                new AttributeToColumnAnnotationConvention<TableColumnAttribute, string>(
                    "ServiceTableColumn", (property, attributes) => attributes.Single().ColumnType.ToString()));
        }

        public string GenerateEntityId<TEntity>(Func<TEntity, bool> predicate) where TEntity : class, ISyncEntity
        {
            // if a match, then do not create
            var isAnyUserMatch = Set<TEntity>().Any(predicate);

            if (isAnyUserMatch)
                return null;

            string newId = null;
            while (newId == null)
            {
                var testedId = Guid.NewGuid().ToString();

                if (!Set<TEntity>().Any(p => p.Id.Equals(testedId)))
                {
                    newId = testedId;
                    break;
                }
            }
            return newId;
        }


        public async Task<int> SecureSaveChangesAsync()
        {
            try
            {
                return await this.SaveChangesAsync();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException ex)
            {
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var inError in error.ValidationErrors)
                    {
                        throw new Exception(inError.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException ex)
            {
                throw new DataTransactionExecutionException(ex.Entries, ex.Message, ex.InnerException);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.Message, ex.InnerException, ex.StackTrace);
                throw;
            }
        }

        public DbSet<UserProfileAuthorization> UserProfileAuthorizations { get; set; }
        public DbSet<UserProfileDevice> UserProfileDevices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Tag> Tags { get; set; }
    }

}
