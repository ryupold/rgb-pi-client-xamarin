
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using Cirrious.CrossCore;
using Cirrious.MvvmCross.Droid.Views;
using RGBPi.Android.Views.HolorColorPicker;
using RGBPi.Core.ViewModels;
using Cirrious.MvvmCross.Dialog.Droid.Views;

namespace RGBPi.Android
{
	public class ColorDialog : MvxDialogActivity
	{
		private ColorPicker colorPicker;

		public EventHandler<int> ColorChanged{ get { return colorPicker.ColorChanged; } set { colorPicker.ColorChanged = value; } }

		public new ColorDialogViewModel ViewModel {
			get { return (ColorDialogViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

		}

		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet ();
			SetContentView (Resource.Layout.ColorDialogView);
			colorPicker = (ColorPicker)FindViewById (Resource.Id.colorPicker);
			View layout = FindViewById (Resource.Id.color_chooser_view);
			
			SVBar sv = (SVBar)FindViewById (Resource.Id.svBar);
			sv.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.addSVBar (sv);
			ColorChanged += (s, c) => {
				ViewModel.SetCurrentColor (c);
				global::Android.Graphics.Color cd = new global::Android.Graphics.Color ((int)ViewModel.BackgroundColor);
				layout.SetBackgroundColor (cd);
				sv.SetBackgroundColor (cd);
				colorPicker.SetBackgroundColor (cd);
			};
		}

		//		protected override Dialog OnCreateDialog (int id)
		//		{
		//			Console.WriteLine ("Opening Color Dialog");
		//
		//
		//
		//			AlertDialog.Builder builder = new AlertDialog.Builder (Application.Context);
		//			LayoutInflater inflater = LayoutInflater.FromContext(Application.Context);
		//
		//			ViewGroup layout = (ViewGroup)inflater.Inflate (Resource.Layout.ColorDialogView, null);
		//			builder.SetView (layout);
		//
		//			colorPicker = layout.FindViewById<ColorPicker> (Resource.Id.color_picker);
		//
		//			SVBar sv = layout.FindViewById<SVBar> (Resource.Id.sv_bar);
		//			sv.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
		//			colorPicker.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
		//			colorPicker.addSVBar (sv);
		//			ColorChanged += (s, c) => {
		//				ViewModel.SetCurrentColor (c);
		//				global::Android.Graphics.Color cd = new global::Android.Graphics.Color ((int)ViewModel.BackgroundColor);
		//				layout.SetBackgroundColor (cd);
		//				sv.SetBackgroundColor (cd);
		//				colorPicker.SetBackgroundColor (cd);
		//			};
		//
		//			return builder.Create ();
		//		}


		
	}
}

