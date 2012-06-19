using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;

namespace mysqlIRCbot
{  
    public class ircbot {
        public static String username, hostname, description, channel;
        public static String mysqlhostname, mysqlusername, mysqlpassword, database;
        public static int port, mysqlport;
		public static bool v = true;
        public static String[] admins;
		
		public static int topicnumber = 1;
         
        public static TcpClient socket;
        public static StreamReader reader;
        public static StreamWriter writer;
		
		public static string[] interpretData;
   
        static void Main() {           
            try {
            loadConfig();
            socket = new TcpClient(hostname, port);
            socket.ReceiveBufferSize = 1024;
            Console.WriteLine("Connected :3");
            NetworkStream stream = socket.GetStream();
            reader = new StreamReader(stream);
            writer = new StreamWriter(stream);
            write("USER " + username + " 3 * :" + description, writer);
            write("NICK " + username, writer);
            read(reader);
            reader.Close();
            writer.Close();
            stream.Close();
            }
            catch {
            Console.WriteLine("Failed to connect :C");
            }
        }

        static void read(StreamReader reader) {        
            try {
                while(true) {
                    interpret(reader.ReadLine());  
                }  
        }
            catch {
                Console.WriteLine("Unable to read from server :C");
            }
        }
 
        static void interpret(String data) {
        Console.WriteLine(data);
        interpretData = data.Split(' ');
            if(interpretData[0].Equals("PING")) onPing(interpretData[1]);
            else if(interpretData[1].Equals("JOIN")) onJoin(interpretData[0]);	
            else if(interpretData[1].Equals("PRIVMSG")) {
                if (interpretData[2].Equals(channel)) {
					if (interpretData.Length == 3) interpretData[4] = " ";
                    onPublicMessage(interpretData[3]);
                }
                else if (interpretData[2].Equals(username)) {
                    onPrivateMessage(interpretData[0], interpretData[3]);
                }
            }
            else if(interpretData[1].Equals("421")) write("JOIN " + channel, writer); //221

           /* else if (interpretData[1].Equals("366")) {
            Thread thread = new Thread(onNickList); //don'T need this.
            thread.Start();
            }  */
   		}
   
        static void loadConfig() {
            try {
                string[] lines = File.ReadAllLines("config.cfg");
                foreach (string line in lines) {
                string[] data = line.Split(':');
                if (data[0].Equals("username")) username = data[1];
                else if (data[0].Equals("hostname")) hostname = data[1];
                else if (data[0].Equals("mysqlusername")) mysqlusername = data[1];
                else if (data[0].Equals("database")) database = data[1];
                else if (data[0].Equals("mysqlpassword")) mysqlpassword = data[1];
                else if (data[0].Equals("mysqlhostname")) mysqlhostname = data[1];
                else if (data[0].Equals("description")) description = data[1];
                else if (data[0].Equals("channel")) channel = data[1];
                else if (data[0].Equals("port")) port = Int32.Parse(data[1]);
                else if (data[0].Equals("mysqlport")) mysqlport = Int32.Parse(data[1]);
				else if (data[0].Equals("admins")) admins = (data);
                }
            }
            catch {
            Console.WriteLine("Cannot read configuration file, are you sure it is there?");
            }
        }

        public static void write(string data, StreamWriter writer) {
            try {
            writer.WriteLine(data);
            Console.WriteLine(">>> " + data);
            writer.Flush();
            }
            catch {
            Console.WriteLine("Error!");
            }
        }

        static void onPing(string pong) {
            pong = "PONG " + pong;
            write(pong, writer);   
        }
		/* On join to channel.. give them +v if v = true; */
        static void onJoin(string data) {
            String[] working = data.Split('!');
            String user = working[0].Substring(1);
			if (!user.Equals(username) && (v == true)) {
            write("PRIVMSG " + channel + " : Welcome to " + channel + ", " + user, writer);
            }
        }

		static void onPublicMessage(string data) {
            data = data.Substring(1);
            if (data.Equals("!info")) write("PRIVMSG " + channel + " :kittyIRCbot v1.0 written by auroriumoxide katja.decuir@gmail.com", writer);
            else if (data.Equals("!time")) {
            DateTime time = DateTime.Now;
            String now = String.Format("{0:F}", time);
            write("PRIVMSG " + channel + " :" + now, writer);
            }
			else if (data.Equals("!help")) write("PRIVMSG " + channel + " :!help = this; !time = get the time; !info = get info; !v = turn +v on or off; !topic [number] name = get a random topic or show specific topic by adding a number. adding 'name' shows you who wrote it; !topic add [topic] = this will add a topic to a list we can approve; !gtfo = exit; !literacy [country] = tells you the literacy rate of a given country. tells you a random one if no country provided; !dice [number] = rolls a dice with [number] sides; !sha1 [string] = encrypts a string to a sha1 hash; !pi [number] = calculates pi to a given number; !poker = to play texas hold 'em poker;", writer);
			else if (data.Equals("!v")) { try {
					if (interpretData[4].Equals("on")) v = true; else if (interpretData[4].Equals("off")) v = false; else write("PRIVMSG " + channel + " : use !v with on or off only", writer); }
				catch { write("PRIVMSG " + channel + " :use !v with on or off only", writer); }
        	}
			else if (data.Equals("!gtfo")) {
    			Environment.ExitCode = 0;
     			Environment.Exit(0);
  			}
			else if (data.Equals("!topic")) try { write("PRIVMSG " + channel + " :" + lookuptopic(interpretData[4],interpretData[5]), writer); } catch { try { write("PRIVMSG " + channel + " :" + lookuptopic(interpretData[4],null), writer); } catch { write("PRIVMSG " + channel + " :" + lookuptopic(null,null), writer); } }
			else if (data.Equals("!literacy")) try { write("PRIVMSG " + channel + " :" + lookupliteracy(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :" + lookupliteracy(null), writer); }
			else if (data.Equals("!dice")) try { write("PRIVMSG " + channel + " :" + rolldice(Int32.Parse(interpretData[4])), writer); } catch { write("PRIVMSG " + channel + " :" + rolldice(0), writer); }
			else if (data.Equals("!sha1")) try { write("PRIVMSG " + channel + " :" + sha1stuff(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :Sha1 hashing failed.", writer); }
			else if (data.Equals("!pi")) try { CalculatePI myPI = new CalculatePI (Int32.Parse(interpretData[4])); } catch { write("PRIVMSG " + channel + " :3.14", writer); }
			else if (data.Equals("!poker")) try { pokerstuff(interpretData[4],interpretData[5]); } catch { try { pokerstuff(interpretData[4],null); } catch { pokerstuff(null,null); } }
		}
		
        static void onPrivateMessage(String user, String data) {
        String[] sender = user.Split('!');
        user = sender[0].Substring(1);
        write("PRIVMSG " + user + " :" + username + ": please use commands in " + channel, writer);
        }

		static void pokerstuff(string stuff, string otherstuff) {
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];

			switch (stuff)
			{
			case "end":
				write("PRIVMSG " + channel + " :This is the end of the round. type '!poker start' for another.", writer);
				//show points
				Poker.clear();
				write("PRIVMSG " + channel + " :I should show you your points here, but that hasn't been implemented yet.", writer);
				break;
			case "next":
				write("PRIVMSG " + channel + " :" + Poker.next(), writer);
				break;
			case "deal":
				Poker.deal();
				break;
			case "start":
				Poker.clear();
				write("PRIVMSG " + channel + " :please type '!poker join' to join", writer);
				Poker.started = true;
				break;
			case "join":
				Poker.join(nick);
				break;
			default:
				write("PRIVMSG " + channel + " :please type '!poker start' to start, '!poker join' to join, '!poker deal' to deal, '!poker next' for the next card, and '!poker end' to end the game", writer);
				break;			
			}
        }
        
        static string lookuptopic(string stuff, string otherstuff) {
			int randomnumber = new Random().Next(1000); int nummber = topicnumber; string topicstring = "";
			if (stuff == "add") {
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
				for (int i=5; i < interpretData.Length; i++) {topicstring += interpretData[i] + " ";}
				String[] nick1 = interpretData[0].Split('!');
				String[] nick2 = nick1[0].Split(':');
				dbconnect.addtopic(topicstring, nick2[1]);
				return "Topic: " + topicstring + ": was added by " + nick2[1];
			} else {
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				return dbconnect.topic(randomnumber,stuff, otherstuff);
			}
        }
		
		static string lookupliteracy(string stuff) {
			int randomnumber = new Random().Next(184);
                databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			return dbconnect.literacy(randomnumber, stuff);
        }
		
		static int rolldice(int sides) {
			int diceroll;
			if (sides == 0) diceroll = new Random().Next(7);
				else diceroll = new Random().Next(sides + 1);
			return diceroll;
        }
		
		static string sha1stuff(string source) {
			ASCIIEncoding enc = new ASCIIEncoding();
            SHA1 sha = new SHA1CryptoServiceProvider();
			byte[] me = sha.ComputeHash(enc.GetBytes(source));
			string result = "0x";
            for (int i = 0; i < me.Length; i++)
            {
                result += me[i].ToString("X2");
            }
			return result;
        }
    }
}
