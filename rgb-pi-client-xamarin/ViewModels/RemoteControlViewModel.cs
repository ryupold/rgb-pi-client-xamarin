using System;
using Cirrious.MvvmCross.ViewModels;
using System.Threading.Tasks;
using Cirrious.CrossCore;

namespace RGBPi.Core
{
	public abstract class RemoteControlViewModel : MvxViewModel
	{
		ISocket socket;

		public RemoteControlViewModel ()
		{

		}

		public async Task<string> SendCommandString(string command){
			return await socket.Send (command);
		}
	}
}

