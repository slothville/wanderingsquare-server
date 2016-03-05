using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;

namespace Personal.Net.Sockets
{
	public static class SocketExtensions
	{
	
		public static Task<Socket> AcceptTask(this Socket s)
		{
			return Task<Socket>.Factory.FromAsync (s.BeginAccept, s.EndAccept, null);
		}
			

		public static Task<int> ReceiveTask(this Socket h, byte[] buffer)
		{

			return Task<int>.Factory.FromAsync (h.BeginReceive(buffer, 0,buffer.Length, 0, null, null), h.EndReceive);

		}

		public static Task<int> SendTask(this Socket h,byte[] buffer)
		{
			return Task<int>.Factory.FromAsync (h.BeginSend(buffer, 0,buffer.Length, 0, null, null), h.EndSend);
		}

		public static Task<Socket> AcceptTaskv2(this Socket socket)
		{
			var tcs = new TaskCompletionSource<Socket> (socket);

			socket.BeginAccept (iar=>
				{
				var t =(TaskCompletionSource<Socket>)iar.AsyncState;
				var s = (Socket)t.Task.AsyncState;
				
					try
					{
						t.SetResult(s.EndAccept(iar));
					}
					catch(Exception exc)
					{
						t.SetException(exc);
					}

				},tcs);

			return tcs.Task;
		}

		public static Task<int> SendTaskv2(this Socket socket,byte[] buffer)
		{
			var tcs = new TaskCompletionSource<int> (socket);

			socket.BeginSend (buffer, 0, buffer.Length, 0, iar => 
				{
					var t = (TaskCompletionSource<int>)iar.AsyncState;
					var s = (Socket)t.Task.AsyncState;

					try
					{
						t.SetResult(s.EndSend(iar));
					}
					catch(Exception exc)
					{
						t.SetException(exc);
					}


			}, tcs);

			return tcs.Task;

		}

		public static Task<int> ReceiveTaskv2(this Socket socket, byte[] buffer)
		{
			var tcs = new TaskCompletionSource<int>(socket);

			socket.BeginReceive(buffer, 0, buffer.Length, 0, iar =>
				{
					var t = (TaskCompletionSource<int>)iar.AsyncState;
					var s = (Socket)t.Task.AsyncState;

					try 
					{ 
						t.SetResult(s.EndReceive(iar));
					}
					catch (Exception exc) 
					{ 
						t.SetException(exc); 
					}

				}, tcs);

			return tcs.Task;
		}

		public static Task ConnectTask(this Socket socket, EndPoint remoteEP)
		{
			return Task.Factory.FromAsync (socket.BeginConnect(remoteEP,null,null),socket.EndConnect);
		} 
			


		public static uint PendingData(Socket s)
		{
			byte[] outValue = BitConverter.GetBytes(0);

			// Check how many bytes have been received.
			s.IOControl(0x4004667F, null, outValue);

			return  BitConverter.ToUInt32(outValue, 0);


	
		}	

		private static bool isAlive(this Socket handler)
		{

			try
			{
				return !(handler.Poll(1000, SelectMode.SelectRead) && handler.Available == 0);
			}
			catch (SocketException) 
			{ 
				return false; 
			}

		}
			
			
	}
}

