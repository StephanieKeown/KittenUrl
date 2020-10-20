using DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KittenUrl.Interfaces
{
    public interface IUrlManager
    {
        string GetUrlFromTextInput(HttpContext context);

        string FindUrl(string url, HttpContext context);

        string ReturnThisDomain(string url, HttpContext context);
    }
}
