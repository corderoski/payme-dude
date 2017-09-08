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
    public class TagController : TableController<Tag>
    {

        private readonly IDataContext _dataContext;

        public TagController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var dbContext = _dataContext as System.Data.Entity.DbContext;
            DomainManager = new EntityDomainManager<Tag>(dbContext, Request);
        }

        // GET tables/Tag
        public IQueryable<Tag> GetAllTag()
        {
            return Query(); 
        }

        // GET tables/Tag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public SingleResult<Tag> GetTag(string id)
        {
            return Lookup(id);
        }

        // PATCH tables/Tag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task<Tag> PatchTag(string id, Delta<Tag> patch)
        {
             return UpdateAsync(id, patch);
        }

        // POST tables/Tag
        public async Task<IHttpActionResult> PostTag(Tag item)
        {
            Tag current = await InsertAsync(item);
            return CreatedAtRoute("Tables", new { id = current.Id }, current);
        }

        // DELETE tables/Tag/48D68C86-6EA6-4C25-AA33-223FC9A27959
        public Task DeleteTag(string id)
        {
             return DeleteAsync(id);
        }
    }
}
