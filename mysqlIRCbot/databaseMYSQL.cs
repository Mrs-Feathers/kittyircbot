using System;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace mysqlIRCbot
{

    public class databaseMYSQL  {
		public MySqlConnection connection = new MySqlConnection();
   
        /* Establish connection to MYSQL server */
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
        /* Add new user to MYSQL database & disconnect */
        public void adduser(String user) {
        MySqlCommand query = connection.CreateCommand();
        query.CommandText = "INSERT INTO irc (user) VALUES ('" + user + "')";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
            }
            catch {
            Console.WriteLine("ERROR: Unable to query database");
            }
        }
       
        /* Remove user from MYSQL database & disconnect */
        public void removeuser(String user) {
        MySqlCommand query = connection.CreateCommand();
        query.CommandText = "DELETE from irc where user = '"+ user + "';";
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
            }
            catch {
            Console.WriteLine("ERROR: Unable to query database");
            }
        }
    }
}

