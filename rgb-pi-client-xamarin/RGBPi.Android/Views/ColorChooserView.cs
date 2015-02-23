using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using RGBPi.Core.ViewModels;
using System;
using RGBPi.Android.Views.HolorColorPicker;
using RGBPi.Core.Model;
using RGBPi.Core.Model.Commands;
using System.Collections.Generic;
using Android.Views;
using Cirrious.MvvmCross.Droid.Fragging.Fragments;
using Android.OS;
using Cirrious.MvvmCross.Droid.Fragging;
using Android.Content;
using Android.Util;

namespace RGBPi.Android
{
	[Activity (Label = "Color Chooser", ScreenOrientation = ScreenOrientation.Portrait)]
	public class ColorChooserView : MvxActivity
	{
		private ColorPicker colorPicker;

		public EventHandler<int> ColorChanged{ get { return colorPicker.ColorChanged; } set { colorPicker.ColorChanged = value; } }

		public new ColorChooserViewModel ViewModel {
			get { return (ColorChooserViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
			

		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet();
			SetContentView (Resource.Layout.ColorChooserView);
			colorPicker = (ColorPicker)FindViewById (Resource.Id.colorPicker);
			SVBar sv = (SVBar)FindViewById (Resource.Id.svBar);
			colorPicker.addSVBar (sv);
			colorPicker.ColorChanged += (s, newColor) => {
				RGBPi.Core.Model.DataTypes.Color c = ViewModel.CurrentColor = new RGBPi.Core.Model.DataTypes.Color (newColor);

				RGBPi.Core.Model.Message msg = new RGBPi.Core.Model.Message ();
				CC fade = new CC ();
				fade.color = c;
				msg.commands = new List<RGBPi.Core.Model.Commands.Command> {
					fade
				};
				ViewModel.SendCommandString (msg);
			};
		}
	}
}