using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTProyect.Models;



namespace OTProyect.Controllers
{

    /// <summary>
    /// Controlador especifico para Repostes
    /// </summary>
    [Authorize(Roles ="Consultor")]
    public class ReporteController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();

        // GET: Reporte
        public ActionResult Reporte()
        {
            ViewBag.PuntosCalor = db.PuntosCalorModel.ToList();
            ViewBag.VariblePrueba = "fdf";

            //ViewBag.PRueba
            //ViewData

            return View("Reporte");
        }

        public ActionResult ReporteCobertura()
        {
            return View("ReporteCobertura");
        }

        public ActionResult Index()
        {
            return View(db.PuntosCalorModel.ToList());
        }
    }
}