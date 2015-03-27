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

			Button btn = FindViewById<Button> (Resource.Id.button1);
			btn.Click += (sender, e) => {
				ShowDialog(1);
			};
		}

		protected override Dialog OnCreateDialog (int id)
		{
			Console.WriteLine ("Opening Color Dialog");

			AlertDialog.Builder builder = new AlertDialog.Builder (Application.Context);
			LayoutInflater inflater = LayoutInflater.FromContext(Application.Context);

			ViewGroup layout = (ViewGroup)inflater.Inflate (Resource.Layout.ColorDialogView, null);
			builder.SetView (layout);

			var colorPicker = layout.FindViewById<ColorPicker> (Resource.Id.color_picker);

			SVBar sv = layout.FindViewById<SVBar> (Resource.Id.sv_bar);
			sv.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.addSVBar (sv);
//			ColorChanged += (s, c) => {
//				ViewModel.SetCurrentColor (c);
//				global::Android.Graphics.Color cd = new global::Android.Graphics.Color ((int)ViewModel.BackgroundColor);
//				layout.SetBackgroundColor (cd);
//				sv.SetBackgroundColor (cd);
//				colorPicker.SetBackgroundColor (cd);
//			};

			return builder.Create ();
		}
	}
}