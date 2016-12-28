using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace FamilySearchAncestorMap.Controllers
{
    public class HomeController : Controller
    {
		private IOptions<AppKeys> _config;

		public HomeController(IOptions<AppKeys> config)
		{
			_config = config;
		}

		public IActionResult Index()
        {
			// If we do not have a OATH cookie, redirect

            return View(_config);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
