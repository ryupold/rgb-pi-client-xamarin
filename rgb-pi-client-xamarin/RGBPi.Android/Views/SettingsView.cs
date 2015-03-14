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
using RGBPi.Core;
using RGBPi.Core.Helpers;

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
			MvxListView list = FindViewById<MvxListView> (Resource.Id.list_hosts);
			list.Adapter = new HostViewAdapter (this, (IMvxAndroidBindingContext)this.BindingContext);
		}
	}

	public class HostViewAdapter : MvxAdapter
	{
		public HostViewAdapter(Context context, IMvxAndroidBindingContext bindingContext)
			: base(context, bindingContext)
		{
		}

		public override int GetItemViewType(int position)
		{
			var item = GetRawItem(position);
			if (item is Host)
				return 0;
			return 0;
		}

		public override int ViewTypeCount
		{
			get { return 1; }
		}

		protected override View GetBindableView(View convertView, object source, int templateId)
		{
			if (source is Host)
				templateId = Resource.Layout.host_item;

			return base.GetBindableView(convertView, source, templateId);
		}
	}
}