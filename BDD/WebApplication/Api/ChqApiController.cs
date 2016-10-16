#region

using Data;
using Domain;
using System.Web.Http;

#endregion

namespace Web.Api
{
    public abstract class ChqApiController : ApiController
    {
       
        public IDataContext DataContext { get; set; }
        public ISystemUser SystemUser { get; set; }
    }
}
