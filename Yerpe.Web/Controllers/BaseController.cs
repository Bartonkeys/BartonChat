using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yerpe.Data;

namespace Yerpe.Web.Controllers
{
    public class BaseController : Controller
    {
        protected Yerpe_dbEntities YerpeContext = new Yerpe_dbEntities();
    }
}