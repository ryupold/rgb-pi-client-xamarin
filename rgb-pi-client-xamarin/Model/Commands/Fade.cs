using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class Fade : Command
	{
		public string type = "fade";
		public float time; 
		public String start;
		public string end;
	}
}

