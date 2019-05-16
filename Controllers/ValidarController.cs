using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
//
using CampIAFCJ.Models;


namespace CampIAFCJ.Controllers
{
    public class ValidarController : Controller
    {
        // GET: Validar
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ValidarFormulario vf)
        {
            return View();
        }
    }
}