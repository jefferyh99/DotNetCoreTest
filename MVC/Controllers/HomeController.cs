﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            _logger.LogTrace("HomeController _logger.LogTrace");
            _logger.LogDebug("HomeController _logger.LogDebug");
            _logger.LogInformation("HomeController _logger.LogInformation");
            _logger.LogWarning("HomeController _logger.LogWarning");
            _logger.LogError("HomeController _logger.LogError");
            _logger.LogCritical("HomeController _logger.LogCritical");


            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            throw new Exception("Contact Exception Test!!!");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
