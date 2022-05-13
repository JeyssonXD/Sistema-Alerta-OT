using OTProyect.Models;
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
        public ActionResult Grafico(ViewModels.PuntosCalor.DataChardPuntosCalor Model)
        {
            using (var db = new ApplicationDbContext())
            {
                var query = db.PuntoCalor.Where(x => x.fecha != null);

                if (ModelState.IsValid && Model.isValid())
                {
                    //Buscar por rango de fecha
                    if (Model.FechaInicial != null && Model.FechaFinal != null)
                    {
                        var auxFechaFinal = new DateTime(Model.FechaFinal.Value.Year, Model.FechaFinal.Value.Month, Model.FechaFinal.Value.Day, 23, 59, 59);
                        query = query.Where(x =>
                            x.fecha >= Model.FechaInicial
                            &&
                            x.fecha <= auxFechaFinal
                        );
                        //se obtiene la data de las variables de brillo 4 y 5
                        Model.Data1 = query.Select(x => Convert.ToSingle(x.bright_ti4)).ToList();
                        Model.Data2 = query.Select(x => Convert.ToSingle(x.bright_ti5)).ToList();
                        Model.xData = query.Select(x => x.fecha.ToShortDateString()).ToList();

                        //fill title
                        Model.Titles.Add("Fecha");
                        Model.Titles.Add("Brillo 4");
                        Model.Titles.Add("Brillo 5");
                    }
                }
            }

            return View(Model);
        }

        public ActionResult GraficoCobertura()
        {
            return View("GraficoCobertura");
        }
    }
}