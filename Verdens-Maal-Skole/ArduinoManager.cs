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
        static string connectionString = @"Server = (localdb)\MSSQLLOCALDB; Database = ArduinoDB"; //MAKE SURE IT'S YOUR OWN CONNECTIONSTRING
        static bool isDone = true;
        private static Timer aTimer;
        static readonly HttpClient _client = new HttpClient();

        public async Task<string[]> GetDataAsync()
        {
            try
            {
                //Sending http request to website
                HttpResponseMessage respons = await _client.GetAsync(@"http://192.168.1.132/"); //MAKE SURE TO CHECK FOR UPDATED ARDUINO IP
                respons.EnsureSuccessStatusCode();
                //Reading the response from website
                string responsBody = await respons.Content.ReadAsStringAsync();

                //Writing the response and replacing all HTML to text "Removed"
                string regString = Regex.Replace(responsBody, "<[^>]*>", "Removed");
                //Spliting the string at ';'
                string[] tempValues = regString.Split(";");
                return tempValues;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
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

                    Task<string[]> task = GetDataAsync();

                    if (task.Result.Length == 3)
                    {
                        PostToDatabase(ConvertStringToFloat(task));
                    }

                    isDone = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
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


        private void PostToDatabase(float[] array)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = 
                    @"INSERT INTO Light([Read]) VALUES (@light);
                    INSERT INTO Temperature([Read]) VALUES (@Temperature);
                    INSERT INTO [Humidity]([Read]) VALUES (@humidity);";

                    command.Parameters.AddWithValue("@humidity", array[0]);
                    command.Parameters.AddWithValue("@Temperature", array[1]);
                    command.Parameters.AddWithValue("@light", array[2]);

                    command.ExecuteNonQuery();
                };

                connection.Close();
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
