using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTProyect.Controllers
{

    /// <summary>
    /// Controlador especifico para visores de mapa
    /// </summary>
    [Authorize(Roles ="Consultor")]
    public class VisorController : Controller
    {
        // GET: Visor
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VisorCobertura()
        {
            return View("VisorCobertura");
        }
    }
}