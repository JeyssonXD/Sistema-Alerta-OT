using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OTProyect.Models
{
    [Table("puntoscalor",Schema ="dbo")]
    public class PuntoCalor
    {
        [Key]
        public long Id { get; set; }

        [Display(Name = "País")]
        public string country_id { get; set; }

        [Display(Name = "Latitud")]
        public double latitude { get; set; }

        [Display(Name = "Longitud")]
        public double longitude { get; set; }

        [Display(Name = "Brillo 4")]
        public double bright_ti4 { get; set; }

        [Display(Name = "Trazo")]
        public double track { get; set; }

        [Display(Name = "Fecha")]
        public DateTime acq_date { get; set; }

        [Display(Name = "Tiempo")]
        public int acq_time { get; set; }

        [Display(Name = "Satelite")]
        public string satellite { get; set; }

        [Display(Name = "Instrumento")]
        public string instrument { get; set; }

        [Display(Name = "Confidencia")]
        public string confidence { get; set; }

        [Display(Name = "Version")]
        public string version { get; set; }

        [Display(Name = "Brillo 5")]
        public double bright_ti5 { get; set; }

        [Display(Name = "frp")]
        public double frp { get; set; }

        [Display(Name = "Día de noche")]
        public string daynight { get; set; }

        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }

        //[Column("geom")]
        //public Geometry Geom {get;set;}
    }
}