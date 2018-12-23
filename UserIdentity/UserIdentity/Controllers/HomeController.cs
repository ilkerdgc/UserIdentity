using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace UserIdentity.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }

        [Authorize] // action metodu dışarıya kapatmak için kullanılır. yani dışarıdan erişilemez olur
        public ActionResult About()
        {
            return View();
        }
    }
}