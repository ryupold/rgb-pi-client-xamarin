
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

namespace RGBPi.Android
{
	public class ColorDialog : MvxActivity
	{
		public class ColorDialog : MvxActivity
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
				SetContentView (Resource.Layout.ColorDialogView);
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
}

