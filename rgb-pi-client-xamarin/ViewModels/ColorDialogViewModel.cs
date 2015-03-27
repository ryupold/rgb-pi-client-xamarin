using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;

namespace RGBPi.Core.ViewModels
{
	public class ColorDialogViewModel : RemoteControlViewModel
	{
		protected Color _currentColor = new Color(0,0,0);
		public Color CurrentColor{ get { return _currentColor; } set{ 
				_currentColor = value; 
				BackgroundColor = MakePastel (_currentColor);
				RaisePropertyChanged( () => CurrentColor);
			}}

		protected Color _backgroundColor = new Color(0,0,0);
		public Color BackgroundColor{ get { return _backgroundColor; } set{ 
				_backgroundColor = value; 
				RaisePropertyChanged( () => BackgroundColor);
		}}

		public void SetCurrentColor(Color c){
			_currentColor = c; 
			BackgroundColor = MakePastel (_currentColor);
		}

		#region Random Color properties
		protected readonly float[] _randomColor = new float[]{0,1,0,1,0,1}; //initialize value:  full random

		public float RandomRedMin{
			get{ return _randomColor[0]; }
			set{ 
				_randomColor [0] = value;
				if (value > _randomColor [1])
					RandomRedMax = value;
				RaisePropertyChanged (()=>RandomRedMin);
			}
		}
			
		public float RandomRedMax{
			get{ return _randomColor[1]; }
			set{ 
				_randomColor [1] = value;
				if (value < _randomColor [0])
					RandomRedMin = value;
				RaisePropertyChanged (()=>RandomRedMax);
			}
		}

		public float RandomGreenMin{
			get{ return _randomColor[2]; }
			set{ 
				_randomColor [2] = value;
				if (value > _randomColor [3])
					RandomGreenMax = value;
				RaisePropertyChanged (()=>RandomGreenMin);
			}
		}

		public float RandomGreenMax{
			get{ return _randomColor[3]; }
			set{ 
				_randomColor [3] = value;
				if (value < _randomColor [2])
					RandomGreenMin = value;
				RaisePropertyChanged (()=>RandomGreenMax);
			}
		}

		public float RandomBlueMin{
			get{ return _randomColor[4]; }
			set{ 
				_randomColor [4] = value;
				if (value > _randomColor [5])
					RandomBlueMax = value;
				RaisePropertyChanged (()=>RandomBlueMin);
			}
		}

		public float RandomBlueMax{
			get{ return _randomColor[5]; }
			set{ 
				_randomColor [5] = value;
				if (value < _randomColor [4])
					RandomBlueMin = value;
				RaisePropertyChanged (()=>RandomBlueMax);
			}
		}

		protected bool _isRandom = false;
		public bool IsRandom{
			get{ return _isRandom; }
			set{ 
				_isRandom = value;
				RaisePropertyChanged (()=>IsRandom);
			}
		}
		#endregion


		protected Color MakePastel(Color col){
			return new Color (
				MakePastel(col.R),
				MakePastel(col.G),
				MakePastel(col.B)
			);
		}

		protected float MakePastel(float col){
			return Math.Max(col * 0.5f, 0.3f);
		}
	}

}

