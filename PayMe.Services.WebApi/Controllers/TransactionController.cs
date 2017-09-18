using Microsoft.Azure.Mobile.Server;
using Microsoft.Owin.Logging;
using PayMe.Framework.Data.Context;
using PayMe.Framework.Data.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.OData;

namespace PayMe.Services.WebApi.Controllers
{


    public class TransactionController : TableController<Transaction>
    {

        private readonly ILogger _logger;
        private readonly IDataContext _dataContext;


        public TransactionController(IDataContext dataContext, ILogger logger)
        {
            _logger = logger;
            _dataContext = dataContext;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            
            var dbContext = _dataContext as System.Data.Entity.DbContext;
            DomainManager = new EntityDomainManager<Transaction>(dbContext, Request);
        }

        public IQueryable<Transaction> Get()
        {
            var userId = this.GetUserId();
            return Query().Where(p => p.UserId == userId);
        }

        public SingleResult<Transaction> GetById(string id)
        {
            return Lookup(id);
        }

        public Task<Transaction> Path(string id, Delta<Transaction> patch)
        {
            return UpdateAsync(id, patch);
        }

        public async Task<IHttpActionResult> Post(Transaction item)
        {
            var current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        public Task Delete(string id)
        {
            return DeleteAsync(id);
        }
    }
}
