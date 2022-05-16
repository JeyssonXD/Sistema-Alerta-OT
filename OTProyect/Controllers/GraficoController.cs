using OTProyect.Models;
using OTProyect.ViewModels.assistant;
using OTProyect.ViewModels.PuntosCalor;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
            try{

                //preparar datos por defecto
                if (Model.FechaInicial == null && Model.FechaFinal == null)
                {
                    //exactamente del dia de hoy contando desde ayer
                    Model.FechaInicial = new DateTime(DateTime.Now.AddDays(-1).Year, DateTime.Now.AddDays(-1).Month, DateTime.Now.AddDays(-1).Day, 0, 0, 0);
                    Model.FechaFinal = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
                }

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
                            //se obtiene la data de las variables de brillo 4
                            Model.Brillo4 = query.Select(x => x.bright_ti4).ToList();
                            var fechasAuxiliar = query.Select(x => x.fecha).ToList();
                            Model.LabelDataBrillo4 = fechasAuxiliar.Select(x => x.ToString()).ToList();
                            //se obtiene la data de las variables de brillo 5
                            Model.Brillo5 = query.Select(x => x.bright_ti5).ToList();
                            Model.LabelDataBrillo5 = Model.LabelDataBrillo4;//la misma que brillo 4
                            //obtener estadistico de confianza

                            var queryConfidenceEstadistico
                            = @"Select confidence,
		                            '' as color,
		                            ROUND((Count(*)* 100.0 / (Select Count(*) 
								                                From dbo.puntoscalor 
								                                where acq_date >= @paramFechaInicial and acq_date <= @paramFechaFinal)),2)::double precision as porcentaje
                            From dbo.puntoscalor
                            where acq_date >= @paramFechaInicial and acq_date <= @paramFechaFinal
                            Group By confidence";

                            var ConfidenceEstadistico = db.Database.SqlQuery<DataMapCofidencePuntosCalor>(
                                    queryConfidenceEstadistico,
                                    new Npgsql.NpgsqlParameter("paramFechaInicial", Model.FechaInicial),
                                    new Npgsql.NpgsqlParameter("paramFechaFinal", Model.FechaFinal)
                                ).ToList();

                            var Colors = new List<string>
                            {
                                "#5D62B4",
                                "#54C3BE",
                                "#EF726F",
                                "#F9C446",
                                "#21B7EC"
                            };

                            //agregar color randmon al estadistico
                            var i = 0;
                            foreach(var valorConfianza in ConfidenceEstadistico){
                                valorConfianza.color = Colors[i];
                                i++;
                            }

                            Model.DatosEstadisticosConfidence = ConfidenceEstadistico;

                        }
                    }
                }

                return View(Model);
            }
            catch(Exception e){
                //Mostrar exection no controlada
                return View("Message", new MessageResult
                {
                    Type = MessageResult.TypeMessage.Error,
                    Title = "Lo sentimos, ha ocurrido un error inesperado",
                    Message = e.Message
                });
            }
           
        }

        public ActionResult GraficoCobertura()
        {
            return View("GraficoCobertura");
        }
    }
}