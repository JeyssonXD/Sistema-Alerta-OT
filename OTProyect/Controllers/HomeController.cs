﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OTProyect.Controllers
{
    public class HomeController : Controller
    {
        //Pagina de inicio del aplicaativo
        public ActionResult Index()
        {
            return View();
        }

    }
}