using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Verdens_Maal_Skole
{
    public static class DataAccess
    {
        static string connectionString = GetConnectionString(); //Make sure the file has YOUR connectionstring


        private static string GetConnectionString()
        {
            FileAccess fileAccess = new FileAccess();
            return fileAccess.ReadConnectionString();
        }


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


        public static DataSet GetAllReaders()
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


        public static DataSet GetDataFromRoom(string roomNr)
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


        public static List<string> GetRoomNumbers()
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
