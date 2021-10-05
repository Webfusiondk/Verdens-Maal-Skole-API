using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Verdens_Maal_Skole
{
    public static class DataAccess
    {
        static string connectionString = GetConnectionString(); //Make sure the file has YOUR connectionstring


        /// <summary>
        /// Returns a connectionstring
        /// </summary>
        /// <returns></returns>
        private static string GetConnectionString()
        {
            return FileAccess.ReadConnectionString();
        }


        /// <summary>
        /// Saves a float array into the DB
        /// </summary>
        /// <param name="array"></param>
        public static void PostToDatabase(float[] array)
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }


        /// <summary>
        /// Returns a DataSet of all DB data
        /// </summary>
        /// <returns></returns>
        public static DataSet GetAllData()
        {
            try
            {
                SqlConnection connection;
                SqlDataAdapter adapter;
                SqlCommand command = new SqlCommand();
                DataSet data = new DataSet();

                connection = new SqlConnection(connectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spGetAll";

                adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                connection.Close();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Returns a DataSet with data from a specific room
        /// </summary>
        /// <param name="roomNr"></param>
        /// <returns></returns>
        public static DataSet GetDataByRoomNr(string roomNr)
        {
            try
            {
                SqlConnection connection;
                SqlDataAdapter adapter;
                SqlCommand command = new SqlCommand();
                SqlParameter param;
                DataSet data = new DataSet();

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
                adapter.Fill(data);

                connection.Close();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Returns a list of strings containing all room numbers
        /// </summary>
        /// <returns></returns>
        public static List<string> GetAllRoomNumbers()
        {
            try
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


    }
}
