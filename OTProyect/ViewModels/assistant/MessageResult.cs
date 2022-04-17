using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OTProyect.ViewModels.assistant
{
    /// <summary>
    /// Clase auxiliar que se utiliza para describir una respuesta sobre una acción
    /// </summary>
    public class MessageResult
    {
        /// <summary>
        /// Error
        /// Simplemente Informativo
        /// Informativo Exitoso
        /// Informativo como fallido
        /// </summary>
        public enum TypeMessage { Error, InformationHelper, InformationSuccess, InformationFail };

        /// <summary>
        /// Tipo de mensaje resultante
        /// </summary>
        public TypeMessage Type { get; set; }

        /// <summary>
        /// Cuerpo del mensaje
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Titulo del mensaje
        /// </summary>
        public string Title { get; set; }
    }
}