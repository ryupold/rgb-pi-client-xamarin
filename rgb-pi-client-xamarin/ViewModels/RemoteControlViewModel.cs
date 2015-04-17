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

		public RemoteControlViewModel ()
		{
			socket = Mvx.Resolve<ISocket> ();
		}

		public void SendMessage(Message command, Action<Answer> answerCallback=null){
			socket.Send (command, answerCallback);
		}
	}
}

