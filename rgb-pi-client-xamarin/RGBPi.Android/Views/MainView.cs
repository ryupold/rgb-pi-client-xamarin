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
using Android.Graphics.Drawables;
using Android.Views;

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

			//add tabs
			CreateColorChooserTab ();
			CreateCommandTestTab ();
		}

		private void CreateColorChooserTab(){
			CreateTab(typeof(ColorChooserView), "cc", "Color Chooser", Resource.Drawable.Icon);
			ColorChooserView ccv = (ColorChooserView)LocalActivityManager.GetActivity ("cc"); 
			ccv.ColorChanged += (s, newColor) => {
				RGBPi.Core.Model.DataTypes.Color c = new RGBPi.Core.Model.DataTypes.Color (newColor);

				RGBPi.Core.Model.Message msg = new RGBPi.Core.Model.Message ();
				CC fade = new CC (c);
				msg.commands = new List<RGBPi.Core.Model.Commands.Command> {
					fade
				};
				ViewModel.SendMessage (msg);
			};
		}

		private void CreateCommandTestTab(){
			CreateTab(typeof(CommandTestView), "ct", "Command Test", Resource.Drawable.Icon);
			CommandTestView ctv = (CommandTestView)LocalActivityManager.GetActivity ("ct"); 
		}

		private void CreateTab(Type activityType, string tag, string label, int drawableId )
		{
			var intent = new Intent(this, activityType);

			intent.AddFlags(ActivityFlags.NewTask);

			var spec = TabHost.NewTabSpec(tag);
			var drawableIcon = Resources.GetDrawable(drawableId);

			//spec.SetIndicator(label, drawableIcon);
			spec.SetIndicator(createTabView(this.ApplicationContext, label));
			spec.SetContent(intent);

			TabHost.AddTab(spec);
		}

		private static View createTabView(Context context, string text) {
			View view = LayoutInflater.From(context).Inflate(RGBPi.Android.Resource.Layout.tabs_bg, null);
			TextView tv = (TextView) view.FindViewById(Resource.Id.tabsText);
			tv.Text = (text);
			return view;
		}

    }
}