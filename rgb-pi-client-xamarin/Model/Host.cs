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

		public Host(){
			name = string.Empty;
			ip = string.Empty;
			port = 0;
		}

		public Host(string name, string ip, int port){
			this.name = name;
			this.ip = ip;
			this.port = port;
		}

		public bool IsValid{
			get{
				return !string.IsNullOrWhiteSpace (name) && !string.IsNullOrWhiteSpace (ip) && port > 0;
			}
		}

		public override bool Equals (object obj)
		{
			if(obj != null && obj is Host){
				Host other = (Host)obj;
				return other.name ==this.name || other.ip == this.ip && other.port == this.port;
			}

			return false;
		}

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString ()
		{
			return string.Format ("Host: {0} (IP={1}, Port={2})", name, ip, port);
		}
	}
}

