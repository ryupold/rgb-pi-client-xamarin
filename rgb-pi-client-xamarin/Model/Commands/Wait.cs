using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class Wait : Command
	{
		public Wait(Time time){
			this.time = time;
		}

		public string type = "wait";
		public string time;
	}
}

