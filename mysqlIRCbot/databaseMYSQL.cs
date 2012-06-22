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

		public string cardlookup (int card) {
			string cardreturn = "";
			string CommandText = "SELECT card FROM cards WHERE cardid = " + card + ";";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					cardreturn = "12" + (string)result["card"] + "";
				}
				result.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }

		public string welcomelookup (string nick) {
			string welcome = "";
			string CommandText = "SELECT * FROM welcomemessages WHERE nick = '" + nick + "';";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					welcome = (string)result["message"];
				}
				result.Close();
                connection.Close();
				return welcome;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }

		public string welcomeadd(string nick, string message) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "INSERT INTO welcomemessages (nick,message) VALUES ('" + nick + "', '" + message + "');";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
				return "Success!";
            } catch {
            Console.WriteLine("ERROR: Unable to query database");
				return "error. don't use apostrophies.";
            }
        }

		public string welcomeupdate(string nick, string message) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "UPDATE welcomemessages SET message = '" + message + "' WHERE nick = '" + nick + "';";  
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

		public string desclookup (string nick) {
			string welcome = "";
			string CommandText = "SELECT * FROM descriptions WHERE nick = '" + nick + "';";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					welcome = (string)result["description"];
				}
				result.Close();
                connection.Close();
				return welcome;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
        }

		public string descadd(string nick, string message) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "INSERT INTO descriptions (nick,description) VALUES ('" + nick + "', '" + message + "');";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
				return "Success!";
            } catch {
            Console.WriteLine("ERROR: Unable to query database");
				return "error. don't use apostrophies.";
            }
        }

		public string descupdate(string nick, string message) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "UPDATE descriptions SET description = '" + message + "' WHERE nick = '" + nick + "';";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
				return "Success!";
            } catch {
            Console.WriteLine("ERROR: Unable to query database");
				return "error. don't use apostrophies.";
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

