using Cirrious.CrossCore.IoC;
using Cirrious.CrossCore;
using RGBPi.Core.Helpers;
using System.Diagnostics;

namespace RGBPi.Core
{
    public class App : Cirrious.MvvmCross.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

			//Mvx.RegisterSingleton<ISettings> (Settings);

            RegisterAppStart<ViewModels.MainViewModel>();
        }
    }
}