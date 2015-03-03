using System;
using RGBPi.Core;
using System.Threading.Tasks;
using System.Collections.Generic;
using Java.Net;
using System.Threading;
using RGBPi.Core.Model;
using System.IO;
using System.Text;

namespace RGBPi.Android
{
	public class Socket : ISocket
	{
		private Java.Net.Socket socket;

		#region implemented abstract members of ISocket
		protected override bool ConnectNative (Host host)
		{
			return ConnectNative (host.ip, host.port);
		}

		protected override bool ConnectNative (string ip, int port)
		{
			if (socket == null)
				socket = new Java.Net.Socket (ip, port);

			return true;
		}

		protected override bool SendNative (string clientMessage)
		{
			byte[] stringAsBytes = System.Text.Encoding.UTF8.GetBytes (clientMessage);
			socket.OutputStream.Write (stringAsBytes, 0, stringAsBytes.Length);
			return true;
		}

		protected override string ReceiveNative ()
		{
			using (MemoryStream ms = new MemoryStream ()) {
				byte[] buffer = new byte[1024];
				int read = -1;
				while ((read = socket.InputStream.Read (buffer, 0, buffer.Length)) > 0) {
					ms.Write (buffer, 0, read);
				}

				return Encoding.UTF8.GetString(ms.ToArray ());
			}
		}
		protected override void CloseNative ()
		{
			if (socket != null) {
				socket.Close ();
				socket = null;
			}
		}
		#endregion implemented abstract members of ISocket
	}
}

