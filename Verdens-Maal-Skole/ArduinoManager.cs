using System;
using System.Collections.Generic;
using System.Data;
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
        static bool isDone = true;
        private static Timer aTimer;
        static readonly HttpClient _client = new HttpClient();


        public bool ConvertStringToBoolean(string data)
        {
            if (data == "1")
            {
                return true;
            }
            else
                return false;
        }


        public DateTime SplitStringToDateTime(string date)
        {
            DateTime time = DateTime.Parse(date);
            return time;
        }


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
                        DataAccess.PostToDatabase(ConvertStringArrayToFloatArray(task));
                    }

                    isDone = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }


        public float[] ConvertStringArrayToFloatArray(Task<string[]> data)
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


        public List<ReaderData> GetAllReaders()
        {
             return DataAccess.GetAllReaders();
        }


        public List<string> GetRoomNumbers()
        {
            return DataAccess.GetRoomNumbers();
        }


        public List<ReaderData> GetDataFromRoom(string roomNr)
        {
            return DataAccess.GetDataFromRoom(roomNr);
        }

    }
}
