using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Midnight_Calculator.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Midnight_Calculator.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index([FromForm] DateTime maghrib, [FromForm] DateTime fajr)
        {

            int maghHourInMinutes = maghrib.Hour * 60 + maghrib.Minute; //20:30= 1230
            int fajrHourInMinutes = fajr.Hour * 60 + fajr.Minute; //05:16 = 316

            int difference = 0;

            if (maghHourInMinutes > fajrHourInMinutes)
            {
                int offsetted_fajr_time = 1440 + fajrHourInMinutes;
                difference = offsetted_fajr_time - maghHourInMinutes; // 1620 (3:00) - 1260 (21:00) = 360 (6h)
            }
            else
            {
                difference = fajrHourInMinutes - maghHourInMinutes;
            }

            int differenceHalf = difference / 2; // 360m /2 = 180m (3h)
            int midPointInMinutes = maghHourInMinutes + differenceHalf; // 1260 + 183 = 1443

            string midnight = "";
            int midpointHour = (midPointInMinutes % 1440) / 60;
            if (midpointHour < 10)
                midnight += "0";
            midnight += midpointHour + ":";

            int midpointMin = (midPointInMinutes % 1440) % 60;
            if (midpointMin < 10)
                midnight += "0";
            midnight += midpointMin;

            ViewData["Midnight"] = midnight;
            ViewData["maghrib"] = maghrib;
            ViewData["fajr"] = fajr;

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
