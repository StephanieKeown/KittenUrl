using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Interfaces;
using DTO;
//using System.Web.Http;
//using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace KittenUrl.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KittenUrlController : ControllerBase
    {
        private readonly ILogger<KittenUrlController> _logger;
        private readonly IKittenUrlRepository _urlRepository;

        public KittenUrlController(ILogger<KittenUrlController> logger, IKittenUrlRepository urlRepository)
        {
            _logger = logger;
            _urlRepository = urlRepository;
        }

        [HttpGet("get/{url?}")]
        public string Get(string url)        
        {
            if(url == null) { }

            var result = _urlRepository.FindUrl(url);

            return result;
        }


        [HttpGet("createurl/{url?}")]//
        public string CreateUrl([FromQuery(Name = "url")] string url)
        {
            var result = _urlRepository.HandleShortenUrl(url);

            return result;
        }
    }
}
