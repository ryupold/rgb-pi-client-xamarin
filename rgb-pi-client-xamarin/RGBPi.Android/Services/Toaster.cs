using System;
using RGBPi.Core;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Droid.Platform;
using Android.Widget;

namespace RGBPi.Android
{
	public class Toaster : IToaster
	{
		public Toaster (){}

		public void ToastString(string message, RGBPi.Core.ToastShowLength length=RGBPi.Core.ToastShowLength.Short){
			Toast.MakeText(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, message, length == RGBPi.Core.ToastShowLength.Long ? ToastLength.Long : 0).Show();
		}

		public void ToastErrors (RGBPi.Core.Model.Answer answer)
		{
			var errorString = "";
			foreach (var e in answer.error) {
				errorString += e+" \n";
			}

			Toast.MakeText(Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity, errorString, ToastLength.Long).Show();
		}
	}
}

