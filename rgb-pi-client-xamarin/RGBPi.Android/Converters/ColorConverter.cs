using System;
using Cirrious.CrossCore.Converters;
using Cirrious.CrossCore.UI;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Diagnostics;
using Android.Util;
using Cirrious.MvvmCross.Plugins.Color;

namespace RGBPi.Android
{
	public class ColorConverter : MvxColorValueConverter
	{

		protected override MvxColor Convert (object value, object parameter, System.Globalization.CultureInfo culture)
		{
			return (MvxColor)value;
		}

//		protected override ColorDrawable Convert (RGBPi.Core.Model.DataTypes.Color value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//		{
//			return new ColorDrawable (new Color((int)value));
//		}
//
//		protected override RGBPi.Core.Model.DataTypes.Color ConvertBack (ColorDrawable value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
//		{
//			return new RGBPi.Core.Model.DataTypes.Color (value.Color.ToArgb());
//		}
//
		
	}
}

