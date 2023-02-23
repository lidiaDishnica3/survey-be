using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InternalSurvey.Api.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace InternalSurvey.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = CreateHostBuilder(args).Build();

            using (var scope = builder.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    await services.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
                    await services.GetRequiredService<SurveyAppContext>().Database.MigrateAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error on migration {ex.Message} {Environment.NewLine} {ex.StackTrace}");
                }
            }

            builder.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
              .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
