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
        static string connectionString = @"Server = ZBC-EMA-UDL2310; Database = ArduinoDB; Trusted_Connection=True;"; //MAKE SURE IT'S YOUR OWN CONNECTIONSTRING
        static bool isDone = true;
        private static Timer aTimer;
        static readonly HttpClient _client = new HttpClient();

        public List<ReaderData> GetAllReades()
        {
            List<ReaderData> dataList = new List<ReaderData>();
            SqlConnection connection;
            SqlDataAdapter adapter;
            SqlCommand command = new SqlCommand();
            DataSet ds = new DataSet();

            int i = 0;

            connection = new SqlConnection(connectionString);

            connection.Open();
            command.Connection = connection;
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "spGetAll";


            adapter = new SqlDataAdapter(command);
            adapter.Fill(ds);

            //We take the data tables and loop threw the rows to take out the data we need to create a ReaderDate Obj
            for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
            {
                dataList.Add(new ReaderData(ds.Tables[0].Rows[i][5].ToString(),
                    SplitStringToDateTime(ds.Tables[0].Rows[i][8].ToString()),
                    new Temperature(float.Parse(ds.Tables[0].Rows[i][10].ToString())),
                    new Humidity(float.Parse(ds.Tables[0].Rows[i][7].ToString())),
                    new Light(Int32.Parse(ds.Tables[0].Rows[i][13].ToString()),
                    ConvertStringToBoolean(ds.Tables[0].Rows[i][14].ToString()))));
            }

            connection.Close();

            return dataList;
        }
        private bool ConvertStringToBoolean(string data)
        {
            if (data == "1")
            {
                return true;
            }
            else
                return false;
        }
        private DateTime SplitStringToDateTime(string date)
        {
            DateTime time = DateTime.Parse(date);
            return time;
        }
        public async Task<string[]> GetArdDataAsync()
        {
            try
            {
                //Sending http request to website
                HttpResponseMessage respons = await _client.GetAsync(@"http://192.168.1.135/"); //MAKE SURE TO CHECK FOR UPDATED ARDUINO IP
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
                dataList.Add(ds.Tables[0].Rows[i][1].ToString());
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

        public List<string> GetRoomNumbers()
        {
            List<string> listOfRooms = new List<string>();
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM Room";
                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    while (reader.Read())
                    {
                        listOfRooms.Add(reader["RoomNr"].ToString());

                    }
                }

                connection.Close();
            }
            return listOfRooms;
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
                        PostToDatabase(ConvertStringArrayToFloatArray(task));
                    }

                    isDone = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
            }
        }
        private float[] ConvertStringArrayToFloatArray(Task<string[]> data)
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
