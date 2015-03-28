using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;
using System.Collections.ObjectModel;
using RGBPi.Core.Model.Commands;
using System.Collections.Generic;

namespace RGBPi.Core.ViewModels
{
	public class FaderViewModel : RemoteControlViewModel
	{
		public FaderViewModel(){
			SetupCommands ();	
		}

		private void SetupCommands(){
//			_addColorCommand = new MvxCommand ((Color c) => AddColor(c));
//			_removeCommand = new MvxCommand ((FaderItemViewModel item) => Remove(item));
		}

		#region Properties
		private ObservableCollection <FaderItemViewModel> _colors = new ObservableCollection<FaderItemViewModel>();
		public ObservableCollection <FaderItemViewModel> Colors {
			get{ return _colors; }
			set{
				_colors = value;
				RaisePropertyChanged (() => Colors);
			}
		}

		private int _time = 5;
		public int Time{
			get{ return _time;}
			set{ _time = value; RaisePropertyChanged (()=>TimeText);}
		}

		public string TimeText{get{ return Time+" s"; }}
		#endregion

		public int GetIndexOfItem(FaderItemViewModel item){
			return Colors.IndexOf (item);
		}

		public void UpdateItems(){
			foreach(FaderItemViewModel f in Colors){
				f.UpdateState ();
			}
		}

		public bool ChangeOrder(FaderItemViewModel item, OrderDirection dir){
			int currentIndex = GetIndexOfItem (item);

			switch(dir){
			case OrderDirection.Up: 
				if(currentIndex > 0){
					Colors.Move (currentIndex, currentIndex - 1);
				}
				else return false;
				break;

				case OrderDirection.Down: 
				if(currentIndex < Colors.Count - 1){
					Colors.Move (currentIndex, currentIndex + 1);
				}
				else return false;
					break;
			}

			RaisePropertyChanged (()=>Colors);
			UpdateItems ();
			return true;
		}

		#region Commands
//		private MvxCommand _addColorCommand;
//		public IMvxCommand AddColorCommand{get{ return _addColorCommand;}}

		public void AddColor(Color color){
			FaderItemViewModel item = new FaderItemViewModel (color, this);
			Colors.Add (item);
			RaisePropertyChanged (()=>Colors);
			UpdateItems ();
		}

//		private MvxCommand _removeCommand;
//		public IMvxCommand RemoveCommand{get{ return _removeCommand;}}

		public void Remove(FaderItemViewModel item){
			Debug.WriteLine ("remove "+item.Item);
			Colors.Remove (item);
			RaisePropertyChanged (()=>Colors);
			UpdateItems ();
		}


		public IMvxCommand StartFadeCommand{get{ return new MvxCommand(() => StartFade());}}

		public void StartFade(){
			Message msg = new Message ();
			List<Command> fades = new List<Command> ();
			Time timePerFade = ((float)this.Time) / (float)Colors.Count;
			foreach(var item in Colors)
			{
				fades.Add (new Fade(timePerFade, item.Item));
			}

			Loop fadeloop = new Loop (new Condition (true), fades);
			msg.commands = new List<Command> {fadeloop};
			SendMessage (msg);
		}

		#endregion
	}

	public enum OrderDirection{
		Up, Down
	}
}

