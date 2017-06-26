using DigitalX.ServiceReference1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DigitalX.Controllers
{
    public class CartController : Controller
    {
        ServiceReference1.Service1Client service = new ServiceReference1.Service1Client();

        // GET: Cart
        public ActionResult Index()
        {
            return View();
        }
    }
}