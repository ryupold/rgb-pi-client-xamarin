using System;
using System.Threading.Tasks;

namespace RGBPi.Core
{
	public class AbstractSocket : ISocket
	{
		public AbstractSocket ()
		{
		}

		public async Task<string> Send(string command){
			return "";
		}
	}
}

