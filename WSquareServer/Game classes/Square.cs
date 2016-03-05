using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Linq;

using Personal.Collections.Generic;
using Personal.IO;

namespace WSquareServer
{
	public class Square
	{

		public int WindowHeight{get;set;}
		public int WindowWidth{get;set;}
		public Texture2D Texture { get; set; }
		public int SquareSize { get; set; }
		public int SquareWidth { get; set; }
		public int SquareHeight { get; set; }
		public int x_square{ get; set;}
		public int y_square{ get; set;}
		public int marginx{ get; set;}
		public int marginy{ get; set;}
		public int DirectionX{ get; set;}
		public int DirectionY{ get; set;}
		public Rectangle square;
		public RingBuffer<InputReport> Report;
		Task<UdpReceiveResult> udpresults;
		private IPEndPoint remoteEP;
		double latency;
		MovingAverage avg;
		private RingBuffer<InputReport> datasendBuffer;
		private RingBuffer<InputReport> datareceivedBuffer;
		private RingBuffer<Point> recentLocations;

		private Queue<InputReport> userInputs;
		private int updates=0;
		private int nupdates = 0;
		private double time=0;
		private List<Point> skipedlocations;


		public Square (Texture2D texture)
		{


			Texture=texture;
			WindowWidth=Program.Game.Window.ClientBounds.Width;
			WindowHeight=Program.Game.Window.ClientBounds.Height;
			SquareSize=15;
			SquareWidth=15;
			SquareHeight=15;
			marginx=WindowWidth- SquareSize;
			marginy=WindowHeight-SquareSize;
			DirectionX = 1;
			DirectionY = 1;
			square = new Rectangle(315,240,SquareSize,SquareSize);
			this.Report = new RingBuffer<InputReport> (20);
			Console.WriteLine (marginx.ToString());
			Console.WriteLine (marginy.ToString());
			udpresults = null;
			remoteEP = new IPEndPoint (IPAddress.Parse("192.168.0.100"),5010);
			avg = new MovingAverage (100);
			datasendBuffer = new RingBuffer<InputReport> (100);
			datareceivedBuffer = new RingBuffer<InputReport> (100);
			userInputs = new Queue<InputReport> (512);
			recentLocations = new RingBuffer<Point> (2);
			skipedlocations = new List<Point> ();


	
			
		}


		/// <summary>
		/// Gets the network buffer from UdpSocket
		/// </summary>
		/// <returns>The network buffer.</returns>
		public UdpReceiveResult[] GetNetworkBuffer()
		{

			List<UdpReceiveResult> list = new List<UdpReceiveResult> ();

			while (Program.UdpSocket.Available > 0) 
			{
				list.Add (Program.UdpSocket.ReceiveAsync ().Result);
			}

			return list.ToArray ();
		}

		/// <summary>
		/// Takes an array of UdpReceiveResult and deserialize each element into InputReport 
		/// </summary>
		/// <returns>InputReport[]</returns>
		public InputReport[] GetNetworkData(UdpReceiveResult[] udpbuffer)
		{

			List<InputReport> inputs = new List<InputReport> ();

			foreach(UdpReceiveResult result in udpbuffer)
			{
				InputReport report = SerializationHelper.DeserializeObject<InputReport> (result.Buffer) as InputReport;
				inputs.Add (report);
				

				latency = DateTime.Now.TimeOfDay.TotalMilliseconds - report.clienttime;
				avg.Add ((float)latency);
				//Console.WriteLine (avg.GetAverage().ToString());

			}

			return inputs.ToArray ();

		}



		public void Update()
		{
	

			//Get raw data from socket
			UdpReceiveResult[] udpreceive = this.GetNetworkBuffer ();

			//Convert raw data into InputReport objects
			InputReport[] newreports = this.GetNetworkData (udpreceive);

			for(int i=0; i<newreports.Length;i++)
			{

				if(userInputs.Count>0)
				{
				double newreport = newreports [i].clienttime;
				double lastreport = userInputs.Peek ().clienttime;
					if((newreport-lastreport)>16f)
					{
						//Console.WriteLine (DateTime.Now.TimeOfDay.ToString()+" : "+(newreport-lastreport).ToString());
					}
				}
				userInputs.Enqueue (newreports[i]);

			}

			InputReport report;
			bool nodata = (userInputs.Count == 0) ? true : false;
			int userinputs = userInputs.Count;

			while(userInputs.Count>0)
			{

				report = userInputs.Dequeue ();

				square.X = report.LocationX + 1 * report.DirectionX;
				square.Y = report.LocationY + 1 * report.DirectionY;
				recentLocations.Enqueue (square.Location);
				skipedlocations.Add (square.Location);
				//Reply report with calculated data

			}

			if(updates>1)
			{

				int order = skipedlocations.Count ();
				int locationx = 0;
				int locationy = 0;

				foreach (Point point in skipedlocations) 
				{
					locationx += point.X;
					locationy += point.Y;
				}

//				square.X = (int)((float)locationx / order);
//				square.Y = (int)((float)locationy / order);
				square.X = (int)Math.Floor ((float)locationx / order);
				square.Y = (int)Math.Floor ((float)locationy / order);
//				square.X = skipedlocations [skipedlocations.Count - 1].X;
//				square.Y = skipedlocations [skipedlocations.Count - 1].Y;


				
				skipedlocations.Clear ();

			}

			updates = 0;

//			if (recentLocations.Count>0) 
//			{
//				int order = recentLocations.Count ();
//				int locationx = 0;
//				int locationy = 0;
//				foreach (Point point in recentLocations) {
//					locationx += point.X;
//					locationy += point.Y;
//				}
//
//				square.X = (int)Math.Floor ((float)locationx / order);
//				square.Y = (int)Math.Floor ((float)locationy / order);
//			} 


			
//			if (this.updates > 1) {
//				Console.WriteLine ((DateTime.Now.TimeOfDay.TotalMilliseconds - time).ToString () + " : " + this.updates.ToString () +" : "  + nodata.ToString ());
//				time = DateTime.Now.TimeOfDay.TotalMilliseconds;
//			} else 
//			{
//				Console.WriteLine (this.updates.ToString () +" : "+ userinputs.ToString() +" : "+ nodata.ToString ());
//			}

			this.updates++;
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			this.updates = (updates > 1) ? updates : 0;
			this.updates = 0;
			spriteBatch.Draw(Texture,square,Color.Yellow);
		}


	}
}

