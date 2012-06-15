using System;
namespace mysqlIRCbot
{
	public class Player
	{
		public string name;
		public Array hand;
		public int points;
		
		public Player (string name_, Array hand_, int points_)
		{
			name = name_;
			hand = hand_;
			points = points_;
		}
	}
}

