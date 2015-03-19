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
	[Activity (Label = "Fader", ScreenOrientation = ScreenOrientation.Portrait)]
	public class FaderView : MvxActivity
	{
		public new FaderViewModel ViewModel {
			get { return (FaderViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
			

		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet();
			SetContentView (Resource.Layout.FaderView);
		}
	}
}