using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTProyect.Controllers
{
    /// <summary>
    /// Controlador especifico para graficos
    /// </summary>
    [Authorize(Roles ="Consultor")]
    public class GraficoController : Controller
    {
        // GET: Grafico
        public ActionResult Grafico()
        {
            return View("Grafico");
        }
    }
}