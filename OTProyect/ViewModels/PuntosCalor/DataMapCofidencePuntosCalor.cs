using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTProyect.ViewModels.PuntosCalor
{
    /// <summary>
    /// Clase auxiliar que se utiliza para mapear datos correspondiente a una estadistica
    /// de la varible Confidence
    /// </summary>
    public class DataMapCofidencePuntosCalor
    {
        //Confianza
        public string confidence { get; set; }

        //color
        public string color { get; set; }

        //porcentaje
        public double porcentaje { get; set; }
    }
}