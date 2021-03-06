﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Oragon.Spring.Extensions.DependencyInjection;
using System;

namespace Oragon.Spring.Core.AspNetCoreTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Program.Main({args})");
#endif

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureOragonSpring(options => { 
            options.MakeBridgeFor<IConfiguration>("Configuration"); 
        })
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });

        /*public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();*/
    }
}
