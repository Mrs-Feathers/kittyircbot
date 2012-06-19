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

		public static bool delt = false;

		public Poker ()
		{
		}

		public static string next ()
		{
			//next card here
			return "not implemented yet.";
		}

		public static void deal ()
		{
			if (delt == false) {
			for (int i = 1; i <= 5; i++)
			{
				do {card1 = new Random().Next(53);} while ((card1 == player1.hand[0]) || (card1 == player1.hand[1]) || (card1 == player2.hand[0]) || (card1 == player2.hand[1]) || (card1 == player3.hand[0]) || (card1 == player3.hand[1]) || (card1 == player4.hand[0]) || (card1 == player4.hand[1]) || (card1 == player5.hand[0]) || (card1 == player5.hand[1]));
				do {card2 = new Random().Next(53);} while ((card2 == player1.hand[0]) || (card2 == player1.hand[1]) || (card2 == player2.hand[0]) || (card2 == player2.hand[1]) || (card2 == player3.hand[0]) || (card2 == player3.hand[1]) || (card2 == player4.hand[0]) || (card2 == player4.hand[1]) || (card2 == player5.hand[0]) || (card2 == player5.hand[1]));
				if (i == 1) 
				{
					player1.hand[0] = card1;
					player1.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					ircbot.write("PRIVMSG " + player1.name + " :Your cards are" + dbconnect.cardlookup(card1,card2), ircbot.writer);
				} else if (i == 2) 
				{
					player2.hand[0] = card1;
					player2.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					ircbot.write("PRIVMSG " + player2.name + " :Your cards are" + dbconnect.cardlookup(card1,card2), ircbot.writer);
				} else if (i == 3) 
				{
					player3.hand[0] = card1;
					player3.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					ircbot.write("PRIVMSG " + player3.name + " :Your cards are" + dbconnect.cardlookup(card1,card2), ircbot.writer);
				} else if (i == 4) 
				{
					player4.hand[0] = card1;
					player4.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					ircbot.write("PRIVMSG " + player4.name + " :Your cards are" + dbconnect.cardlookup(card1,card2), ircbot.writer);
				} else if (i == 5) 
				{
					player5.hand[0] = card1;
					player5.hand[1] = card2;
					databaseMYSQL dbconnect = new databaseMYSQL(ircbot.mysqlhostname, ircbot.mysqlport, ircbot.mysqlusername, ircbot.mysqlpassword, ircbot.database);
					ircbot.write("PRIVMSG " + player5.name + " :Your cards are" + dbconnect.cardlookup(card1,card2), ircbot.writer);
				}
			}
			delt = true;
			} else ircbot.write("PRIVMSG " + ircbot.channel + " :already dealt!", ircbot.writer);
		}

		public static void join (string nick)
		{
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
		}

		public static void clear ()
		{
			players = 0;
			delt = false;

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

