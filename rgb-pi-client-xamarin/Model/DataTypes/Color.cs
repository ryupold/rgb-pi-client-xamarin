using System;
using System.Collections.Generic;
using System.Globalization;

namespace RGBPi.Core.Model.DataTypes
{
	public struct Color
	{
		public readonly float R, G, B;
		public readonly byte Address;
		private readonly string type;
		private readonly float[] randomRGB;

		#region ctors

		public Color (float r, float g, float b) : this (r, g, b, 0xF)
		{
		}

		public Color (float r, float g, float b, byte address)
		{
			type = "f";
			Address = address;
			R = r;
			G = g;
			B = b;
			randomRGB = new float[6];
		}

		public Color (int argb)
			: this (((argb >> 16) & 0xFF) / 255f,
				((argb >> 8) & 0xFF)/255f,
				((argb) & 0xFF)/255f,
			        0xF
			)
		{
		}

		/// <summary>
		/// Creates a random color
		/// </summary>
		/// <param name="rndFromRed">Random from red.</param>
		/// <param name="rndToRed">Random to red.</param>
		/// <param name="rndFromGreen">Random from green.</param>
		/// <param name="rndToGreen">Random to green.</param>
		/// <param name="rndFromBlue">Random from blue.</param>
		/// <param name="rndToBlue">Random to blue.</param>
		public Color(float rndFromRed, float rndToRed, float rndFromGreen, float rndToGreen, float rndFromBlue, float rndToBlue){

			this.type = "r";
			this.Address = 0xF;
			this.randomRGB = new float[6];

			float fromRed  = rndFromRed;
			float toRed    = rndToRed;
			float fromGreen= rndFromGreen;
			float toGreen  = rndToGreen;
			float fromBlue = rndFromBlue;
			float toBlue   = rndToBlue;

			Random rnd = new Random (); 
			this.R = (float)rnd.NextDouble () * (toRed - fromRed) + fromRed;
			this.G = (float)rnd.NextDouble () * (toGreen - fromGreen) + fromGreen;
			this.B = (float)rnd.NextDouble () * (toBlue - fromBlue) + fromRed;

			this.randomRGB [0] = fromRed;
			this.randomRGB [1] = toRed;
			this.randomRGB [2] = fromGreen;
			this.randomRGB [3] = toGreen;
			this.randomRGB [4] = fromBlue;
			this.randomRGB [5] = toBlue;
		}

		/// <summary>
		/// Creates a color from a color string.
		/// </summary>
		/// <param name="colorString">Color string.</param>
		public Color (string colorString)
		{
			this.R = 0;
			this.G = 0;
			this.B = 0;
			this.Address = 0;
			this.randomRGB = new float[6];

			List<string> types = new List<string> { "x", "b", "f", "r", "hsv", "hsl" };
			string[] colorParts;


			if (colorString [0] != '{' || colorString [colorString.Length - 1] != '}')
				throw new ArgumentException ("color must defined within {} brackets" + colorString);

			colorString = colorString.Substring (1, colorString.Length - 2);
			colorParts = colorString.Split (':');


			if (!types.Contains (colorParts [0]))
				throw new ArgumentException ("unknown color type: " + colorParts [0]);

			type = colorParts [0];

			// extracting Address
			if (colorParts.Length > 2)
				this.Address = byte.Parse (colorParts [2], System.Globalization.NumberStyles.HexNumber);
			else if (colorParts.Length <= 2)
				this.Address = 0xF;

			// extracting RGB
			if (type == "x") {
				int rgbcomps = int.Parse (colorParts [1], System.Globalization.NumberStyles.HexNumber);
				this.R = (rgbcomps >> 16) / 255f;
				this.G = ((rgbcomps & 0xFF) >> 8) / 255f;
				this.B = (rgbcomps & 0xFF) / 255f;
			}
			if (type == "b") {
				string[] rgbcomps = colorParts [1].Split (',');
				this.R = int.Parse (rgbcomps [0]) / 255f;
				this.G = int.Parse (rgbcomps [1]) / 255f;
				this.B = int.Parse (rgbcomps [2]) / 255f;
			}

			if (type == "f") {
				string[] rgbcomps = colorParts [1].Split (',');
				this.R = float.Parse (rgbcomps [0]);
				this.G = float.Parse (rgbcomps [1]);
				this.B = float.Parse (rgbcomps [2]);
			}

			if (type == "r") {
				string[] rndValues = colorParts [1].Split (',');

				float fromRed = float.Parse (rndValues [0].Split ('-') [0]);
				float toRed = float.Parse (rndValues [0].Split ('-') [1]);
				float fromGreen = float.Parse (rndValues [1].Split ('-') [0]);
				float toGreen = float.Parse (rndValues [1].Split ('-') [1]);
				float fromBlue = float.Parse (rndValues [2].Split ('-') [0]);
				float toBlue = float.Parse (rndValues [2].Split ('-') [1]);

				Random rnd = new Random (); 
				this.R = (float)rnd.NextDouble () * (toRed - fromRed) + fromRed;
				this.G = (float)rnd.NextDouble () * (toGreen - fromGreen) + fromGreen;
				this.B = (float)rnd.NextDouble () * (toBlue - fromBlue) + fromRed;

				this.randomRGB [0] = fromRed;
				this.randomRGB [1] = toRed;
				this.randomRGB [2] = fromGreen;
				this.randomRGB [3] = toGreen;
				this.randomRGB [4] = fromBlue;
				this.randomRGB [5] = toBlue;
			}
			if (type == "hsv") {
				string[] hsvcomps = colorParts [1].Split (',');
				float h = float.Parse (hsvcomps [0]);
				float s = float.Parse (hsvcomps [1]);
				float v = float.Parse (hsvcomps [2]);

				Color c = FromHSV (h, s, v);
				this.R = c.R;
				this.G = c.G;
				this.B = c.B;
			}
				
			if (type == "hsl") {
				string[] hslcomps = colorParts [1].Split (',');
				float h = float.Parse (hslcomps [0]);
				float s = float.Parse (hslcomps [1]);
				float l = float.Parse (hslcomps [2]);

				throw new NotImplementedException ("TODO: C# HSL implementation");
				////TODO: C# HSL implementation
				//Color c = FromHSL (h, s, l);
				//this.R = c.R;
				//this.G = c.G;
				//this.B = c.B;
			}
			


			this.R = Math.Max (Math.Min (this.R, 1), 0);
			this.G = Math.Max (Math.Min (this.G, 1), 0);
			this.B = Math.Max (Math.Min (this.B, 1), 0);
		}

		/// <summary>
		/// Create Color object from HSV values
		/// </summary>
		/// <returns>Color</returns>
		/// <param name="h">Hue (0 - 360)</param>
		/// <param name="S">Saturation (0 - 100)</param>
		/// <param name="V">Value (0 - 100)</param>
		public static Color FromHSV (float h, float S, float V)
		{    
			double H = h;
			while (H < 0) {
				H += 360;
			}
			;
			while (H >= 360) {
				H -= 360;
			}
			;
			double R, G, B;
			if (V <= 0) {
				R = G = B = 0;
			} else if (S <= 0) {
				R = G = B = V;
			} else {
				double hf = H / 60.0;
				int i = (int)Math.Floor (hf);
				double f = hf - i;
				double pv = V * (1 - S);
				double qv = V * (1 - S * f);
				double tv = V * (1 - S * (1 - f));
				switch (i) {

				// Red is the dominant color

				case 0:
					R = V;
					G = tv;
					B = pv;
					break;

				// Green is the dominant color

				case 1:
					R = qv;
					G = V;
					B = pv;
					break;
				case 2:
					R = pv;
					G = V;
					B = tv;
					break;

				// Blue is the dominant color

				case 3:
					R = pv;
					G = qv;
					B = V;
					break;
				case 4:
					R = tv;
					G = pv;
					B = V;
					break;

				// Red is the dominant color

				case 5:
					R = V;
					G = pv;
					B = qv;
					break;

				// Just in case we overshoot on our math by a little, we put these here. Since its a switch it won't slow us down at all to put these here.

				case 6:
					R = V;
					G = tv;
					B = pv;
					break;
				case -1:
					R = V;
					G = pv;
					B = qv;
					break;

				// The color is not defined, we should throw an error.

				default:
					//LFATAL("i Value error in Pixel conversion, Value is %d", i);
					R = G = B = V; // Just pretend its black/white
					break;
				}
			}

			return new Color ((float)R, (float)G, (float)B);
		}

		#endregion ctors

		#region Properties
		public bool IsRandom{get{ return type == "r";}}
		#endregion Properties

		#region Conversion
		public static implicit operator int (Color color)
		{
			int r = (int)(color.R*255);
			int g = (int)(color.G*255);
			int b = (int)(color.B*255);
			return 0xff << 24 + r << 16 + g << 8 + b;
		}

		public static implicit operator string (Color color)
		{
			return color.ToString ();
		}

		public static implicit operator Color (int argb)
		{
			return new Color (argb);
		}

		public static implicit operator Color (string str)
		{
			return new Color (str);
		}

		public override string ToString ()
		{
			string colorString = "{";

			if (IsRandom) {
				colorString += "r:" + randomRGB [0].ToString(CultureInfo.InvariantCulture) + "-" + randomRGB [1].ToString(CultureInfo.InvariantCulture) + 
					"," + randomRGB [2].ToString(CultureInfo.InvariantCulture) + "-" + randomRGB [3].ToString(CultureInfo.InvariantCulture)
					+ "," + randomRGB [4].ToString(CultureInfo.InvariantCulture) + "-" + randomRGB [5].ToString(CultureInfo.InvariantCulture);
			} else {
				colorString += "f:"+R.ToString(CultureInfo.InvariantCulture)+","+G.ToString(CultureInfo.InvariantCulture)+","+B.ToString(CultureInfo.InvariantCulture);
			}

			colorString += "}";
			return colorString;
		}

		#endregion Conversion
	}
}

