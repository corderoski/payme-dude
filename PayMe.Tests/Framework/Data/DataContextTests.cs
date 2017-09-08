using Autofac;
using PayMe.Framework.Data.Context;
using System.Linq;
using Xunit;

namespace PayMe.Tests.Framework.Data
{
    public class DataContextTests
    {
        private readonly IDataContext _dataContext;

        public DataContextTests()
        {
            _dataContext = CompositionRoot.Container.Resolve<IDataContext>();
        }

        [Fact]
        public void InitializationNotNull_Success()
        {
            Assert.NotNull(_dataContext.Database.Connection.ConnectionString);
            Assert.NotNull(_dataContext.Users);

            var queryResult = _dataContext.Database.SqlQuery<int>("SELECT 1 * 1");
            var result = queryResult.SingleAsync().Result;
            Assert.True(result > 0);
        }

        [Fact]
        public void DbSetUsers_GetItems_Success()
        {
            Assert.NotNull(_dataContext.Users.ToList());
            Assert.NotNull(_dataContext.UserProfileAuthorizations.ToList());
            Assert.NotNull(_dataContext.UserProfileDevices.ToList());
        }

    }
}
