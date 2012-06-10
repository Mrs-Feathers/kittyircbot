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
            write("PRIVMSG " + channel + " : " + now, writer);
            }
			else if (data.Equals("!help")) write("PRIVMSG " + channel + " : !help = this; !time = get the time; !info = get info; !v = turn +v on or off; !topic = get a random topic; !gtfo = exit; !literacy [country] = tells you the literacy rate of a given country. tells you a random one if no country provided; !dice [number] = rolls a dice with [number] sides; !sha1 [string] = encrypts a string to a sha1 hash;", writer);
			else if (data.Equals("!v")) { try {
					if (interpretData[4].Equals("on")) v = true; else if (interpretData[4].Equals("off")) v = false; else write("PRIVMSG " + channel + " : use !v with on or off only", writer); }
				catch { write("PRIVMSG " + channel + " : use !v with on or off only", writer); }
        	}
			else if (data.Equals("!gtfo")) {
    			Environment.ExitCode = 0;
     			Environment.Exit(0);
  			}
			else if (data.Equals("!topic")) write("PRIVMSG " + channel + " :" + lookuptopic(), writer);
			else if (data.Equals("!literacy")) try { write("PRIVMSG " + channel + " :" + lookupliteracy(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :" + lookupliteracy(null), writer); }
			else if (data.Equals("!dice")) try { write("PRIVMSG " + channel + " :" + rolldice(Int32.Parse(interpretData[4])), writer); } catch { write("PRIVMSG " + channel + " :" + rolldice(0), writer); }
			else if (data.Equals("!sha1")) try { write("PRIVMSG " + channel + " :" + sha1stuff(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :Sha1 hashing failed.", writer); }
			else if (data.Equals("!pi")) try { CalculatePI myPI = new CalculatePI (Int32.Parse(interpretData[4])); } catch { write("PRIVMSG " + channel + " :3.14", writer); }
		}
		
        static void onPrivateMessage(String user, String data) {
        String[] sender = user.Split('!');
        user = sender[0].Substring(1);
        write("PRIVMSG " + user + " :" + username + ": please use commands in " + channel, writer);
        }
        
        static string lookuptopic() {
			int randomnumber = new Random().Next(1001);
                databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			return dbconnect.topic(randomnumber);
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
