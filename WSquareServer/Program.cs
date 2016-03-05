#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
#endregion

namespace WSquareServer
{

	static class Program
    {
		private static WSquare game;
		private static UdpClient udpsocket;

		/// <summary>
		/// Grants access to main game instance to other classes
		/// </summary>
		public static WSquare Game
		{
			get{return game;}
		}

		/// <summary>
		/// Grants access to the receive socket instance to other classes
		/// </summary>
		public static UdpClient UdpSocket
		{
			get{ return udpsocket;}
		}

		internal static void RunGame ()
		{
			//udpsocket linked to any local interface at specified port
			udpsocket = new UdpClient (new IPEndPoint(IPAddress.Any,5002));

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

