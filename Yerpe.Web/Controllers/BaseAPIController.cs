using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Http;
using Yerpe.Data;

namespace Yerpe.Web.Controllers
{
    public class BaseApiController: ApiController
    {
        protected Yerpe_dbEntities YerpeContext = new Yerpe_dbEntities();
    }
}
