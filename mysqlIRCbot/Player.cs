using System;
namespace kittyIRCbot
{
	public class Player
	{
		public string name;
		public int[] hand = new int[2];
		public int points;
		
		public Player ()
		{
			hand[0] = 0;
			hand[1] = 0;
		}
	}
}

