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

namespace RGBPi.Android
{
	public class HostView : View
	{
//		public HostViewModel ViewModel {
//			get { return (HostViewModel) base.DataContext; }
//			set { base.DataContext = value; }
//		}


		public HostView (Context c) : base (c){
			Console.WriteLine ("asdasdasdasdsad");
		}


	}
}