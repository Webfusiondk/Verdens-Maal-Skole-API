using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Verdens_Maal_Skole
{
    public class Program
    {
        static ArduinoManager manager = new ArduinoManager();
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
