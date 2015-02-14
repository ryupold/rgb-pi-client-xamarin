using System;
using System.Threading.Tasks;

namespace RGBPi.Core
{
	public interface ISocket
	{
		Task<string> Send (string command);
	}
}

