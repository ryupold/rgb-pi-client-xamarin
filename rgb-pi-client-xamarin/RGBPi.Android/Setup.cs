using Android.Content;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.Droid.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.CrossCore;
using RGBPi.Core;
using System.Collections.Generic;
using System.Reflection;
using Cirrious.CrossCore.Converters;
using System;
using Cirrious.MvvmCross.Dialog.Droid;


namespace RGBPi.Android
{
    public class Setup : MvxAndroidDialogSetup
    {
		public Setup(Context applicationContext) : base(applicationContext)
        {
        }

        protected override IMvxApplication CreateApp()
        {
			Mvx.RegisterSingleton<ISettings> (new AndroidSettings(this.ApplicationContext));
			Mvx.RegisterSingleton<ISocket>(new Socket());
			Mvx.RegisterSingleton<IToaster>(new Toaster());
            return new Core.App();
        }

//		protected override List<Assembly> ValueConverterAssemblies 
//		{
//			get
//			{
//				var toReturn = base.ValueConverterAssemblies;
//				toReturn.Add(typeof(ColorConverter).Assembly);
//				return toReturn;
//			}
//		}

		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }
    }
}