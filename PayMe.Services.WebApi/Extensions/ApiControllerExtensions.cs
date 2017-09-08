using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Tables;
using PayMe.Framework.Data.Context;
using System.Net.Http;

namespace PayMe.Services.WebApi.Controllers
{
    public static class ApiControllerExtensions
    {

        public static IDomainManager<TEntity> GetAzureDomainManager<TEntity>(IDataContext dataContext, HttpRequestMessage request) where TEntity: class, ITableData
        {
            var dbContext = dataContext as System.Data.Entity.DbContext;
            return new EntityDomainManager<TEntity>(dbContext, request);
        }

    }
}