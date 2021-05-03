using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPatients.Controllers
{
    public class RemoteController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult dobValidation(DateTime? DateOfBirth)
        {
            if (DateOfBirth <= DateTime.Now)
                return Json(true);
            else
                return Json("Date of Birth cannot be in the future");
        }

        public JsonResult dodValidation(DateTime? DateOfDeath)
        {
            if (DateOfDeath <= DateTime.Now)
                return Json(true);
            else
                return Json("Date of Death cannot be in the future");
        }


    }
}
