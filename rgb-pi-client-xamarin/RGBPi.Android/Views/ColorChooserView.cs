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
using Android.Widget;

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
			View layout = FindViewById (Resource.Id.color_chooser_view);

			SVBar sv = (SVBar)FindViewById (Resource.Id.svBar);
			sv.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.addSVBar (sv);
			ColorChanged += (s, c) => {
				ViewModel.SetCurrentColor(c);
				global::Android.Graphics.Color cd = new global::Android.Graphics.Color((int)ViewModel.BackgroundColor);
				layout.SetBackgroundColor(cd);
				sv.SetBackgroundColor (cd);
				colorPicker.SetBackgroundColor (cd);
			};
		}
	}
}