using System;
using RGBPi.Core.Model.DataTypes;

namespace RGBPi.Core
{
	public abstract class Util
	{
		private Util (){}

		public static Color MakePastel(Color col){
			return new Color (
				MakePastel(col.R),
				MakePastel(col.G),
				MakePastel(col.B)
			);
		}

		public static float MakePastel(float col){
			return Math.Max(col * 0.5f, 0.3f);
		}
	}
}

