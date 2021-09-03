using System;
using System.Collections.Generic;
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

        public async Task<string[]> GetArdDataAsync()
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

        public List<string> GetDataFromRoom(string roomNr)
        {
            List<string> dataList = new List<string>();

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var command = new SqlCommand())
                {
                    command.CommandText = $"EXEC [spGetReadingByRoomNr] {roomNr}";

                    var reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        foreach(var row in reader)
                        {
                            dataList.Add(row.ToString());
                        }
                    }
                }
                connection.Close();
            }
            return dataList;
        }


        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try
            {
                //Checks if the task is done before starting new
                if (isDone == true)
                {
                    isDone = false;

                    Task<string[]> task = GetArdDataAsync();

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

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = @"EXEC spInsertSensorData @roomNr, @temperature, @humidity, @light";
                    
                    cmd.Parameters.AddWithValue(@"@roomNr", "B.16");
                    cmd.Parameters.AddWithValue(@"@temperature", array[0]);
                    cmd.Parameters.AddWithValue(@"humidity", array[1]);
                    cmd.Parameters.AddWithValue(@"light", array[2]);

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Data has been posted to Database");
                }

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
