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
		public List<Command> commands = null;
		public List<Filter> filters = null;
		public Request request = null;
		public List<Trigger> triggers = null;
	}
}

