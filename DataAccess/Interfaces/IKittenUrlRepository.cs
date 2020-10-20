using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IKittenUrlRepository
    {
        string HandleShortenUrl(string url);

        string FindUrl(string url);  //should return a url type if thats possible.
    }
}
