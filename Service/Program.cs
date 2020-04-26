/// <summary>
/// Developer: ShyamSk
/// </summary>

namespace ShareLocation.Service
{
    using Engaze.Core.Web;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using System;


    public class Program
    {
        public static void Main(string[] args)
        {
            EngazeWebHost.Run<Startup>(args);
        }       
    }
}
