using System;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore;
using RGBPi.Core.Model;

namespace RGBPi.Core
{
	public abstract class RemoteControlViewModel : MvxViewModel
	{
		protected ISocket socket;
		protected ISettings settings;

		public RemoteControlViewModel ()
		{
			socket = Mvx.Resolve<ISocket> ();
			settings = Mvx.Resolve<ISettings> ();
		}

		public void SendMessage(Message command, Action<Answer> answerCallback=null){
			if (settings.ActiveHost != null) 
			{
				socket.Send (command, answerCallback);
			} else {
				Mvx.Resolve<IToaster> ().ToastString ("no active host. Go to settings and add one.");
			}
		}
	}
}

