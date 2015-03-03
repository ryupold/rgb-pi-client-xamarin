using System;
using RGBPi.Core.Model;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace RGBPi.Core.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class Answer
	{
		[DataMember]
		public int commands;

		[DataMember]
		public int filters;

//		[DataMember]
//		public RequestAnswer requests;

		[DataMember]
		public int triggers;

		[DataMember]
		public string[] error;
	}
}

