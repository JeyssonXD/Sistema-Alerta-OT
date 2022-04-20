using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTProyect.Controllers
{
    
    [Authorize(Roles = "Consultor")]
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Inicio()
        {
            return View("Inicio");
        }
    }
}