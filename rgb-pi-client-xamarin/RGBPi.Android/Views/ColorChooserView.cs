using Android.App;
using Android.Content.PM;
using Cirrious.MvvmCross.Droid.Views;
using RGBPi.Core.ViewModels;
using Com.Larswerkman.HolorColorPicker;

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
		}
    }
}