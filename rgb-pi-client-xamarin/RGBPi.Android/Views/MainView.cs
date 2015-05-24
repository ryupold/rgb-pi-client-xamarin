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
		, Theme = "@style/Theme.RGBPi"
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainView : MvxTabActivity
    {
		private Dictionary<string, View> tabViews = new Dictionary<string, View> ();

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
			CreateFaderTab ();
			CreateDimTab ();
			CreateCommandsTab ();
			CreateCommandBuilderTab ();
			TabHost.TabChanged += (sender, e) => {
				foreach (var t in tabViews) {
					ImageView high = t.Value.FindViewById<ImageView>(Resource.Id.tabsHighlight);

					if(t.Key.Equals(e.TabId)){
						high.Visibility = ViewStates.Visible;
					}
					else{
						high.Visibility = ViewStates.Invisible;
					}
				}
			};


			//this is for testing purposes
//			CreateCommandTestTab ();
		}

		private void CreateColorChooserTab(){
			CreateTab(typeof(ColorChooserView), "cc", null, Resource.Drawable.ic_palette_white_36dp, true);
			ColorChooserView ccv = (ColorChooserView)LocalActivityManager.GetActivity ("cc"); 
			ViewModel.ColorChooser = ccv.ViewModel;
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

		private void CreateFaderTab(){
			CreateTab(typeof(FaderView), "sf", null, Resource.Drawable.ic_swap_calls_white_36dp);
		}

		private void CreateDimTab(){
			CreateTab(typeof(DimView), "dim", null, Resource.Drawable.ic_timelapse_white_36dp);

		}

		private void CreateCommandsTab(){
			CreateTab(typeof(CommandsView), "favs", null, Resource.Drawable.ic_slideshow_white_36dp); //ic_apps_white_36dp
		}

		private void CreateCommandBuilderTab(){
			CreateTab(typeof(CommandBuilderView), "cmd", null, Resource.Drawable.ic_extension_white_36dp);
			//CreateTab(typeof(CommandBuilderView), "cmd", null, Resource.Drawable.ic_dehaze_white_36dp);
		}

		private void CreateCommandTestTab(){
			CreateTab(typeof(CommandTestView), "ct", null, Resource.Drawable.icon_bw_invert);
		}

		private void CreateTab(Type activityType, string tag, string label, int drawableId, bool selected=false)
		{
			var intent = new Intent(this, activityType);

			intent.AddFlags(ActivityFlags.NewTask);

			var spec = TabHost.NewTabSpec(tag);

			//spec.SetIndicator(createTabView(this.ApplicationContext, drawableId));
			var tv = createTabView(this.ApplicationContext, label, drawableId, selected);
			tabViews.Add (tag, tv);
			spec.SetIndicator(tv);
			spec.SetContent(intent);

			TabHost.AddTab(spec);
		}

		private static View createTabView(Context context, string text, int resDrawableID, bool selected=false) {
			LinearLayout view = (LinearLayout)LayoutInflater.From(context).Inflate(RGBPi.Android.Resource.Layout.tabs_bg, null);
			ImageView img = view.FindViewById<ImageView>(Resource.Id.tabsIcon);
			TextView tv = view.FindViewById<TextView>(Resource.Id.tabsText);
			ImageView high = view.FindViewById<ImageView>(Resource.Id.tabsHighlight);
			if(!selected) 
				high.Visibility = ViewStates.Invisible;

			img.SetImageResource (resDrawableID);

			if (string.IsNullOrWhiteSpace (text)) {
				view.RemoveView (tv);
			} else {
				tv.Text = (text);
			}
			return view;
		}

    }
}