using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class Fade : Command
	{
		public Fade(Time time, Color end){
			this.time = time;
			this.end = end;
		}

		public Fade(Time time, Color end, Color start){
			this.time = time;
			this.end = end;
			this.start = start;
		}

		public string type = "fade";
		public string time;
		public string start = null;
		public string end;
	}
}

