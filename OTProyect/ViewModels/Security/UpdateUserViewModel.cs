using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OTProyect.ViewModels.Security
{
    /// <summary>
    /// View Models que se utiliza para actualizar cuentas de usuarios
    /// </summary>
    public class UpdateUserViewModel : IValidatableObject
    {
        
        public string Id { get; set; }

        [Required]
        [Display(Name ="Nombre de Usuario")]
        public string UserName { get; set; }

        [Display(Name = "Roles")]
        [Required]
        public List<string> Roles { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var errores = new List<ValidationResult>();

            foreach (var item in Roles)
            {
                if (!AccountRoles.GetRolesList().Contains(item))
                {
                    errores.Add(new ValidationResult(String.Concat("El rol especificado no esta definido:", item)));
                    break;
                }
            }

            return errores;
        }
    }
}