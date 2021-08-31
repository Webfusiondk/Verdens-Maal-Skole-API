using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;

namespace Verdens_Maal_Skole
{
    public class ArduinoManager
    {
        static string connectionString = @"(localdb)\MSSQLLOCALDB";
        static bool isDone = true;
        private static Timer aTimer;
        static readonly HttpClient client = new HttpClient();
        public async Task<string[]> GetCelciusAsync()
        {
            try
            {
                //Sending http request to website
                HttpResponseMessage respons = await client.GetAsync(@"http://192.168.1.114/");
                respons.EnsureSuccessStatusCode();
                //Reading the response from website
                string responsBody = await respons.Content.ReadAsStringAsync();

                //Writing the response and replacing all HTML to text "Removed"
                string regString = Regex.Replace(responsBody, "<[^>]*>", "Removed");
                //Spliting the streng at ;
                string[] tempValues = regString.Split(";");
                return tempValues;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                //Checks if the task is done before starting new
                if (isDone == true)
                {
                    isDone = false;
                    Task<string[]> task = GetCelciusAsync();
                    if (task.Result.Length == 3)
                    {
                        PostToDataBase(ConvertStringToFloat(task));
                    }
                    isDone = true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        private float[] ConvertStringToFloat(Task<string[]> data)
        {
            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            float[] temp = new float[3];
            for (int i = 0; i < data.Result.Length; i++)
            {
                temp[i] = float.Parse(data.Result[i], ci);
            }
            return temp;
        }
        private void PostToDataBase(float[] arrya)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    connection.Open();
                    command.CommandText = @"insert into Light([read]) values (@light);
                                    insert into Tempature([read]) values (@tempature);
                                    insert into Humidity([read]) values (@humididty);";
                    command.Parameters.AddWithValue("@humididty", arrya[1]);
                    command.Parameters.AddWithValue("@tempature", arrya[2]);
                    command.Parameters.AddWithValue("@light", arrya[3]);
                    command.ExecuteNonQuery();
                };
            }
        }
        public void StartEventTimer()
        {
            aTimer = new Timer();
            aTimer.Interval = 2000;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;

            // Have the timer fire repeated events (true is the default)
            aTimer.AutoReset = true;

            // Start the timer
            aTimer.Enabled = true;
        }
    }
}
