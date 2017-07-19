using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FingerPrint.DC
{
    public class DbAccess
    {
        public static MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //constructor
        public DbAccess()
        {
            Initialize();
        }

        //Initializing Values
        public void Initialize()
        {

            server = "localhost";
            database = "afis";
            uid = "root";
            password = "";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);

        }

        //open connection to database
        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //Cannot connect to server.  Contact administrator
                        break;
                    case 1045:
                        //Invalid username/password, please try again
                        break;
                }
                return false;
            }

        }

        //Close connection
        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException)
            {
                //  MessageBox.Show(ex.Message);
                return false;
            }
        }

        public MySqlCommand GetCommand(String query)
        {
            MySqlCommand cmd = new MySqlCommand(query, connection);
            return cmd;
        }


        public static void Main()
        {

        }


    }

   
}