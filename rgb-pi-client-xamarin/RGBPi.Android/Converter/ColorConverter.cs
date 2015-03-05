using System;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.UI;
using Android.Graphics.Drawables;
using Android.Graphics;

namespace RGBPi.Android
{
	public class ColorConverter : IMvxValueConverter
	{
		#region IMvxValueConverter implementation

		public object Convert (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			RGBPi.Core.Model.DataTypes.Color c = (RGBPi.Core.Model.DataTypes.Color)value;
			return new ColorDrawable(new Color(c)); 
		}

		public object ConvertBack (object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return new RGBPi.Core.Model.DataTypes.Color (((ColorDrawable)value).Color.ToArgb());
		}

		#endregion


	}
}

