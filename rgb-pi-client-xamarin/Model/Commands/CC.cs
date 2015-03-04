using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class CC : Command
	{
		public CC(Color c){
			this.color = c;
		}

		public CC(Color c, string op){
			this.color = c;
			this.op = op;
		}

		public string type = "cc";
		public string color;

		[JsonProperty("operator")]
		public string op = null;
	}
}

