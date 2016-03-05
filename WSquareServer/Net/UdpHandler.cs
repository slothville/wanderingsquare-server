using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Concurrent;

namespace Personal.Net.Sockets
{
	public class UdpHandler
	{

		private UdpClient socket;
		private ConcurrentQueue<byte[]> dataReceived;
		private bool serviceRunning=false;

		/// <summary>
		/// Returns the number of bytes of the next element available in the udp handler buffer
		/// </summary>
		/// <value><c>0</c> if the buffer is empty.</value>
		public int Available
		{
			get{return dataReceived.Count;}
		}

		/// <summary>
		/// Gets a value indicating whether the buffer is empty.
		/// </summary>
		/// <value><c>true</c> if is empty; otherwise, <c>false</c>.</value>
		public bool isEmpty
		{
			get{return dataReceived.IsEmpty;}
		}

		public UdpHandler (IPAddress address, int port)
		{
			socket = new UdpClient (new IPEndPoint (address,port));
			dataReceived = new ConcurrentQueue<byte[]> ();
		}

		/// <summary>
		/// Gets the buffer.
		/// </summary>
		/// <returns>If the buffer is empty this method returns a zero lenght byte[]</returns>
		public byte[] GetBuffer()
		{
			byte[] data;

			if (!this.isEmpty) 
			{
				if (dataReceived.TryDequeue (out data)) {

					return data;
				}
				else 
				{
					return data = new byte[0];
				}

			} 
			else 
			{
				return data= new byte[0];
			}
		}

		public Task<int> SendAsync(byte[] datagram, int bytes, IPEndPoint endPoint)
		{
			return socket.SendAsync (datagram, bytes, endPoint);
		}

		public async Task<bool> RunService()
		{
			this.serviceRunning = true;

			try
			{

				while(serviceRunning)
				{
					var data = await this.socket.ReceiveAsync();

					this.dataReceived.Enqueue(data.Buffer);
				}

				return true;

			}
			catch(Exception)
			{
				return false;
			}


		}


	}
}

