using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;

namespace Verdens_Maal_Skole
{
    public class ArduinoManager
    {
        /// <summary>
        /// Sends data to the database
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool SaveArduinoData(string data)
        {
            try
            {
                string[] dataArray = splitString(data);

                DataAccess.PostToDatabase(ConvertStringArrayToFloatArray(dataArray));
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Splits a string on every semicolon
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string[] splitString(string data)
        {
            try
            {
                //Spliting the string at ';'
                string[] tempValues = data.Split(";");

                return tempValues;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Returns a boolean based off of a given string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private bool ConvertStringToBoolean(string data)
        {
            if (data == "1")
            {
                return true;
            }
            else
                return false;
        }


        /// <summary>
        /// Returns a DateTime converted from a given string
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        private DateTime SplitStringToDateTime(string date)
        {
            DateTime time = DateTime.Parse(date);
            return time;
        }


        /// <summary>
        /// Returns a float array converted from a given string array
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private float[] ConvertStringArrayToFloatArray(string[] data)
        {
            var ci = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.NumberDecimalSeparator = ".";
            float[] temp = new float[3];
            for (int i = 0; i < data.Length; i++)
            {
                temp[i] = float.Parse(data[i], ci);
            }

            return temp;
        }


        /// <summary>
        /// Returns a List of ReaderData converted from a DB given Dataset
        /// </summary>
        /// <returns></returns>
        public List<ReaderData> GetAllReaders()
        {
            try
            {
                DataSet dataSet = DataAccess.GetAllData();
                int i = 0;
                List<ReaderData> dataList = new List<ReaderData>();

                for (i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
                {
                    dataList.Add(new ReaderData(dataSet.Tables[0].Rows[i][1].ToString(),
                        SplitStringToDateTime(dataSet.Tables[0].Rows[i][5].ToString()),
                        new Temperature(float.Parse(dataSet.Tables[0].Rows[i][8].ToString())),
                        new Humidity(float.Parse(dataSet.Tables[0].Rows[i][10].ToString())),
                        new Light(Int32.Parse(dataSet.Tables[0].Rows[i][12].ToString()),
                        ConvertStringToBoolean(dataSet.Tables[0].Rows[i][13].ToString()))));
                }

                return dataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        /// <summary>
        /// Returns a List of strings containing Room numbers
        /// </summary>
        /// <returns></returns>
        public List<string> GetAllRoomNumbers()
        {
            return DataAccess.GetAllRoomNumbers();
        }


        /// <summary>
        /// Returns a list of ReaderData based on a given Room number
        /// </summary>
        /// <param name="roomNr"></param>
        /// <returns></returns>
        public List<ReaderData> GetReaderDataByRoomNr(string roomNr)
        {
            try
            {
                DataSet dataSet = DataAccess.GetDataByRoomNr(roomNr);
                int i = 0;
                List<ReaderData> dataList = new List<ReaderData>();

                for (i = 0; i <= dataSet.Tables[0].Rows.Count - 1; i++)
                {
                    dataList.Add(new ReaderData(roomNr,
                   SplitStringToDateTime(dataSet.Tables[0].Rows[i][5].ToString()),
                   new Temperature(float.Parse(dataSet.Tables[0].Rows[i][9].ToString())),
                   new Humidity(float.Parse(dataSet.Tables[0].Rows[i][7].ToString())),
                   new Light(Int32.Parse(dataSet.Tables[0].Rows[i][11].ToString()),
                   ConvertStringToBoolean(dataSet.Tables[0].Rows[i][12].ToString()))));
                }

                return dataList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

    }
}
