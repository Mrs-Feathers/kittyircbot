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
		
		public string topic (int number, string lolstuffiscool, string fivetwothree) {
			string topic1 = ""; string CommandText;
			if ((lolstuffiscool == null) && (lolstuffiscool != "name")) CommandText = "SELECT * FROM globaltopics WHERE topicid = " + number + ";";
			else CommandText = "SELECT * FROM globaltopics WHERE topicid = " + Int32.Parse(lolstuffiscool) + ";";
			MySqlCommand query = new MySqlCommand(CommandText,connection); //connection.CreateCommand(); 
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					if (lolstuffiscool == null) topic1 += "12Topic " + number + " - ";
					topic1 += (string)result["topic"];
					if ((fivetwothree == "name")) topic1 = "topic " + lolstuffiscool + " was added by " + (string)result["nickname"] + ".";
					
				}
				result.Close();
                connection.Close();
				return topic1;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }

		public string cardlookup (int card1, int card2) {
			string cardreturn = "";
			string CommandText1 = "SELECT card FROM cards WHERE cardid = " + card1 + ";";
			string CommandText2 = "SELECT card FROM cards WHERE cardid = " + card2 + ";";
			MySqlCommand query1 = new MySqlCommand(CommandText1,connection);
            try {
				MySqlDataReader result1 = query1.ExecuteReader();
				while (result1.Read()) {
					cardreturn += (string)result1["card"] + " and ";
				}
				result1.Close();
				MySqlCommand query2 = new MySqlCommand(CommandText2,connection);
				MySqlDataReader result2 = query2.ExecuteReader();
				while (result2.Read()) {
					cardreturn += (string)result2["card"];
				}
				result2.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }
		
		public string literacy(int number, string lolstuffiscool) {
			string country = ""; string rate = ""; string literacyrate = "ERROR: unset variable"; string CommandText;
			if (lolstuffiscool == null) CommandText = "SELECT * FROM literacy WHERE id = " + number + ";";
			else CommandText = "SELECT * FROM literacy WHERE country = '" + lolstuffiscool + "';";
			MySqlCommand query = new MySqlCommand(CommandText,connection); //connection.CreateCommand(); 
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					country = (string)result["country"];
					rate = (string)result["rate"];	
				}
				literacyrate = country + "'s literacy rate is " + rate + ".";
				result.Close();
                connection.Close();
				return literacyrate;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }
		
		public string addtopic(string topic, string nick) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "INSERT INTO addedtopics (topic,nickname) VALUES ('" + topic + "', '" + nick + "');";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
				return "Success!";
            } catch {
            Console.WriteLine("ERROR: Unable to query database");
				return "error";
            }
        }
    }
}

