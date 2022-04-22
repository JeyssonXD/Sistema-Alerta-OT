using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace OTProyect.Controllers
{

    /// <summary>
    /// Controlador especifico para Repostes
    /// </summary>
    [Authorize(Roles ="Consultor")]
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Reporte()
        {
            return View("Reporte");
        }

        public ActionResult ReporteCobertura()
        {
            return View("ReporteCobertura");
        }
    }
}