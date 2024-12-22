using GuacosTracker3.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Diagnostics;

namespace GuacosTracker3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private string _title = "Home";
        private string _subtitle = "";

        [ViewData]
        public string Page
        {
            get
            {
                if (_subtitle != "")
                {
                    string title = string.Format("{0} - {1}", _title, _subtitle);
                    return title;
                }
                return _title;
            }

            set { _title = value; }
        }

        public string Subtitle
        {
            get { return _subtitle; }
            set { _subtitle = value; }
        }

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}