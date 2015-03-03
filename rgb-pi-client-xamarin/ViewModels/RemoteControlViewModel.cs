using System;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using RGBPi.Core.Model;

namespace RGBPi.Core
{
	public abstract class RemoteControlViewModel : MvxViewModel
	{
		ISocket socket;
		public EventHandler<Answer> OnResponse { get{ return socket.OnResponse; } set{ socket.OnResponse = value; }}

		public RemoteControlViewModel ()
		{
			socket = Mvx.Resolve<ISocket> ();
		}

		public void SendCommandString(Message command){
			socket.Send (command);
		}
	}
}

