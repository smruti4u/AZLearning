using Azwebapp.Models;
using Azwebapp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Azwebapp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IConfiguration configuration;

        private readonly IKeyVaultService kvService;
        public HomeController(ILogger<HomeController> logger, IConfiguration configuration, IKeyVaultService kvService)
        {
            this.configuration = configuration;
            this.kvService = kvService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var value = configuration["devconnectionstring"];
            var connectionString = await kvService.GetValue();
            ViewBag.text = connectionString;
            return View();
        }

        public IActionResult Privacy()
        {
            throw new Exception("Something Went Wrong");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
