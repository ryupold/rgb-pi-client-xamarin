using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;

namespace RGBPi.Core.ViewModels
{
	public class ColorChooserViewModel : MvxViewModel
	{
		private Color _currentColor;
		public Color CurrentColor{ get{ return _currentColor; } set{ 
				_currentColor = value;  
				RaisePropertyChanged( () => CurrentColor);
			}}

		public ColorChooserViewModel(){

		}


	}

}

