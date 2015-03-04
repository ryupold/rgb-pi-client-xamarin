using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class Loop : Command
	{
		public Loop(Condition condition, List<Command> commands){
			this.condition = condition;
			this.commands = commands;
		}

		public string type = "loop";
		public string condition;
		public List<Command> commands;
	}
}

