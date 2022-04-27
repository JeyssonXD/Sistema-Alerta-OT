using OTProyect.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OTProyect.ViewModels.Security
{
    public class DataPageUser : IValidatableObject
    {
        #region filter

        public string UserName { get; set; }

        #endregion

        #region model list
        public PagedList.IPagedList<ApplicationUser> Lista { get; set; }

        #endregion

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();
            
            return errors;
        }

        public bool isValid()
        {
            var bandera = false;

            if (UserName != null)
            {
                bandera = true;
            }

            return bandera;
        }

    }
}