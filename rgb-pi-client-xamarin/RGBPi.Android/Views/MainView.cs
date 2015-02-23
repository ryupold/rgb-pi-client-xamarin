using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using RGBPi.Core.ViewModels;
using System;
using RGBPi.Android.Views.HolorColorPicker;
using RGBPi.Core.Model;
using RGBPi.Core.Model.Commands;
using System.Collections.Generic;
using Android.Content;
using Android.Support.V7.Widget;
using Android.Widget;

namespace RGBPi.Android
{
    [Activity(
		Label = "RGB-Pi Remote"
		, MainLauncher = true
		, Icon = "@drawable/icon"
		, NoHistory = true
		, Theme = "@style/Theme.RGBPi"
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainView : MvxTabActivity
    {
		public new MainViewModel ViewModel
		{
			get { return (MainViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		protected override void OnViewModelSet()
		{
			base.OnViewModelSet();
			SetContentView(Resource.Layout.MainView);

			var toolbar = FindViewById<Toolbar> (Resource.Id.toolbar);
			TextView tv = new TextView(this.BaseContext);
			tv.Text = "RGB Pi";
			toolbar.AddView(tv);

			CreateTab(typeof(ColorChooserView), "cc", "Color Chooser", Resource.Drawable.Icon);
			CreateTab(typeof(ColorChooserView), "cc 2", "Color Chooser 2", Resource.Drawable.Icon);
		}

		private void CreateTab(Type activityType, string tag, string label, int drawableId )
		{
			var intent = new Intent(this, activityType);
			intent.AddFlags(ActivityFlags.NewTask);

			var spec = TabHost.NewTabSpec(tag);
			var drawableIcon = Resources.GetDrawable(drawableId);

			spec.SetIndicator(label, drawableIcon);
			spec.SetContent(intent);

			TabHost.AddTab(spec);
		}
    }
}