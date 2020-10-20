using DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KittenUrl.ApiClient.KittenUrl
{
    public interface IKittenUrlClient
    {
        Task<string> Get(string url);

        Task<string> CreateUrl(string url);
    }
}
