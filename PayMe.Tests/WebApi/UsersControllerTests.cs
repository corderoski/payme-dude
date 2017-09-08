using Autofac;
using Microsoft.Owin.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PayMe.Framework.Data.Context;
using PayMe.Framework.Data.DTO;
using PayMe.Framework.Services;
using PayMe.Services.WebApi.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;
using PayMe.Tests.Helpers;
using System.Net.Http;
using System.Diagnostics;
using System.Web.Http.Results;

namespace PayMe.Tests.WebApi
{
    public class UsersControllerTests
    {

        private readonly UsersController _usersControllers;

        public UsersControllerTests()
        {
            var container = CompositionRoot.Container;
            var authManagerService = container.Resolve<IAuthManagerService>();
            var logger = container.Resolve<ILogger>();
            var dataContext = container.Resolve<IDataContext>();

            _usersControllers = new UsersController(logger, dataContext, authManagerService);
            _usersControllers.Request = new HttpRequestMessage(HttpMethod.Post, "/api/users");
        }

        [Fact]
        public async void ValidateAccountAsync_InvalidRegistration_Fail()
        {
            using (var transactionScope = new TransactionScope())
            {
                var checksum = string.Empty;
                var result = await _usersControllers.ValidateAccountAsync(checksum);

                Assert.NotNull(result);
                Assert.IsType(typeof(BadRequestErrorMessageResult), result);
            }
        }

        [Fact]
        public async void CreateUserAsync_InvalidProviderAndEmail_Fail()
        {
            using (var transactionScope = new TransactionScope())
            {
                var model = new RegisterAuth { };
                
                var result = await _usersControllers.CreateUserAsync(model.ToJToken());

                Assert.NotNull(result);
                Assert.IsType(typeof(BadRequestErrorMessageResult), result);
            }
        }

        [Fact]
        public async void CreateUserAsync_ValidCreation_Success()
        {
            using (var transactionScope = new TransactionScope())
            {
                var model = GetRegisterAuth_Valid();

                var result = await _usersControllers.CreateUserAsync(model.ToJToken());

                Assert.NotNull(result);
                Assert.IsType(typeof(OkResult), result);
            }
        }

        private RegisterAuth GetRegisterAuth_Valid()
        {
            return new RegisterAuth
            {
                DeviceId = Guid.NewGuid().ToString(),
                Email = "string",
                Provider = "Provider",
                ProviderUserId = "ProviderUserId",
            };
        }

    }
}
