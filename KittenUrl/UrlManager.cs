using DTO;
using KittenUrl.Interfaces;
using KittenUrl.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KittenUrl
{
    public class UrlManager : IUrlManager //this should probably be static??  Do we want it instantiated??
    {
        public UrlManager()
        {

        }

        public string GetUrlFromTextInput(HttpContext context)
        {
            // Perform basic form validation
            if (!context.Request.HasFormContentType || !context.Request.Form.ContainsKey("url"))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync("Cannot process request").ToString();
            }

            context.Request.Form.TryGetValue("url", out var formData);
            var requestedUrl = formData.ToString();

            // Test our URL
            if (!Uri.TryCreate(requestedUrl, UriKind.Absolute, out Uri result))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return context.Response.WriteAsync("Could not understand URL.").ToString();
            }

            var urlInput = result.Scheme + "://" + result.Host + result.PathAndQuery;

            return urlInput;
        }

        public string FindUrl(string url, HttpContext context)
        {
            var path = context.Request.Path;
            return path;         
        }

        public string ReturnThisDomain(string url, HttpContext context)
        {
            return context.Request.Scheme + "://" + context.Request.Host.Value;
        }
    }
}
