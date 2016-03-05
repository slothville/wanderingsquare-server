using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace WSquareServer
{
	/// <summary>
	/// Encloses the basic state information of the object to be sent between server and client. 
	/// This includes current input, location, time (server and client) and packet id
	/// </summary>
	[Serializable]
	public class InputReport
	{

		public InputReport ()
		{

		}

		public int marginx{ get; set;}
		public int marginy{ get; set;}
		public int LocationX{ get; set;}
		public int LocationY{ get; set;}
		public int DirectionX{ get; set;}
		public int DirectionY{ get; set;}
		public double clienttime{ get; set;}
		public double servertime{ get; set;}
		public uint id{ get; set;}
	
	}
}

