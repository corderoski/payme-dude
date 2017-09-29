using Microsoft.Azure.Mobile.Server;
using PayMe.Framework.Data.Context;
using PayMe.Framework.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace PayMe.Services.WebApi.Controllers
{
    public class TransactionTagController : TableController<TransactionTag>
    {

        private readonly IDataContext _dataContext;

        public TransactionTagController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var dbContext = _dataContext as System.Data.Entity.DbContext;
            DomainManager = new EntityDomainManager<TransactionTag>(dbContext, Request);
        }

        // GET tables/TransactionTag
        public IQueryable<TransactionTag> GetAllTag()
        {
            var userId = this.GetUserId();
            return Query();
        }

        // GET tables/TransactionTag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<TransactionTag> GetTag(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/TransactionTag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<TransactionTag> PatchTag(string id, Delta<TransactionTag> patch)
        {
            return UpdateAsync(id, patch);
        }

        // POST tables/TransactionTag
        public async Task<IHttpActionResult> PostTag(TransactionTag item)
        {
            TransactionTag current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/TransactionTag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTag(string id)
        {
            return DeleteAsync(id);
        }
    }
}
