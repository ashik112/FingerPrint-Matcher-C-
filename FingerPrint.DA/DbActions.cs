using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FingerPrint.DC;
using FingerPrint.Info;
using MySql.Data.MySqlClient;
using MySql.Data;
using SourceAFIS.Simple;
using System.Data;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using SourceAFIS.Simple;
using System.Runtime.Serialization;
using FingerPrint.Info;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace FingerPrint.DA
{    
    public class DbActions 
    {

    
        public bool CreateUser(byte[] imageProfile,string name,  string gender, string dob, byte[] finger, MyPerson person)
        {

            //object yourMysteryObject = person;
            //MyPerson p = (MyPerson)person;
            //   BinaryFormatter formatter = new BinaryFormatter();
            // var ms = new MemoryStream();

            //   var ser = new BinaryFormatter();
            //ser.Serialize(ms, person);
            //ms.Position = 0;

            // return result;
            //            Stream stream = new Stream();
            // formatter.Serialize(stream, person);

            //MyPerson p = person;
            List<MyPerson> p = new List<MyPerson>();
            p.Add(person);
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            formatter.Serialize(ms, person);
            byte[] steam= ms.GetBuffer();
            // Console.WriteLine("Saving database...");
            // using (Stream stream = File.Open("database.dat", FileMode.Create))
            //      formatter.Serialize(stream, p);
            //MemoryStream stream1 = new MemoryStream();
            // DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(p));
            // ser.WriteObject(stream1, p);

            int status;
            DbAccess db = new DbAccess();

            //Open connection
            if (db.OpenConnection() == true)
            {
                Console.WriteLine(imageProfile);
                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = DbAccess.connection;
                cmd.CommandText = "INSERT INTO user(pic,name,gender,dob,finger,template) VALUES(?pic, ?name, ?gender, ?dob, ?finger, ?template)";
                cmd.Parameters.Add("?pic", MySqlDbType.Blob).Value = imageProfile;
                cmd.Parameters.Add("?name", MySqlDbType.VarChar).Value = name;
                cmd.Parameters.Add("?gender", MySqlDbType.VarChar).Value = gender;
                cmd.Parameters.Add("?dob", MySqlDbType.VarChar).Value = dob;
                cmd.Parameters.Add("?finger", MySqlDbType.Blob).Value = finger;
                cmd.Parameters.Add("?template", MySqlDbType.LongBlob).Value = steam;
                //cmd.Parameters.Add("?template", MySqlDbType.VarBinary).Value= memStream.GetBuffer();

                //  cmd.Parameters.Add("?template", MySqlDbType.VarBinary, Int32.MaxValue);

                //  cmd.Parameters["?template"].Value = memStream.GetBuffer();

                //Execute query
                status =cmd.ExecuteNonQuery();

                //close connection
                db.CloseConnection();
                if(status>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void Delete()
        {

        }

        public void Update()
        {

        }

        public List<Agent> AgentRead(string username, string password)
        {
            List<Agent> agent = new List<Agent>();
            DbAccess db = new DbAccess();

            string query = "SELECT * FROM agent where username='"+username+"'"+"AND password='"+password+"'";

            //Open connection
            if (db.OpenConnection() == true)
            {
                MySqlCommand cmd = db.GetCommand(query);
                MySqlDataReader dataReader = (MySqlDataReader)cmd.ExecuteReader();

                //Read the data and store them in the list
                int count = 0;
                while (dataReader.Read())
                {
                    Agent a = new Agent();
                    a.Id = dataReader["id"]+ "";
                    a.Username = dataReader["Username"] + "";
                    a.Password = dataReader["Password"] + "";
                    agent.Add(a);
                    
                    //  list[0].Add(dataReader["id"] + "");
                    // list[1].Add(dataReader["username"] + "");
                    // list[2].Add(dataReader["password"] + "");
                    // string id = dataReader["id"] + "";
                    count++;
                }

                //close Data Reader
                dataReader.Close();

                //close connection
                db.CloseConnection();
                return agent;
            }
            else
            {
                return agent;
            }
        }

        public List<User> GetPersons()
        {
            List<User> user = new List<User>();
            DbAccess db = new DbAccess();

            string query = "SELECT * FROM user";

            //Open connection
            if (db.OpenConnection() == true)
            {
                MySqlCommand cmd = db.GetCommand(query);
                MySqlDataReader dataReader = (MySqlDataReader)cmd.ExecuteReader();

                //Read the data and store them in the list
                int count = 0;
                while (dataReader.Read())
                {
                    User a = new User();
                    a.id = dataReader["id"] + "";
                    a.profile = (byte[])dataReader["pic"];
                    a.name = dataReader["name"] + "";
                    a.gender = dataReader["gender"] + "";
                    a.dob = dataReader["dob"] + "";
                    a.finger = (byte[])dataReader["finger"];
                    a.template = (byte[])dataReader["template"];
                    user.Add(a);

                    //  list[0].Add(dataReader["id"] + "");
                    // list[1].Add(dataReader["username"] + "");
                    // list[2].Add(dataReader["password"] + "");
                    // string id = dataReader["id"] + "";
                    count++;
                }

                //close Data Reader
                dataReader.Close();

                //close connection
                db.CloseConnection();
                return user;
            }
            else
            {
                return user;
            }
        }


        public User GetPerson(int id)
        {
            User user = new User();
            DbAccess db = new DbAccess();

            string query = "SELECT * FROM user where id="+id+"";

            //Open connection
            if (db.OpenConnection() == true)
            {
                MySqlCommand cmd = db.GetCommand(query);
                MySqlDataReader dataReader = (MySqlDataReader)cmd.ExecuteReader();

                //Read the data and store them in the list
                // int count = 0;
                /*  if(dataReader.FieldCount==1)
                  {

                  }*/
                 while (dataReader.Read())
                 {

                     user.id = dataReader["id"] + "";
                     user.profile = (byte[])dataReader["pic"];
                     user.name = dataReader["name"] + "";
                     user.gender = dataReader["gender"] + "";
                     user.dob = dataReader["dob"] + "";
                     user.finger = (byte[])dataReader["finger"];
                     //user.template = (byte[])dataReader["template"];

                 }

                //close Data Reader
                dataReader.Close();

                //close connection
                db.CloseConnection();
                return user;
            }
            else
            {
                return user;
            }
        }





        public static void Main()
        {

        }
    }
}

/*
 using System;
using MySql.Data.MySqlClient;

namespace FingerPrint.DC
{
   public class DatabaseConnection
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //constructor
        public DatabaseConnection()
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
        private bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                switch(ex.Number)
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
        private bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException )
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

    }
}
*/