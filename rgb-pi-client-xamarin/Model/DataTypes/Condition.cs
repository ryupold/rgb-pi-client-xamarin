using System;
using System.Collections.Generic;
using System.Globalization;

namespace RGBPi.Core.Model.DataTypes
{
	public struct Condition
	{
		public bool condition;
		public float time;
		public int count;
		public string color;
		private string type;

		#region ctors

		public Condition (bool b)
		{
			this.type = "b";
			this.condition = b;
			this.count = 0;
			this.time = 0;
			this.color = null;
		}

		public Condition(int count){
			this.type = "i";
			this.condition = false;
			this.count = count;
			this.time = 0;
			this.color = null;
		}

		public Condition(float time){
			this.type = "t";
			this.condition = false;
			this.count = 0;
			this.time = time;
			this.color = null;
		}

		public Condition(Color color){
			this.type = "c";
			this.condition = false;
			this.count = 0;
			this.time = 0;
			this.color = color;
		}

		/// <summary>
		/// Creates a Time object from a time-string.
		/// </summary>
		/// <param name="timeString">Time string.</param>
		public Condition (string conditionString)
		{
			this.condition = false;
			this.count = 0;
			this.time = 0;
			this.color = null;

			List<string> types = new List<string> {"b", "t", "i", "c"};
			string[] conditionParts;

			if (conditionString [0] == '{' && conditionString [conditionString.Length - 1] == '}') {

				conditionString = conditionString.Substring (1, conditionString.Length - 2);
				conditionParts = conditionString.Split (':');

				if (!types.Contains (conditionParts [0]))
					throw new ArgumentException ("unknown condition type: " + conditionParts [0]);

				type = conditionParts [0];

				// constant bool
				if (type == "b") {
					if (!bool.TryParse (conditionParts [1], out this.condition)) {
						throw new ArgumentException ("condition must be a constant value or defined within {} brackets" + conditionString);
					}
				}

				//time
				if (type == "t")
				{
					float.TryParse (conditionParts [1], NumberStyles.Float, CultureInfo.InvariantCulture, out this.time);
				}
				//count
				if (type == "i")
				{
					int.TryParse (conditionParts [1], out this.count);
				}
				//color
				if (type == "c")
				{
					bool.TryParse (conditionParts [1], out this.condition);
				}

			} else {
				type = "b";
				if (!bool.TryParse (conditionString, out this.condition)) {
					throw new ArgumentException ("condtion must be a constant value or defined within {} brackets" + conditionString);
				}
			}
		}

		#endregion ctors

		#region Properties


		#endregion Properties

		#region Conversion

		public static implicit operator bool (Condition cond)
		{
			return cond.condition;
		}

		public static implicit operator string (Condition cond)
		{
			return cond.ToString ();
		}

		public static implicit operator Condition (bool b)
		{
			return new Condition (b);
		}

		public static implicit operator Condition (string str)
		{
			return new Condition(str);
		}

		public override string ToString ()
		{
			string colorString = "{";

			if (type == "b")
			{
				colorString += "b:" + (this.condition ? 1 : 0);
			} 
			else if(type == "t") 
			{
				colorString += "t:" + this.time.ToString (CultureInfo.InvariantCulture);
			}
			else if(type == "i") 
			{
				colorString += "i:" + this.count;
			}
			else if(type == "c") 
			{
				colorString += "c:" + this.color;
			}

			colorString += "}";
			return colorString;
		}

		#endregion Conversion
	}
}

