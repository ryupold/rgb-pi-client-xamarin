using System;
using System.Runtime.Serialization;
using RGBPi.Core.Model.Commands;
using RGBPi.Core.Model.Filters;
using RGBPi.Core.Model.Requests;
using RGBPi.Core.Model.Triggers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace RGBPi.Core.Model
{
	[JsonObject]
	public class Message
	{
		public Message(){}

		public Message(List<Command> commands, List<Filter> filters = null, Request request = null, List<Trigger> triggers = null){
			this.commands = commands;
			this.filters = filters;
			this.request = request;
			this.triggers = triggers;
		}

		public List<Command> commands = null;
		public List<Filter> filters = null;
		public Request request = null;
		public List<Trigger> triggers = null;
	}
}

