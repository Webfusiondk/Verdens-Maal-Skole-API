﻿using System;
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
        static string connectionString = @"Server = (localdb)\MSSQLLOCALDB; Database = ArduinoDB"; //MAKE SURE IT'S YOUR OWN CONNECTIONSTRING
        static bool isDone = true;
        private static Timer aTimer;
        static readonly HttpClient _client = new HttpClient();
        List<ReaderData> data = new List<ReaderData>() {
        new Light(800,DateTime.Now,"A23"),
        new Humidity(60,DateTime.Now,"A23"),
        new Light(600,DateTime.Now,"A23"),
        new Temperature(17,DateTime.Now.AddSeconds(20),"A23"),
        new Humidity(40,DateTime.Now.AddSeconds(20),"A23"),
        new Temperature(15,DateTime.Now.AddSeconds(20),"A23"),
        };

        public List<ReaderData> GetAllReades()
        {
            //Should call in database to get all data
            //Right now returning dummy data

            return data;
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

        public List<string> GetDataFromRoom(string roomNr)
        {

            List<string> dataList = new List<string>();
            SqlConnection connection;
            SqlDataAdapter adapter;
            SqlCommand command = new SqlCommand();
            SqlParameter param;
            DataSet ds = new DataSet();

            int i = 0;

            connection = new SqlConnection(connectionString);

            connection.Open();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "spGetReadingByRoomNr";

            param = new SqlParameter("@roomNr", roomNr);
            param.Direction = ParameterDirection.Input;
            param.DbType = DbType.String;
            command.Parameters.Add(param);

            adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);

            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                dataList.Add(ds.Tables[0].Rows[i][0].ToString());
            }

            connection.Close();

            return dataList;





            //SqlDataReader rd;
            //int strnr = 0;

            //List<string> dataList = new List<string>();

            //using (var con = new SqlConnection(connectionString))
            //{
            //    con.Open();
            //    using (var cmd = new SqlCommand())
            //    {
            //        cmd.Connection = con;
            //        cmd.CommandType = System.Data.CommandType.StoredProcedure;
            //        cmd.CommandText = $"[spGetReadingByRoomNr]";
            //        cmd.Parameters.AddWithValue("@roomNr", roomNr);
            //        rd = cmd.ExecuteReader();
            //        while (rd.Read())
            //        {
            //            strnr++;
            //            dataList.Add(rd.GetString(strnr));
            //        }
            //        rd.Close();
            //    }
            //    con.Close();
            //}
            //return dataList;
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
