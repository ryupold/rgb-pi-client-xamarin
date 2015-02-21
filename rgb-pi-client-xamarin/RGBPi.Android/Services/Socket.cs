using System;
using RGBPi.Core;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RGBPi.Android
{
	public class Socket : ISocket
	{
		private Java.Net.Socket socket = new Java.Net.Socket();
		private Queue<string> commandQ;
		private bool stop = false;
		private Task task;
		private Action worker = () => {
//			bool connected = false;
//			while(!stop){
//				//really?
//			}
		};


		public Socket ()
		{
			//task = Task.Factory.StartNew (worker);
		}

		public async Task<string> Send (string command)
		{
			byte[] stringAsBytes = System.Text.Encoding.UTF8.GetBytes (command);
			await socket.OutputStream.WriteAsync(stringAsBytes, 0, stringAsBytes.Length);
			byte[] buffer = new byte[1024];
			string response = "";
			int read = -1;
			while((read = await socket.InputStream.ReadAsync (buffer, 0, buffer.Length)) > 0){
				response += System.Text.Encoding.UTF8.GetString (buffer, 0, read);
			}
			return response;
		}

		private async Task<string> SendCommand (string command)
		{
			byte[] stringAsBytes = System.Text.Encoding.UTF8.GetBytes (command);
			await socket.OutputStream.WriteAsync(stringAsBytes, 0, stringAsBytes.Length);
			byte[] buffer = new byte[1024];
			string response = "";
			int read = -1;
			while((read = await socket.InputStream.ReadAsync (buffer, 0, buffer.Length)) > 0){
				response += System.Text.Encoding.UTF8.GetString (buffer, 0, read);
			}
			return response;
		}
	}
}

