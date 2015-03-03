using System;
using Newtonsoft.Json;

namespace RGBPi.Core
{
	[JsonObject]
	public class Host
	{
		public string name;
		public string ip;
		public int port;

		public Host(){}

		public Host(string name, string ip, int port){
			this.name = name;
			this.ip = ip;
			this.port = port;
		}

		public override bool Equals (object obj)
		{
			if(obj != null && obj is Host){
				Host other = (Host)obj;
				return other.name ==this.name || other.ip == this.ip && other.port == this.port;
			}

			return false;
		}
	}
}

