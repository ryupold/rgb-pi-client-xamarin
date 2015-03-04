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
	[Activity (Label = "Command Test", ScreenOrientation = ScreenOrientation.Portrait)]
	public class CommandTestView : MvxActivity
	{
		public new CommandTestViewModel ViewModel {
			get { return (CommandTestViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
			
		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet();
			SetContentView (Resource.Layout.CommandTestView);

		}
	}
}