#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace WSquareServer
{

	static class Program
    {
		private static WSquare game;

		/// <summary>
		/// Grants access to main game instance to other classes
		/// </summary>
		public static WSquare Game
		{
			get{return game;}
		}

		internal static void RunGame ()
		{
			game = new WSquare ();
			game.Run ();
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
        [STAThread]
		static void Main (string[] args)
		{

			RunGame ();

		}


	}


}

