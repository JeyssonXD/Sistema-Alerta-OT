using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OTProyect.Models;
using OTProyect.ViewModels.assistant;
using OTProyect.ViewModels.PuntosCalor;
using PagedList;

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
        public ActionResult Reporte(string sortOrder, int? page, DataPagePuntosCalor search, DateTime? CurrentFechaInicial, DateTime? CurrentFechaFinal, string CurrentSatellite, string Currentinstrument)
        {
            try
            {
                //deaclaramos valores iniciales para ordenacion
                ViewBag.CurrentSort = sortOrder;
                ViewBag.Param1SortParm = String.IsNullOrEmpty(sortOrder) ? "Fecha_desc" : "";

                //mapear los datos sobre los filtro actuales y mantener
                #region Map CurrentFilter
                var currentFilter = new DataPagePuntosCalor
                {
                    instrument = Currentinstrument,
                    satellite = CurrentSatellite,
                    FechaInicial = CurrentFechaInicial,
                    FechaFinal = CurrentFechaFinal
                };
                #endregion

                //verificar pagina inicial o filtro
                if (search.isValid() && !currentFilter.isValid())
                {
                    page = 1;
                }
                else
                {
                    search = currentFilter;
                }

                ViewBag.currentFilter = search;

                using (var db = new ApplicationDbContext())
                {
                    //Predicado
                    var query = db.PuntoCalor.Where(x => x.fecha != null);

                    //busqueda de los datos
                    if (ModelState.IsValid && search.isValid())
                    {

                        //Buscar por rango de fecha
                        if (search.FechaInicial != null && search.FechaFinal != null)
                        {
                            var auxFechaFinal = new DateTime(search.FechaFinal.Value.Year, search.FechaFinal.Value.Month, search.FechaFinal.Value.Day, 23, 59, 59);
                            query = query.Where(x =>
                                x.fecha >= search.FechaInicial
                                &&
                                x.fecha <= auxFechaFinal
                            );
                        }

                        //Buscar por satelite
                        if (search.satellite != null)
                        {
                            query = query.Where(x => x.satellite.Contains(search.satellite));
                        }

                        //Buscar por Instrumento
                        if (search.instrument != null)
                        {
                            query = query.Where(x => x.instrument.Contains(search.instrument));
                        }

                    }

                    ViewBag.CurrentSort = sortOrder;

                    switch (sortOrder)
                    {
                        case "Fecha_desc":
                            query = query.OrderByDescending(s => s.fecha);
                            break;
                        default:
                            query = query.OrderBy(s => s.fecha);
                            break;
                    }

                    //page
                    int pageSize = 10;
                    int pageNumber = (page ?? 1);

                    //View Page
                    var ModelView = new DataPagePuntosCalor
                    {
                        Lista = query.ToPagedList(pageNumber, pageSize)
                    };

                    if (search != null)
                    {
                        if (search.instrument != null) { ModelView.instrument = search.instrument; }
                        if (search.satellite != null) { ModelView.satellite = search.satellite; }
                        if (search.FechaInicial != null) { ModelView.FechaInicial = search.FechaInicial; }
                        if (search.FechaFinal != null) { ModelView.FechaFinal = search.FechaFinal; }
                    }

                    //count
                    var Count = query.Count();
                    ViewBag.Count = Count;

                    return View("ReportePuntosCalor",ModelView);
                }
            }
            catch (Exception e)
            {
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
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