using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Verdens_Maal_Skole
{
    public class Program
    {
        static ArduinoManager manager = new ArduinoManager();
        public static void Main(string[] args)
        {
            //manager.StartEventTimer();
            //foreach (string item in manager.GetDataFromRoom("B.16"))
            //{
            //    System.Console.WriteLine(item);
            //}
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
