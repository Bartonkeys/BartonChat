using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yerpe.Web.Controllers
{
    public class YerpeController : Controller
    {
        // GET: Yerpe
        public ActionResult Talk()
        {
            return View();
        }
    }
}