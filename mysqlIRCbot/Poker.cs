using System;
namespace mysqlIRCbot
{
	public class Poker
	{
		public static int players = 0;
		public static int maxplayers = 5;
		public static Player player1 = new Player();
		public static Player player2 = new Player();
		public static Player player3 = new Player();
		public static Player player4 = new Player();
		public static Player player5 = new Player();

		public static int card1;
		public static int card2;
		public static int card3;

		public static int center1;
		public static int center2;
		public static int center3;
		public static int center4;
		public static int center5;

		public static Random rand = new Random();

		public static string cardone;
		public static string cardtwo;
		public static string cardthree;

		public static int centercard = 0;

		public static bool delt = false;
		public static bool started = false;

		public static string next ()
		{
			if (delt == true) {
			do {card1 = rand.Next(53);} while ((card1 == card2) || (card1 == card3) || (card1 == center1) || (card1 == center2) || (card1 == center3) || (card1 == center4) || (card1 == center5) || (card1 == player1.hand[0]) || (card1 == player1.hand[1]) || (card1 == player2.hand[0]) || (card1 == player2.hand[1]) || (card1 == player3.hand[0]) || (card1 == player3.hand[1]) || (card1 == player4.hand[0]) || (card1 == player4.hand[1]) || (card1 == player5.hand[0]) || (card1 == player5.hand[1]));
			do {card2 = rand.Next(53);} while ((card2 == card1) || (card2 == card3) || (card2 == center1) || (card2 == center2) || (card2 == center3) || (card2 == center4) || (card2 == center5) || (card2 == player1.hand[0]) || (card2 == player1.hand[1]) || (card2 == player2.hand[0]) || (card2 == player2.hand[1]) || (card2 == player3.hand[0]) || (card2 == player3.hand[1]) || (card2 == player4.hand[0]) || (card2 == player4.hand[1]) || (card2 == player5.hand[0]) || (card2 == player5.hand[1]));
			do {card3 = rand.Next(53);} while ((card3 == card1) || (card3 == card2) || (card3 == center1) || (card3 == center2) || (card3 == center3) || (card3 == center4) || (card3 == center5) || (card3 == player1.hand[0]) || (card3 == player1.hand[1]) || (card3 == player2.hand[0]) || (card3 == player2.hand[1]) || (card3 == player3.hand[0]) || (card3 == player3.hand[1]) || (card3 == player4.hand[0]) || (card3 == player4.hand[1]) || (card3 == player5.hand[0]) || (card3 == player5.hand[1]));
			if (centercard < 5)
			{
				if (centercard == 0) 
				{
					databaseMYSQL dbconnect1 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					center1 = card1;
					cardone = dbconnect1.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					center2 = card2; //1-3 the flop. 4 the turn. 5 the river.
					cardtwo = dbconnect2.cardlookup(card2);
					databaseMYSQL dbconnect3 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					center3 = card3;
					cardthree = dbconnect3.cardlookup(card3);
					centercard = 3;
					return "The flop: " + cardone + " " + cardtwo + " " + cardthree;
				}
				else if (centercard == 3) 
				{
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					center4 = card1;
					cardone = dbconnect.cardlookup(card1);
					centercard++;
					return "The turn: " + cardone;
				}
				else if (centercard == 4) 
				{
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					center5 = card1;
					cardone = dbconnect.cardlookup(card1);
					centercard++;
					return "The river: " + cardone;
				} else return "error";
			} else return "i should show you everyone's cards here"; //show cards and tell people to end
			} else return "you need to deal first";
		}

		public static void deal ()
		{
			if (delt == false) {
			for (int i = 1; i <= 5; i++)
			{
				do {card1 = rand.Next(53);} while ((card1 == card2) || (card1 == player1.hand[0]) || (card1 == player1.hand[1]) || (card1 == player2.hand[0]) || (card1 == player2.hand[1]) || (card1 == player3.hand[0]) || (card1 == player3.hand[1]) || (card1 == player4.hand[0]) || (card1 == player4.hand[1]) || (card1 == player5.hand[0]) || (card1 == player5.hand[1]));
				do {card2 = rand.Next(53);} while ((card1 == card2) || (card2 == player1.hand[0]) || (card2 == player1.hand[1]) || (card2 == player2.hand[0]) || (card2 == player2.hand[1]) || (card2 == player3.hand[0]) || (card2 == player3.hand[1]) || (card2 == player4.hand[0]) || (card2 == player4.hand[1]) || (card2 == player5.hand[0]) || (card2 == player5.hand[1]));
				if (i == 1) 
				{
					player1.hand[0] = card1;
					player1.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardone = dbconnect.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardtwo = dbconnect2.cardlookup(card2);
					ircbot.write("PRIVMSG " + player1.name + " :Your cards are " + cardone + " and " + cardtwo, ircbot.writer);
				} else if (i == 2) 
				{
					player2.hand[0] = card1;
					player2.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardone = dbconnect.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardtwo = dbconnect2.cardlookup(card2);
					ircbot.write("PRIVMSG " + player2.name + " :Your cards are " + cardone + " and " + cardtwo, ircbot.writer);
				} else if (i == 3) 
				{
					player3.hand[0] = card1;
					player3.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardone = dbconnect.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardtwo = dbconnect2.cardlookup(card2);
					ircbot.write("PRIVMSG " + player3.name + " :Your cards are " + cardone + " and " + cardtwo, ircbot.writer);
				} else if (i == 4) 
				{
					player4.hand[0] = card1;
					player4.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardone = dbconnect.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardtwo = dbconnect2.cardlookup(card2);
					ircbot.write("PRIVMSG " + player4.name + " :Your cards are " + cardone + " and " + cardtwo, ircbot.writer);
				} else if (i == 5) 
				{
					player5.hand[0] = card1;
					player5.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardone = dbconnect.cardlookup(card1);
					databaseMYSQL dbconnect2 = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					cardtwo = dbconnect2.cardlookup(card2);
					ircbot.write("PRIVMSG " + player5.name + " :Your cards are " + cardone + " and " + cardtwo, ircbot.writer);
				}
			}
			delt = true;
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :already dealt!", ircbot.writer);
		}

		public static void join (string nick)
		{
			if (delt == false) {
			if (started == true) {
			if ((nick != player1.name) && (nick != player2.name) && (nick != player3.name) && (nick != player4.name) && (nick != player5.name))
			{
			ircbot.write("PRIVMSG " + ircbot.channel + " :Player " + nick + " just joined!", ircbot.writer);
			if (players == 0)
			{
				player1.name = nick;
				players++;
			} else if (players == 1)
			{
				player2.name = nick;
				players++;
			} else if (players == 2)
			{
				player3.name = nick;
				players++;
			} else if (players == 3)
			{
				player4.name = nick;
				players++;
			} else if (players == 4)
			{
				player5.name = nick;
				players++;
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :only 5 players allowed!", ircbot.writer);
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :you can't join twice!", ircbot.writer);
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :not started yet!", ircbot.writer);
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :game already in progress!", ircbot.writer);
		}

		public static void clear ()
		{
			players = 0;
			delt = false;
			started = false;

			cardone = "";
			cardtwo = "";
			cardthree = "";

			centercard = 0;

			card1 = 0;
			card2 = 0;
			card3 = 0;

			center1 = 0;
			center2 = 0;
			center3 = 0;
			center4 = 0;
			center5 = 0;

			player1.name = "";
			player1.hand[0] = 0;
			player1.hand[1] = 0;
			player1.points = 0;

			player2.name = "";
			player2.hand[0] = 0;
			player2.hand[1] = 0;
			player2.points = 0;

			player3.name = "";
			player3.hand[0] = 0;
			player3.hand[1] = 0;
			player3.points = 0;

			player4.name = "";
			player4.hand[0] = 0;
			player4.hand[1] = 0;
			player4.points = 0;

			player5.name = "";
			player5.hand[0] = 0;
			player5.hand[1] = 0;
			player5.points = 0;
		}
	}
}

