using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;

namespace RGBPi.Core.ViewModels
{
	public class ColorChooserViewModel : RemoteControlViewModel
	{
		private Color _currentColor = new Color(0,0,0);
		public Color CurrentColor{ get { return _currentColor; } set{ 
				_currentColor = value; 
				BackgroundColor = MakePastel (_currentColor);
				RaisePropertyChanged( () => CurrentColor);
			}}

		private Color _backgroundColor = new Color(0,0,0);
		public Color BackgroundColor{ get { return _backgroundColor; } set{ 
				_backgroundColor = value; 
				RaisePropertyChanged( () => BackgroundColor);
		}}

		public void SetCurrentColor(Color c){
			_currentColor = c; 
			BackgroundColor = MakePastel (_currentColor);
		}


		private Color MakePastel(Color col){
			return new Color (
				MakePastel(col.R),
				MakePastel(col.G),
				MakePastel(col.B)
			);
		}

		private float MakePastel(float col){
			return Math.Max(col * 0.5f, 0.3f);
		}
	}

}

