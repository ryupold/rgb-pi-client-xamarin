using System;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace RGBPi.Core
{
	public class RemoteControlViewModel : MvxViewModel
	{
		ISocket socket;

		public RemoteControlViewModel ()
		{
			socket = Mvx.Resolve<ISocket>();
		}

		public async Task<string> SendCommandString(string command){
			return await socket.Send (command);
		}
	}
}

