using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class Nop : Command
	{
		public string type = "nop";
	}
}

