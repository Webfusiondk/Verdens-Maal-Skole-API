using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Verdens_Maal_Skole
{
    public static class DataAccess
    {
        static string connectionString = GetConnectionString(); //Make sure the file has YOUR connectionstring
        private static List<string> AllRooms = GetAllRoomNumbers();
        static Random random = new Random();


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
                string roomnum = AllRooms[random.Next(AllRooms.Count)];
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = @"EXEC spInsertSensorData @roomNr, @temperature, @humidity, @light";

                        cmd.Parameters.AddWithValue(@"@roomNr", roomnum);
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

        /// <summary>
        /// Returns a token that matches given token string
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static DataSet SelectToken(string token)
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
                command.CommandText = "spSelectToken";

                param = new SqlParameter("@token", token);
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
        /// Runs a stored procedure that deletes outdated session Tokens
        /// </summary>
        public static void RemoveOldSessionTokens()
        {
            try
            {
                SqlConnection connection;
                SqlCommand command = new SqlCommand();

                connection = new SqlConnection(connectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spDeleteOldSessionTokens";

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Inserts a new session token into the database table
        /// </summary>
        /// <param name="token"></param>
        public static void PostToken(string token)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command = new SqlCommand();

                connection = new SqlConnection(connectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandText = "insert into SessionTokens(Token) VALUES(@token)";

                command.Parameters.AddWithValue(@"@token", token);

                command.ExecuteNonQuery();

                connection.Close();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }


        /// <summary>
        /// Updates a users session token in the database
        /// </summary>
        public static int UpdateSessionToken(string token)
        {
            RemoveOldSessionTokens();
            try
            {
                SqlConnection connection;
                SqlCommand command = new SqlCommand();
                SqlParameter param;

                connection = new SqlConnection(connectionString);

                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "spUpdateSessionToken";

                param = new SqlParameter("@token", token);
                param.Direction = ParameterDirection.Input;
                param.DbType = DbType.String;
                command.Parameters.Add(param);

                int resault = command.ExecuteNonQuery();

                connection.Close();
                return resault;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }


    }
}
