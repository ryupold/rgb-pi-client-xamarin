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
using Cirrious.MvvmCross.Binding.Droid.Views;
using Cirrious.MvvmCross.Binding.Droid.BindingContext;

namespace RGBPi.Android
{
	[Activity (Label = "Settings"
		, Icon = "@drawable/icon"
		, Theme = "@style/Theme.RGBPi",
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class SettingsView : MvxActivity
	{


		public new SettingsViewModel ViewModel {
			get { return (SettingsViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
			

		protected override void OnViewModelSet ()
		{
			base.OnViewModelSet();
			SetContentView (Resource.Layout.SettingsView);

		}
	}

	public class CustomAdapter : MvxAdapter
	{
		public CustomAdapter(Context context, IMvxAndroidBindingContext bindingContext)
			: base(context, bindingContext)
		{
		}

		public override int GetItemViewType(int position)
		{
			var item = GetRawItem(position);
			if (item is Kitten)
				return 0;
			return 1;
		}

		public override int ViewTypeCount
		{
			get { return 2; }
		}

		protected override View GetBindableView(View convertView, object source, int templateId)
		{
			if (source is Kitten)
				templateId = Resource.Layout.ListItem_Kitten;
			else if (source is Dog)
				templateId = Resource.Layout.ListItem_Dog;

			return base.GetBindableView(convertView, source, templateId);
		}
	}
}