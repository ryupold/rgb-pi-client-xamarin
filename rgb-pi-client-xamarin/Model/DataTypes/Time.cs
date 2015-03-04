using System;
using System.Collections.Generic;
using System.Globalization;

namespace RGBPi.Core.Model.DataTypes
{
	public struct Time
	{
		public readonly float time;
		private readonly string type;
		private readonly float[] randomTime;

		#region ctors

		public Time (float t)
		{
			this.type = "c";
			this.time = t;
			this.randomTime = new float[2];
		}

		/// <summary>
		/// Creates a random time object in the given interval
		/// </summary>
		/// <param name="rndMin">Random minimum.</param>
		/// <param name="rndMax">Random maximum.</param>
		public Time (float rndMin, float rndMax)
		{

			this.type = "r";
			this.randomTime = new float[]{ rndMin, rndMax };

			Random rnd = new Random (); 
			this.time = (float)rnd.NextDouble () * (rndMax - rndMin) + rndMin;
		}

		/// <summary>
		/// Creates a Time object from a time-string.
		/// </summary>
		/// <param name="timeString">Time string.</param>
		public Time (string timeString)
		{
			this.time = 0;
			this.randomTime = new float[2];

			List<string> types = new List<string> { "c", "r" };
			string[] timeParts;


			if (timeString [0] == '{' && timeString [timeString.Length - 1] == '}') {


				timeString = timeString.Substring (1, timeString.Length - 2);
				timeParts = timeString.Split (':');


				if (!types.Contains (timeParts [0]))
					throw new ArgumentException ("unknown color type: " + timeParts [0]);

				type = timeParts [0];

				// constant time
				if (type == "c") {
					if (!float.TryParse (timeParts [1], NumberStyles.Float, CultureInfo.InvariantCulture, out this.time)) {
						throw new ArgumentException ("time must be a constant value or defined within {} brackets" + timeString);
					}
				}
				//random time
				if (type == "r") {
					string[] rndValues = timeParts [1].Split (',');

					float min, max;
					float.TryParse (rndValues [0], NumberStyles.Float, CultureInfo.InvariantCulture, out min);
					float.TryParse (rndValues [1], NumberStyles.Float, CultureInfo.InvariantCulture, out max);

					Random rnd = new Random (); 
					this.time = (float)rnd.NextDouble () * (max - min) + min;

					this.randomTime = new float[]{ min, max };
				}			

				this.time = Math.Max (this.time, 0);

			} else {
				if (float.TryParse (timeString, NumberStyles.Float, CultureInfo.InvariantCulture, out this.time)) {
					this.randomTime = new float[2];
				} else {
					throw new ArgumentException ("time must be a constant value or defined within {} brackets" + timeString);
				}
			}
		}

		#endregion ctors

		#region Properties

		public bool IsRandom{ get { return type == "r"; } }

		#endregion Properties

		#region Conversion

		public static implicit operator float (Time time)
		{
			return time.time;
		}

		public static implicit operator string (Time time)
		{
			return time.ToString ();
		}

		public static implicit operator Time (float time)
		{
			return new Time (time);
		}

		public static implicit operator Time (string str)
		{
			return new Time (str);
		}

		public override string ToString ()
		{
			string colorString = "{";

			if (IsRandom) {
				colorString += "r:" + randomTime [0].ToString (CultureInfo.InvariantCulture) +
				"," + randomTime [1].ToString (CultureInfo.InvariantCulture);
			} else {
				colorString += "c:" + this.time.ToString (CultureInfo.InvariantCulture);
			}

			colorString += "}";
			return colorString;
		}

		#endregion Conversion
	}
}

