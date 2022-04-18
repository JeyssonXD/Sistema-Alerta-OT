using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTProyect.ViewModels.Security
{
    public static class AccountRoles
    {
        public static IEnumerable<SelectListItem> GetRolesEnumerable()
        {
            return new List<SelectListItem>
                {
                    new SelectListItem{
                        Text="Administrador",
                        Value="Administrador"
                    },
                    new SelectListItem{
                        Text="Consultor",
                        Value="Consultor"
                    }
                };
         }

        public static List<string> GetRolesList()
        {
            return new List<string> {
                "Administrador",
                "Consultor"
            };

        }
    }
}