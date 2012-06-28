using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Security.Cryptography;
using System.Timers;
using System.Collections.Generic;

namespace kittyIRCbot
{  
    public class ircbot {
        public static String username, nickpassword, hostname, description, channel;
        public static String mysqlhostname, mysqlusername, mysqlpassword, database;
        public static int port, mysqlport, botsetting;
		public static bool v = true;
        public static List<string> admins = new List<string>();

		
		public static int topicnumber = 0;
		public static int topicmax = 969;

		public static double maxxp = 1000000;

		public static System.Timers.Timer topictime = new System.Timers.Timer(60000);
		public static bool allowtopic = true;
         
        public static TcpClient socket;
        public static StreamReader reader;
        public static StreamWriter writer;
		
		public static string[] interpretData;
   
        static void Main() {   
			topictime.Elapsed += new ElapsedEventHandler(topictime_Elapsed);
            try {
            loadConfig();
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
			dbconnect.init(botsetting);
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
					gainxp();
                }
                else if (interpretData[2].Equals(username)) {
                    onPrivateMessage(interpretData[0], interpretData[3]);
                }
            }
            else if(interpretData[1].Equals("376")) { //421
				write("JOIN " + channel, writer);
				write("PRIVMSG NickServ IDENTIFY " + nickpassword, writer);
			}
   		}
   
        static void loadConfig() {
            try {
                string[] lines = File.ReadAllLines("config.cfg");
                foreach (string line in lines) {
                string[] data = line.Split(':');
                if (data[0].Equals("mysqlusername")) mysqlusername = data[1];
                else if (data[0].Equals("database")) database = data[1];
                else if (data[0].Equals("mysqlpassword")) mysqlpassword = data[1];
                else if (data[0].Equals("mysqlhostname")) mysqlhostname = data[1];
				else if (data[0].Equals("mysqlport")) mysqlport = Int32.Parse(data[1]);
				else if (data[0].Equals("botsetting")) botsetting = Int32.Parse(data[1]);
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

        static void onJoin(string data) {
            String[] working = data.Split('!');
            String user = working[0].Substring(1);
			string welcome = "";
			if (!user.Equals(username) && (v == true)) {
            write("MODE " + channel + " +v " + user, writer);
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
			welcome = dbconnect.welcomelookup(user);
			if (welcome != "") write("PRIVMSG " + channel + " :" + welcome, writer);
            }
        }

		static void onPublicMessage(string data) {
			String[] nick3 = interpretData[0].Split('!');
			String[] nick4 = nick3[0].Split(':');
			String nick44 = nick4[1];
            data = data.Substring(1);
            if (data.Equals("!info")) write("PRIVMSG " + channel + " :kittyIRCbot v1.0 written by auroriumoxide katja.decuir@gmail.com", writer);
            else if (data.Equals("!time")) {
            DateTime time = DateTime.Now;
            String now = String.Format("{0:F}", time);
            write("PRIVMSG " + channel + " :" + now, writer);
            }
			else if (data.Equals("!help")) { write("PRIVMSG " + nick44 + " :!help = this; !time = get the time; !info = get info; !v = turn +v on or off; !topicrand = random topic; !topic [number] name = get a topic or show specific topic by adding a number. adding 'name' shows you who wrote it; !topic add [topic] = this will add a topic to a list we can approve; !gtfo = exit; !literacy [country] = tells you the literacy rate of a given country. tells you a random one if no country provided;", writer); write("PRIVMSG " + nick44 + " :!dice [number] = rolls a dice with [number] sides; !sha1 [string] = encrypts a string to a sha1 hash; !pi [number] = calculates pi to a given number; !poker = to play texas hold 'em poker; !welcome [message] = will create a welcome message for you; !desc [name] or !desc update [message] = gives a description about someone or updates your description;", writer); write("PRIVMSG " + nick44 + " :!echo = make the bot say something in channel; !act = make the bot /me something in the channel; !stats [nick] = see someone's XP stats; nickmapping = please ask auroriumoxide to map your name to your main nick; !topstats = see a top ten list of the people with the highest xp points; !showawards = show awards won; !showawards notwon = shows awards that no one has won;", writer); write("PRIVMSG " + nick44 + " :!award [nick] [award #] [reason] = awards an award to someone; !enc [key] [message] = use a stream cipher to encrypt a message with a key; !dec [key] [message] = use a stream cipher to decrypt a message with a key;", writer);}
			else if (data.Equals("!v")) { try { String[] nick1 = interpretData[0].Split('!'); String[] nick2 = nick1[0].Split(':'); String nick = nick2[1];
					if (interpretData[4].Equals("on") && admins.Contains(nick)) v = true; else if (interpretData[4].Equals("off") && admins.Contains(nick)) v = false; else write("PRIVMSG " + channel + " : use !v with on or off only", writer); }
				catch { write("PRIVMSG " + channel + " :use !v with on or off only", writer); }
        	}
			else if (data.Equals("!gtfo")) { String[] nick1 = interpretData[0].Split('!'); String[] nick2 = nick1[0].Split(':'); String nick = nick2[1];
    			if (nick == "auroriumoxide") { write("QUIT :Quit: goodbye", writer); Environment.ExitCode = 0;
					Environment.Exit(0); } else write("PRIVMSG " + channel + " :Error: " + nick + " is not in the sudoers file. This incident will be reported.", writer);
  			}
			else if (data.Equals("!topic")) { if (allowtopic == true) { try { write("PRIVMSG " + channel + " :" + lookuptopic(interpretData[4],interpretData[5]), writer); } catch { try { write("PRIVMSG " + channel + " :" + lookuptopic(interpretData[4],null), writer); } catch { write("PRIVMSG " + channel + " :" + lookuptopic(null,null), writer); } } allowtopic = false; topictime.Enabled = true;}}
			else if (data.Equals("!desc")) { try { write("PRIVMSG " + channel + " :" + lookupdescription(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :" + lookupdescription(null), writer); } }
			else if (data.Equals("!topicrand")) { if (allowtopic == true) {  try { write("PRIVMSG " + channel + " :" + lookuptopicrand(interpretData[4],interpretData[5]), writer); } catch { try { write("PRIVMSG " + channel + " :" + lookuptopicrand(interpretData[4],null), writer); } catch { write("PRIVMSG " + channel + " :" + lookuptopicrand(null,null), writer); } } allowtopic = false; topictime.Enabled = true;}}
			else if (data.Equals("!literacy")) try { write("PRIVMSG " + channel + " :" + lookupliteracy(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :" + lookupliteracy(null), writer); }
			else if (data.Equals("!dice")) try { write("PRIVMSG " + channel + " :" + rolldice(Int32.Parse(interpretData[4])), writer); } catch { write("PRIVMSG " + channel + " :" + rolldice(0), writer); }
			else if (data.Equals("!sha1")) try { write("PRIVMSG " + channel + " :" + sha1stuff(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :Sha1 hashing failed.", writer); }
			else if (data.Equals("!pi")) try { CalculatePI myPI = new CalculatePI (Int32.Parse(interpretData[4])); } catch { write("PRIVMSG " + channel + " :3.14", writer); }
			else if (data.Equals("!poker")) try { pokerstuff(interpretData[4],interpretData[5]); } catch { try { pokerstuff(interpretData[4],null); } catch { pokerstuff(null,null); } }
			else if (data.Equals("!welcome")) try { write("PRIVMSG " + channel + " :" + welcome(), writer); string stuff = interpretData[4]; } catch { write("PRIVMSG " + channel + " :failed.", writer); }
			else if (data.Equals("!echo")) try { write("PRIVMSG " + channel + " :" + echo(), writer); } catch { write("PRIVMSG " + channel + " :For some reason, you suck at not failing.", writer); }
			else if (data.Equals("!act")) try { write("PRIVMSG " + channel + " :" + (char)1 + "ACTION " + echo() + (char)1, writer); } catch { write("PRIVMSG " + channel + " :For some reason, you suck at not failing.", writer); }
			else if (data.Equals("!stats")) { try { write("PRIVMSG " + channel + " :" + getstats(interpretData[4]), writer); } catch { write("PRIVMSG " + channel + " :" + getstats(null), writer); } }
			else if (data.Equals("!topstats")) try { topten(); } catch { write("PRIVMSG " + nick44 + " :Error", writer); }
			else if (data.Equals("!showawards")) { try { showawards(interpretData[4]); } catch { showawards(null); }}
			else if (data.Equals("!enc")) { try { write("PRIVMSG " + channel + " :" + encrypt(interpretData[4]), writer); string stuff = interpretData[5]; } catch { write("PRIVMSG " + channel + " :use: !enc [key] [message]", writer); } }
			else if (data.Equals("!dec")) { try { write("PRIVMSG " + channel + " :" + StreamCipher.decrypt(interpretData[4],interpretData[5]), writer); } catch { write("PRIVMSG " + channel + " :use: !dec [key] [message]", writer); } }
		}
		
        static void onPrivateMessage(String user, String data) {
        String[] sender = user.Split('!');
        user = sender[0].Substring(1);
        data = data.Substring(1);
            if (data.Equals("!info")) write("PRIVMSG " + user + " :kittyIRCbot v1.0 written by auroriumoxide katja.decuir@gmail.com", writer);
            else if (data.Equals("!time")) {
            DateTime time = DateTime.Now;
            String now = String.Format("{0:F}", time);
            write("PRIVMSG " + user + " :" + now, writer);
            }
			else if (data.Equals("!help")) { write("PRIVMSG " + user + " :!help = this; !time = get the time; !info = get info; !v = turn +v on or off; !topicrand = random topic; !topic [number] name = get a topic or show specific topic by adding a number. adding 'name' shows you who wrote it; !topic add [topic] = this will add a topic to a list we can approve; !gtfo = exit; !literacy [country] = tells you the literacy rate of a given country. tells you a random one if no country provided;", writer); write("PRIVMSG " + user + " :!dice [number] = rolls a dice with [number] sides; !sha1 [string] = encrypts a string to a sha1 hash; !pi [number] = calculates pi to a given number; !poker = to play texas hold 'em poker; !welcome [message] = will create a welcome message for you; !desc [name] or !desc update [message] = gives a description about someone or updates your description;", writer); write("PRIVMSG " + user + " :!echo = make the bot say something in channel; !act = make the bot /me something in the channel; !stats [nick] = see someone's XP stats; nickmapping = please ask auroriumoxide to map your name to your main nick; !topstats = see a top ten list of the people with the highest xp points; !showawards = show awards won; !showawards notwon = shows awards that no one has won;", writer); write("PRIVMSG " + user + " :!award [nick] [award #] [reason] = awards an award to someone; !enc [key] [message] = use a stream cipher to encrypt a message with a key; !dec [key] [message] = use a stream cipher to decrypt a message with a key;", writer);}
			else if (data.Equals("!v")) { try { String[] nick1 = interpretData[0].Split('!'); String[] nick2 = nick1[0].Split(':'); String nick = nick2[1];
					if (interpretData[4].Equals("on") && admins.Contains(nick)) v = true; else if (interpretData[4].Equals("off") && admins.Contains(nick)) v = false; else write("PRIVMSG " + user + " : use !v with on or off only", writer); }
				catch { write("PRIVMSG " + user + " :use !v with on or off only", writer); }
        	}
			else if (data.Equals("!gtfo")) { String[] nick1 = interpretData[0].Split('!'); String[] nick2 = nick1[0].Split(':'); String nick = nick2[1];
    			if (nick == "auroriumoxide") { write("QUIT :Quit: goodbye", writer); Environment.ExitCode = 0;
					Environment.Exit(0); } else write("PRIVMSG " + user + " :Error: " + nick + " is not in the sudoers file. This incident will be reported.", writer);
  			}
			else if (data.Equals("!desc")) { try { write("PRIVMSG " + user + " :" + lookupdescription(interpretData[4]), writer); } catch { write("PRIVMSG " + user + " :" + lookupdescription(null), writer); } }
			else if (data.Equals("!literacy")) try { write("PRIVMSG " + user + " :" + lookupliteracy(interpretData[4]), writer); } catch { write("PRIVMSG " + user + " :" + lookupliteracy(null), writer); }
			else if (data.Equals("!dice")) try { write("PRIVMSG " + user + " :" + rolldice(Int32.Parse(interpretData[4])), writer); } catch { write("PRIVMSG " + user + " :" + rolldice(0), writer); }
			else if (data.Equals("!sha1")) try { write("PRIVMSG " + user + " :" + sha1stuff(interpretData[4]), writer); } catch { write("PRIVMSG " + user + " :Sha1 hashing failed.", writer); }
			else if (data.Equals("!pi")) write("PRIVMSG " + user + " :Only use !pi in the main channel", writer);
			else if (data.Equals("!topic")) write("PRIVMSG " + user + " :Only use !topic in the main channel", writer);
			else if (data.Equals("!topicrand")) write("PRIVMSG " + user + " :Only use !topicrand in the main channel", writer);
			else if (data.Equals("!poker")) write("PRIVMSG " + user + " :Only use !poker in the main channel", writer);
			else if (data.Equals("!welcome")) try { write("PRIVMSG " + user + " :" + welcome(), writer); string stuff = interpretData[4]; } catch { write("PRIVMSG " + user + " :failed.", writer); }
			else if (data.Equals("!echo")) try { write("PRIVMSG " + channel + " :" + echo(), writer); } catch { write("PRIVMSG " + user + " :For some reason, you suck at not failing.", writer); }
			else if (data.Equals("!act")) try { write("PRIVMSG " + channel + " :" + (char)1 + "ACTION " + echo() + (char)1, writer); } catch { write("PRIVMSG " + user + " :For some reason, you suck at not failing.", writer); }
        	else if (data.Equals("!stats")) { try { write("PRIVMSG " + user + " :" + getstats(interpretData[4]), writer); } catch { write("PRIVMSG " + user + " :" + getstats(null), writer); } }
			else if (data.Equals("!topstats")) try { topten(); } catch { write("PRIVMSG " + user + " :Error", writer); }
			else if (data.Equals("!showawards")) { try { showawards(interpretData[4]); } catch { showawards(null); } }
			else if (data.Equals("!enc")) { try { write("PRIVMSG " + user + " :" + encrypt(interpretData[4]), writer); string stuff = interpretData[5]; } catch { write("PRIVMSG " + user + " :use: !enc [key] [message]", writer); } }
			else if (data.Equals("!dec")) { try { write("PRIVMSG " + user + " :" + StreamCipher.decrypt(interpretData[4],interpretData[5]), writer); } catch { write("PRIVMSG " + user + " :use: !dec [key] [message]", writer); } }
		}

		static void pokerstuff(string stuff, string otherstuff) {
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];

			switch (stuff)
			{
			case "end":
				write("PRIVMSG " + channel + " :This is the end of the round. type '!poker start' for another.", writer);
				Poker.clear();
				//show points
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

		static string encrypt(string key)
		{
			string message = "";
			for (int i=5; i < interpretData.Length; i++) {message += interpretData[i] + " ";}
			return StreamCipher.encrypt(key, message);
		}

		static string echo () {
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];
			string echostring = "";
			DateTime now = DateTime.Now;
			for (int i=4; i < interpretData.Length; i++) {echostring += interpretData[i] + " ";}
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			dbconnect.addecho(nick,echostring,now);
			return echostring;
		}

		static void showawards (string stuff)
		{
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			if (stuff == "notwon") { dbconnect.showawardsnotwon(nick); } else {dbconnect.showawards(nick);}
		}

		static void topten ()
		{
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			dbconnect.topten(nick);
		}

		static void gainxp () {
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			String nick = nick2[1];
			databaseMYSQL dbconnect2 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			string nick3;
			try {nick3 = dbconnect2.getmain(nick);} catch {nick3 = "";}
			if (nick3 != "") nick = nick3;
			string[] echostring = new string[interpretData.Length - 3];
			int x = 0;
			double xp = 0;
			double xp2 = 0;
			for (int i=3; i < interpretData.Length; i++) {echostring[x] = interpretData[i] + " "; x++;}
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			try {xp = dbconnect.getxp(nick); xp2 = xp;} catch {xp = 0;}
			databaseMYSQL dbconnect1 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			double hello = 0;
			if ((xp == 0)) {try {dbconnect1.addxp(nick,echostring.Length);} catch {Console.WriteLine("error");}}
			try {hello = xp + echostring.Length;} catch {try {dbconnect1.addxp(nick,echostring.Length);} catch {Console.WriteLine("error");}}
			xp = hello;
			string infostuffs = "";
			try {if (Calculatelevel(xp) == 1) {
					databaseMYSQL dbconnect12 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
					string linkstuff = dbconnect12.levelonelinklookup(botsetting);
					infostuffs = " For more information, visit" + linkstuff;
				
				} 
				if (Calculatelevel(xp2) != Calculatelevel(xp)) write("PRIVMSG " + channel + " :" + nick + " is now at level " + Calculatelevel(xp) + infostuffs, writer);} catch {Console.WriteLine("parse error");}
			try {dbconnect1.updatexp(nick,xp);} catch {try {dbconnect1.addxp(nick,xp);} catch {Console.WriteLine("error");}}
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
				topicnumber++;
				if (topicnumber > topicmax) topicnumber = 1;
				changetopicnumber();
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				return dbconnect.topic(topicnumber,stuff, otherstuff);
			}
        }

		static string lookupdescription(string stuff) {
			if (stuff == "update") {
				string descriptionstring = "";
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
				for (int i=5; i < interpretData.Length; i++) {descriptionstring += interpretData[i] + " ";}
				String[] nick1 = interpretData[0].Split('!');
				String[] nick2 = nick1[0].Split(':');
				string nick = nick2[1];
				databaseMYSQL dbconnect2 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				string nick3;
				try {nick3 = dbconnect2.getmain(nick);} catch {nick3 = "";}
			if (nick3 != "") nick = nick3;
				if ((dbconnect.desclookup(nick) != "") && (descriptionstring != " ") && (descriptionstring != "")) 
				{
					databaseMYSQL dbconnect1 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
					return dbconnect1.descupdate(nick,descriptionstring);
				} else if ((descriptionstring != " ") && (descriptionstring != "")) {
					databaseMYSQL dbconnect1 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
					return dbconnect1.descadd(nick,descriptionstring);
				} else return nick + ": you need to type something after !desc update";
			} else {
				String[] nick1 = interpretData[0].Split('!');
				String[] nick2 = nick1[0].Split(':');
				string nick = nick2[1];
				databaseMYSQL dbconnect2 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				string nick3;
				try {nick3 = dbconnect2.getmain(nick);} catch {nick3 = "";}
			if (nick3 != "") nick = nick3;
				string hellohowareyou = "";
				if (stuff == null) hellohowareyou = nick; else hellohowareyou = stuff;
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				return hellohowareyou + ": " + dbconnect.desclookup(hellohowareyou);
			}
        }

				static string getstats(string stuff) {
				String[] nick1 = interpretData[0].Split('!');
				String[] nick2 = nick1[0].Split(':');
				string nick = nick2[1];
				databaseMYSQL dbconnect2 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			string nick3;	
			try { nick3 = dbconnect2.getmain(nick);} catch { nick3 = "";}
				if (nick3 != "") nick = nick3;
				string hellohowareyou = "";
				if (stuff == null) hellohowareyou = nick; else hellohowareyou = stuff;
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
				double xp;
			try { xp = dbconnect.getxp(hellohowareyou); } catch {xp = 0;}
			double level = Calculatelevel(xp);
			return hellohowareyou + ": 12Level: " + level + " 12XP: " + xp + " 12XP needed for next level: " + CalculateXP(level, xp);
        }

		static double Calculatelevel (double xp)
		{
			for (double i = 0; i < 100; i++)
			{
				if ((xp > (maxxp*((i)/100))) && (xp < (maxxp*((i+1)/100)))) {return i+1;}
			}
			return 0;
		}

		static double CalculateXP (double level, double xp)
		{
			return (maxxp*((level)/100)) - xp;
		}

		static string welcome ()
		{
			string welcomestring = "";
			databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
			for (int i=4; i < interpretData.Length; i++) {welcomestring += interpretData[i] + " ";}
			String[] nick1 = interpretData[0].Split('!');
			String[] nick2 = nick1[0].Split(':');
			string nick = nick2[1];
			databaseMYSQL dbconnect2 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
			string nick3;
			try { nick3 = dbconnect2.getmain(nick);} catch { nick3 = "";}
			if (nick3 != "") nick = nick3;
			if ((dbconnect.welcomelookup(nick) != "") && (welcomestring != " ") && (welcomestring != "")) 
			{
				databaseMYSQL dbconnect1 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
				return dbconnect1.welcomeupdate(nick,welcomestring);
			} else if ((welcomestring != " ") && (welcomestring != "")) {
				databaseMYSQL dbconnect1 = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
				return dbconnect1.welcomeadd(nick,welcomestring);
			} else return nick + ": you need to type something after !welcome";
		}

		static string lookuptopicrand(string stuff, string otherstuff) {
			int randomnumber = new Random().Next(1000);
				databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				return dbconnect.topic(randomnumber,stuff, otherstuff);
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

		static void topictime_Elapsed (object o,EventArgs ee)
		{
			allowtopic = true;
			topictime.Enabled = false;
		}

		public static void adminsadd (string[] adminstoadd)
		{
			foreach (string a in adminstoadd)
			{
				admins.Add(a);
			}
		}

		static void changetopicnumber () {
		try {
                databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);    
				dbconnect.topicchange(topicnumber);
            }
            catch {
            Console.WriteLine("cannot connect to database, bitch.");
            }
		}
    }
}
