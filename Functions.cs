using System;
using System.Collections.Generic;

namespace TBChatExample
{
	public class Functions
	{
		static Dictionary<int, ConsoleColor> colors = new Dictionary<int, ConsoleColor>();
		public static void InitColors()
		{
			colors.Add(1, ConsoleColor.Blue);
			colors.Add(2, ConsoleColor.Green);
			colors.Add(3, ConsoleColor.Red);
			colors.Add(4, ConsoleColor.Yellow);
			colors.Add(5, ConsoleColor.Cyan);
		}

		public static void log(string data, int type)
		{
			Console.ForegroundColor = colors[type];
			Console.Write("\n{0}: ", data);
			Console.ForegroundColor = ConsoleColor.White;
		}

		public static int RandomNum()
		{
			Random rnd = new Random();
			int colorNum = rnd.Next(1, 5);

			return colorNum;
		}
	}
}

