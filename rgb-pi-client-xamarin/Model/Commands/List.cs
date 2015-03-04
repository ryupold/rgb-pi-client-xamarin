using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.DataTypes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RGBPi.Core.Model.Commands
{
	[JsonObject]
	public class List : Command
	{
		public List(List<Command> commands){
			this.commands = commands;
		}

		public string type = "list";
		public List<Command> commands;
	}
}

