using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace mysqlIRCbot
{

    public class databaseMYSQL  {
		public MySqlConnection connection = new MySqlConnection();

        public databaseMYSQL (String hostname, int port, String username, String password, String database) {
            try {  
             connection.ConnectionString = "server=" + hostname + ";" +
            "database=" + database + ";" +
            "uid=" + username + ";" +
            "password=" + password + ";";
            connection.Open();
             }
             catch {
                Console.WriteLine("Unable to connect to MYSQL server");
             }
       
        }
		
		public string topic(int number) {
        //MySqlCommand query = connection.CreateCommand();
			//query.CommandText = "SELECT topic WHERE id = " + number + ";";  
            try {
                //MySqlDataReader result = query.ExecuteReader();
                //result.Close();
				//string topic1 = result.GetString(1);
                //connection.Close();
				string lulzytime = Convert.ToString(number);
				string topic1 = "the random number generated to pick the topic was: " + lulzytime;
				return topic1;
            }
            catch {
            Console.WriteLine("ERROR: Unable to query database");
			return "ERROR: Unable to query database";
            }
        }
    }
}

