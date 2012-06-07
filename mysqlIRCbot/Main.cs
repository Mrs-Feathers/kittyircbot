using System;
using System.Net;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Threading;

namespace mysqlIRCbot
{  
    public class ircbot {
        public static String username, hostname, description, channel;
        public static String mysqlhostname, mysqlusername, mysqlpassword, database;
        public static int port, mysqlport;
        // Storing of nicklist... make this the admin list who can give this bot commands
        public static String[] nicks;
         
        public static TcpClient socket;
        public static StreamReader reader;
        public static StreamWriter writer;
   
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
        string[] interpretData = data.Split(' ');
            if(interpretData[0].Equals("PING")) onPing(interpretData[1]);
			// If someone joins channel trigger join event.. if v = 1 then give them voice.
            else if(interpretData[1].Equals("JOIN")) onJoin(interpretData[0]);
            // If someone leaves channel trigger part event.. don't know if i need this
            else if(interpretData[1].Equals("PART") || interpretData[1].Equals("QUIT")) onPart(interpretData[0]);
			
            else if(interpretData[1].Equals("PRIVMSG")) {
                if (interpretData[2].Equals(channel)) {
                    onPublicMessage(interpretData[3]);
                }
                else if (interpretData[2].Equals(username)) {
                    onPrivateMessage(interpretData[0], interpretData[3]);
                }
            }
            else if(interpretData[1].Equals("221")) write("JOIN " + channel, writer);
            // If the server is sending the nicklist across add them to database.. don't need this
            else if (interpretData[1].Equals("353")) nicks = interpretData;
            // Write nicklist to MYSQL database
            else if (interpretData[1].Equals("366")) {
            Thread thread = new Thread(onNickList); //don'T need this.
            thread.Start();
            }  
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
				else if (data[0].Equals("admins")) admin = (data);
                }
            }
            catch {
            Console.WriteLine("Cannot read configuration file, are you sure it is there?");
            }
        }

        /* Writing data to the IRC server */
        static void write(string data, StreamWriter writer) {
            try {
            writer.WriteLine(data);
            Console.WriteLine(">>> " + data);
            writer.Flush();
            }
            catch {
            Console.WriteLine("Error!");
            }
        }
    /* On ping request write pong back to the server */
        static void onPing(string pong) {
            pong = "PONG " + pong;
            write(pong, writer);   
        }
    /* On join to channel.. give them +v */
        static void onJoin(string data) {
            String[] working = data.Split('!');
            String user = working[0].Substring(1);
            if (!user.Equals(username)) {
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
			else if (data.Equals("!help")) write("PRIVMSG " + channel + " : !help = this; !time = get the time; !info = get info;", writer);
        }
		
        static void onPrivateMessage(String user, String data) {
        String[] sender = user.Split('!');
        user = sender[0].Substring(1);
        write("PRIVMSG " + user + " :" + username + ": please use commands in " + channel, writer);
        }
           
    /* Don't need this */
        static void onPart(String data) {
        String[] working = data.Split('!');
        String user = working[0].Substring(1);
            if (!user.Equals(username)) {
                databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
                dbconnect.removeuser(user);
            }
        }
   
    /* Don't need this */
        static void onNickList() {
            nicks[5] = nicks[5].Substring(1);
            Console.Write("length" + nicks.Length);
            for (int i = 1; i < (nicks.Length - 4); i++) {
            databaseMYSQL dbconnect = new databaseMYSQL(mysqlhostname, mysqlport, mysqlusername, mysqlpassword, database);
            if (!nicks[4+i].Equals(username)) dbconnect.adduser(nicks[4+i]);
            }
        }
    }
}
