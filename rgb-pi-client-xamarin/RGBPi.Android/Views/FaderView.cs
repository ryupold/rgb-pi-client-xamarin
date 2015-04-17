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
using RGBPi.Core;
using RGBPi.Core.Model.DataTypes;
using Cirrious.MvvmCross.Binding.Droid.Views;

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

			var btn = FindViewById<ImageView> (Resource.Id.btn_add);
			btn.Click += (sender, e) => {
				RunOnUiThread(()=>ShowDialog(1));
			};

			//error toast
			ViewModel.OnResponse += (sender, answer) => {
				if(answer.error != null && answer.error.Length > 0){
					string errorString = "";
					foreach (var e in answer.error) {
						errorString += e+" \n";
					}
					Toast.MakeText(this, errorString, ToastLength.Long).Show();
				}
			};
		}

		protected override Dialog OnCreateDialog (int id)
		{
			Console.WriteLine ("Opening Color Dialog ("+id+")");

			AlertDialog.Builder builder = new AlertDialog.Builder (this);
			LayoutInflater inflater = LayoutInflater.FromContext(this);

			ViewGroup layout = (ViewGroup)inflater.Inflate (Resource.Layout.ColorDialogView, null);
			builder.SetView (layout);

			var colorPicker = layout.FindViewById<ColorPicker> (Resource.Id.color_picker);

			SVBar sv = layout.FindViewById<SVBar> (Resource.Id.sv_bar);
			var cb = layout.FindViewById<CheckBox> (Resource.Id.chk_random);
			sv.HorizontalOrientation = false;
			sv.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.SetBackgroundColor (global::Android.Graphics.Color.Transparent);
			colorPicker.addSVBar (sv);
			colorPicker.ColorChanged += (s, c) => {
				global::Android.Graphics.Color cd = new global::Android.Graphics.Color ((int)Util.MakePastel(c));
				layout.SetBackgroundColor (cd);
				sv.SetBackgroundColor (cd);
				colorPicker.SetBackgroundColor (cd);
			};

			AlertDialog d = builder.Create ();
			d.KeyPress += (sender, e) => {
				Console.WriteLine(e.KeyCode + " "+e.Handled);
				d.Dismiss();
			};
			d.SetButton ("add", (sender, e) => {
				Console.WriteLine(e.Which);
				if(cb.Checked){
					ViewModel.AddColor(new Color(0, 1, 0, 1, 0, 1));
				}
				else{
					ViewModel.AddColor(colorPicker.CurrentColor);
				}
			});
			d.SetButton2 ("cancel", (sender, e) => {
				Console.WriteLine(e.Which);
				d.Dismiss();
			}); 
			return d;
		}
	}
}