using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;

namespace RGBPi.Core.ViewModels
{
	public class ColorChooserViewModel : RemoteControlViewModel
	{
		private Color _currentColor;
		public Color CurrentColor{ get; set;}

		public ColorChooserViewModel(){

		}
	}
}

