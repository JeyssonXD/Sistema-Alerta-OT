using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using OTProyect.Models;

namespace OTProyect.ViewModels.PuntosCalor
{
    public class DataPagePuntosCalor : IValidatableObject
    {
        #region filter

        [Display(Name = "Satelite")]
        public string satellite { get; set; }

        [Display(Name = "Instrumento")]
        public string instrument { get; set; }

        [Display(Name = "Fecha Inicial")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime? FechaFinal { get; set; }

        #endregion

        #region model list
        public PagedList.IPagedList<PuntoCalor> Lista { get; set; }

        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if(FechaInicial!=null && FechaFinal != null){
                if (FechaInicial > FechaFinal)
                {
                    errors.Add(new ValidationResult("Fecha Final no puede ser menor a fecha inicial"));
                }

                if(FechaInicial!=null && FechaFinal == null){
                    errors.Add(new ValidationResult("Para realizar un filtro de fecha, debe seleccionar un rango espefico, favor seleccionar especificar Fecha Final"));
                }

                if (FechaInicial == null && FechaFinal != null)
                {
                    errors.Add(new ValidationResult("Para realizar un filtro de fecha, debe seleccionar un rango espefico, favor seleccionar especificar Fecha Inicial"));
                }
            }




            return errors;
        }

        /// <summary>
        /// Formulario de busqueda es valido si se cumplen las condiciones especificadas
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            var bandera = false;

            if (FechaInicial != null && FechaFinal!=null)
            {
                bandera = true;
            }

            if (instrument != null)
            {
                bandera = true;
            }

            if (satellite != null)
            {
                bandera = true;
            }

            return bandera;
        }
    }
}