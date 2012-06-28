using System;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;

namespace kittyIRCbot
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

		public string getmain (string nick) {
			string cardreturn = "";
			string CommandText = "SELECT * FROM nickmapping WHERE altnick = '" + nick + "';";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					cardreturn = (string)result["nick"];
				}
				result.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "";
            }
        }

		public string topten (string nick) {
			string cardreturn = "";
			string CommandText = "SELECT * FROM xppoints ORDER BY points DESC LIMIT 10;";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
			int x = 0;
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					x++;
					ircbot.write("PRIVMSG " + nick + " :" + x + ". " + (string)result["nick"] + ": " + System.Convert.ToInt32(result["points"]), ircbot.writer);
				}
				result.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "";
            }
        }

		public string showawards (string nick) {
			string cardreturn = "";
			string CommandText = "SELECT * FROM awards ORDER BY id ASC;";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					if ((string)result["winner"] != "") ircbot.write("PRIVMSG " + nick + " :" + System.Convert.ToInt32(result["id"]) + ". Award: '" + (string)result["name"] + "' was awarded to: " + (string)result["winner"] + " by: " + (string)result["creator"] + " for reason: " + (string)result["reason"], ircbot.writer);
				}
				result.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "";
            }
        }

		public string showawardsnotwon (string nick) {
			string cardreturn = "";
			string CommandText = "SELECT * FROM awards ORDER BY id ASC;";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					if ((string)result["winner"] == "") ircbot.write("PRIVMSG " + nick + " :" + System.Convert.ToInt32(result["id"]) + ". Award: '" + (string)result["name"] + "' was created by: " + (string)result["creator"], ircbot.writer);
				}
				result.Close();
                connection.Close();
				return cardreturn;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "";
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

		public string addecho(string nick, string echo, DateTime now) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "INSERT INTO echomessages (nick,echo,time) VALUES ('" + nick + "', '" + echo + "', '" + now.ToString("yyyy-MM-dd HH:mm:ss") + "');";  
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

		public string updatexp(string nick, double xp) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "UPDATE xppoints SET points = " + xp + " WHERE nick = '" + nick + "';";  
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

		public string addxp(string nick, double xp) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "INSERT INTO xppoints (nick,points) VALUES ('" + nick + "', " + xp + ");";  
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

		public double getxp (string nick) {
			double welcome = 0;
			string CommandText = "SELECT * FROM xppoints WHERE nick = '" + nick + "';";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					welcome = System.Convert.ToDouble(result["points"]);
				}
				result.Close();
                connection.Close();
				try {return welcome;} catch {Console.WriteLine("error with get getxp"); return 0;}
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return 0;
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

		public string levelonelinklookup (int setting) {
			string link = "";
			string CommandText = "SELECT * FROM settings WHERE id = " + setting + ";";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					link = (string)result["levelonelink"];
				}
				result.Close();
                connection.Close();
				return link;
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return "ERROR: " + e.ToString();
            }
		}

		public void init (int setting) {
			string CommandText = "SELECT * FROM settings WHERE id = " + setting + ";";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
				ircbot.username = (string)result["nick"];
				ircbot.nickpassword = (string)result["nickpassword"];
                ircbot.hostname = (string)result["hostname"];
                ircbot.description = (string)result["description"];
                ircbot.channel = (string)result["channel"];
                ircbot.port = System.Convert.ToInt32(result["port"]);
				string adminlist = (string)result["admins"];
				string[] admins = adminlist.Split(':');
				ircbot.adminsadd(admins);
				ircbot.topicnumber = System.Convert.ToInt32(result["topicnumber"]);
				}
				result.Close();
                connection.Close();
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
            }
		}

		public void topicchange (int topic) {
        MySqlCommand query = connection.CreateCommand();
			query.CommandText = "UPDATE settings SET topicnumber = " + topic + " WHERE id = " + ircbot.botsetting + ";";  
            try {
                MySqlDataReader result = query.ExecuteReader();
                result.Close();
                connection.Close();
            } catch {
            Console.WriteLine("ERROR: Unable to query database");
            }
        }

		public int gettopic () {
			int welcome = 0;
			string CommandText = "SELECT * FROM settings WHERE id = " + ircbot.botsetting + ";";
			MySqlCommand query = new MySqlCommand(CommandText,connection);
            try {
				MySqlDataReader result = query.ExecuteReader();
				while (result.Read()) {
					welcome = System.Convert.ToInt32(result["topicnumber"]);
				}
				result.Close();
                connection.Close();
				try {return welcome;} catch {Console.WriteLine("error with get getxp"); return 0;}
            }
            catch (MySqlException e) {
            Console.WriteLine("ERROR: " + e.ToString());
			return 0;
            }
        }
    }
}

