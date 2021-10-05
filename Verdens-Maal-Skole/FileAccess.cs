using System;
using System.Collections.Generic;
using System.IO;

namespace Verdens_Maal_Skole
{
    public static class FileAccess
    {
        //Path for local txt file containing the database connectionstring
        static string conStringPath = Environment.CurrentDirectory + "\\DatabaseConnection.txt";


        /// <summary>
        /// Returns connectionstring read from local txt file
        /// </summary>
        /// <returns></returns>
        public static string ReadConnectionString()
        {
            List<string> result = new List<string>();
            string line = string.Empty;

            try
            {
                using (StreamReader reader = new StreamReader(conStringPath))
                {
                    while ((line = reader.ReadLine()) != null)
                    {
                        result.Add(line);
                    }

                }
                return result[0];
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return string.Empty;

            }
        }

    }
}
