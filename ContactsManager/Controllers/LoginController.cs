using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace SMEasy.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}