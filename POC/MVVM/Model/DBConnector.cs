using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Data;
using System.Diagnostics;

namespace POC.MVVM.Model
{
    class DBConnector
    {
        private static string server = "localhost";
        private static string database = "retaildb";
        private static string uid = "root";
        private static string password = "";
        private static MySqlConnection connection = new MySqlConnection($"SERVER={server};DATABASE={database};UID={uid};PASSWORD={password};PORT=3306;");

        /// <summary>
        /// This method opens connection and must be used for each time to connect
        /// </summary>
        /// <returns>Status for the connection</returns>
        static bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                //When handling errors, you can your application's response based on the error number.
                //The two most common error numbers when connecting are as follows:
                //0: Cannot connect to server.
                //1045: Invalid user name and/or password.
                switch (ex.Number)
                {
                    case 0:
                        MessageBox.Show("Cannot connect to server. Contact Administrator");
                        break;

                    case 1045:
                        MessageBox.Show("Invalid DB Authentication, please try again");
                        break;
                }
                return false;
            }
        }

        /// <summary>
        /// This method must be used after each connection
        /// </summary>
        /// <returns></returns>
        static bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        internal static bool SendToDB(string InsertQuery)
        {
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(InsertQuery, connection);

                try
                {
                    //Execute command
                    int reader = cmd.ExecuteNonQuery();
                }
                catch
                {
                    CloseConnection();
                    return false;
                }

                //close connection
                CloseConnection();
                return true;
            }
            return false;
        }

        internal static DataTable GetFromDB(string SelectQuery)
        {
            DataTable dataTable = new DataTable();
            if (OpenConnection() == true)
            {
                //create command and assign the query and connection from the constructor
                MySqlCommand cmd = new MySqlCommand(SelectQuery, connection);

                try
                {
                    //Execute command
                    MySqlDataReader dataReader = cmd.ExecuteReader();
                    DataSet ds = new DataSet();
                    
                    ds.Tables.Add(dataTable);
                    ds.EnforceConstraints = false;
                    dataTable.Load(dataReader);
                    dataReader.Close();
                }
                catch(Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                    Debug.WriteLine(ex.StackTrace);
                    CloseConnection();
                    return null;
                }

                //close connection
                CloseConnection();
                return dataTable;
            }
            return null;
        }
    }
}
