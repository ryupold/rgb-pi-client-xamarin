using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using RGBPi.Core.ViewModels;
using System;
using RGBPi.Android.Views.HolorColorPicker;
using RGBPi.Core.Model;
using RGBPi.Core.Model.Commands;
using System.Collections.Generic;

namespace RGBPi.Android
{
    [Activity(
		Label = "RGB-Pi Remote"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class ColorChooserView : MvxActivity
    {
		public new ColorChooserViewModel ViewModel
		{
			get { return (ColorChooserViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnViewModelSet()
		{
			base.OnViewModelSet();
			SetContentView(Resource.Layout.ColorChooserView);
			ColorPicker cp = (ColorPicker)FindViewById (Resource.Id.colorPicker);
			SVBar sv = (SVBar)FindViewById (Resource.Id.svBar);
			cp.addSVBar (sv);

			ViewModel.OnResponse += (s, cmd) => {

			};

			cp.ColorChanged += (s, newColor) => {
				RGBPi.Core.Model.DataTypes.Color c = ViewModel.CurrentColor = new RGBPi.Core.Model.DataTypes.Color(newColor);

				Message msg = new Message();
				CC cc = new CC();
				cc.color = c;
				msg.commands = new List<RGBPi.Core.Model.Commands.Command>{
					cc
				};
				ViewModel.SendCommandString(msg);
			};
		}
    }
}