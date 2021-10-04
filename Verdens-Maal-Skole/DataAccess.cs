using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Verdens_Maal_Skole
{
    public class DataAccess
    {
        static string connectionString = @"Server = (localdb)\MSSQLLOCALDB; Database = ArduinoDB; Trusted_Connection=True;"; //MAKE SURE IT'S YOUR OWN CONNECTIONSTRING
        ArduinoManager ArduinoManager = new ArduinoManager();

        public void PostToDatabase(float[] array)
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

        public List<ReaderData> GetAllReaders()
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
                    ArduinoManager.SplitStringToDateTime(ds.Tables[0].Rows[i][8].ToString()),
                    new Temperature(float.Parse(ds.Tables[0].Rows[i][10].ToString())),
                    new Humidity(float.Parse(ds.Tables[0].Rows[i][7].ToString())),
                    new Light(Int32.Parse(ds.Tables[0].Rows[i][13].ToString()),
                    ArduinoManager.ConvertStringToBoolean(ds.Tables[0].Rows[i][14].ToString()))));
            }

            connection.Close();

            return dataList;
        }


        public List<ReaderData> GetDataFromRoom(string roomNr)
        {
            try
            {
                List<ReaderData> dataList = new List<ReaderData>();
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
                    dataList.Add(new ReaderData(roomNr,
                   ArduinoManager.SplitStringToDateTime(ds.Tables[0].Rows[i][7].ToString()),
                   new Temperature(float.Parse(ds.Tables[0].Rows[i][9].ToString())),
                   new Humidity(float.Parse(ds.Tables[0].Rows[i][6].ToString())),
                   new Light(Int32.Parse(ds.Tables[0].Rows[i][12].ToString()),
                   ArduinoManager.ConvertStringToBoolean(ds.Tables[0].Rows[i][13].ToString())))
                        );
                }

                connection.Close();

                return dataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
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


    }
}
