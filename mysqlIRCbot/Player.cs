using System;
namespace mysqlIRCbot
{
	public class Player
	{
		public string name;
		public int[] hand;
		public int points;
		
		public Player ()
		{
			hand[0] = 0;
			hand[1] = 0;
		}
	}
}

