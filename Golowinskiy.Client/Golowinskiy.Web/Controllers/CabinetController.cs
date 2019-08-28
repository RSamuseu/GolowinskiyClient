using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Golowinskiy.Web.Entities;
using Golowinskiy.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Golowinskiy.Web.Controllers
{
    [Authorize]
    public class CabinetController : Controller
    {
        [HttpGet]
        public IActionResult Cabinet()
        {
            bool isAuthenticate = (HttpContext.User != null)
                           && HttpContext.User.Identity.IsAuthenticated;

            if (isAuthenticate)
                return View();
            else
                return Redirect("~/Cabinet/Cabinet");
        }

        [HttpGet]
        public IActionResult Header()
        {
            var model = new CabinetViewModel();
            model.UserName = HttpContext.User.Identity.Name;
            return PartialView(model);
        }
    }
}