using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using KittenUrl.Models;
using KittenUrl.Interfaces;
using Microsoft.AspNetCore.Http;
using KittenUrl.ApiClient.KittenUrl;
using DTO;
using Newtonsoft.Json;

namespace KittenUrl.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUrlManager _urlManager;
        private readonly IKittenUrlClient _urlClient;

        public HomeController(ILogger<HomeController> logger, IUrlManager urlManager, IKittenUrlClient urlClient)
        {
            _logger = logger;
            _urlManager = urlManager;
            _urlClient = urlClient;
        }

        [HttpGet]
        [Route("{url?}/{shorturl?}/{longurl?}")]
        public IActionResult Index(string url, string shorturl, string longurl)
        {    
            var model = new KittenUrlModel(); 
            
            if(url != null)
            {
                var urlPath = _urlManager.FindUrl(url, this.HttpContext); 
                
                var apiClientResult = _urlClient.Get(urlPath).Result;

                return Redirect(apiClientResult);   
            }
            else if(shorturl != null)
            {
                model.LongUrl = longurl;
                model.ShortUrl = shorturl;           
            }
            return View(model);
        }

        public IActionResult Shorten()
        {
            var url = _urlManager.GetUrlFromTextInput(this.HttpContext);
            var schemeAndDomain = _urlManager.ReturnThisDomain("",this.HttpContext);

            //this should just return the key
            var result = _urlClient.CreateUrl(url);

            var model = new KittenUrlModel()
            {
                ShortUrl = schemeAndDomain +  "/" + result.Result,
                LongUrl = url
            };

            return RedirectToAction("Index", "Home", new { url = "", shorturl = model.ShortUrl, longurl = url});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
