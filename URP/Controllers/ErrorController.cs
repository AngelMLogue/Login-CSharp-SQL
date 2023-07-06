using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace URP.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        [HttpGet]
        public ActionResult UnauthorizedOperation(String OP,String MO, String MsjErrorException)
        {
            ViewBag.OP = OP;
            ViewBag.MO = MO;
            ViewBag.MsjErrorException = MsjErrorException;
            return View();
        }
    }
}