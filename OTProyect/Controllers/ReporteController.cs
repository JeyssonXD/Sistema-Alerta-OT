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
    [Authorize]
    public class ReporteController : Controller
    {
        // GET: Reporte
        public ActionResult Reporte()
        {
            return View("Reporte");
        }
    }
}