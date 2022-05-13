using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OTProyect.ViewModels.PuntosCalor
{
    public class DataChardPuntosCalor : IValidatableObject
    {

        [Display(Name = "Fecha Inicial")]
        public DateTime? FechaInicial { get; set; }

        [Display(Name = "Fecha Final")]
        public DateTime? FechaFinal { get; set; }

        #region Chard

        public List<string> Titles { get; set; }

        public List<string> xData { get; set; }

        public List<float> Data1 { get; set; }

        public List<float> Data2 { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            if (FechaInicial != null && FechaFinal != null)
            {
                if (FechaInicial > FechaFinal)
                {
                    errors.Add(new ValidationResult("Fecha Final no puede ser menor a fecha inicial"));
                }

                if (FechaInicial != null && FechaFinal == null)
                {
                    errors.Add(new ValidationResult("Para realizar un filtro de fecha, debe seleccionar un rango espefico, favor seleccionar especificar Fecha Final"));
                }

                if (FechaInicial == null && FechaFinal != null)
                {
                    errors.Add(new ValidationResult("Para realizar un filtro de fecha, debe seleccionar un rango espefico, favor seleccionar especificar Fecha Inicial"));
                }
            }

            return errors;
        }
        #endregion

        /// <summary>
        /// Formulario de busqueda es valido si se cumplen las condiciones especificadas
        /// </summary>
        /// <returns></returns>
        public bool isValid()
        {
            var bandera = false;

            if (FechaInicial != null && FechaFinal != null)
            {
                bandera = true;
            }

            return bandera;
        }
    }
}