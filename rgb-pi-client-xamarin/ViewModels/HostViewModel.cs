using System;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;

namespace RGBPi.Core
{
	public class HostViewModel : MvxViewModel
	{
		#region Host
		private Host _item;

		public Host Item {
			get{ return _item; }
			set{ 
				_item = value;
				RaisePropertyChanged (() => Item);
			}
		}

		public string Name {
			get{ return Item.name; }
			set{ 
				Item.name = value;
				RaisePropertyChanged (() => Name);
			}
		}

		public string IP {
			get{ return Item.ip; }
			set{ 
				Item.ip = value;
				RaisePropertyChanged (() => IP);
			}
		}

		public string Port {
			get{ return Item.port.ToString(); }
			set{ 
				Item.port = int.Parse(value);
				RaisePropertyChanged (() => Port);
			}
		}

		private bool _isInEditMode;
		public bool IsInEditMode{ 
			get
			{
				return _isInEditMode;
			}
			set{
				_isInEditMode = value;
				RaisePropertyChanged (()=>IsInEditMode);
			}
		}

		private bool _isNew;
		public bool IsNew{
			get{ return _isNew;}
			set{ 
				_isNew = value;
				RaisePropertyChanged (() => IsNew);
			}
		}

		#endregion 

		#region ViewModel stuff

		#endregion

		public HostViewModel ()
		{
			SetupCommands ();
		}

		public HostViewModel (Host item)
		{
			_item = item;
			SetupCommands ();
		}

		private void SetupCommands(){
			_saveCommand = new MvxCommand (() => Save());
			_toggleEditCommand = new MvxCommand (() => ToggleEdit());
			_setActiveCommand = new MvxCommand (() => SetActive());
			_removeCommand = new MvxCommand (() => Remove());
		}

		#region Commands

		private MvxCommand _removeCommand;
		public IMvxCommand RemoveCommand{get{ return _removeCommand;}}

		private void Remove (){
			Debug.WriteLine ("TODO: remove "+Item);
		}

		private MvxCommand _setActiveCommand;
		public IMvxCommand SetActiveCommand{get{ return _setActiveCommand;}}

		private void SetActive (){
			Debug.WriteLine ("TODO: active "+Item);
		}

		private MvxCommand _toggleEditCommand;
		public IMvxCommand ToggleEditCommand{get{ return _toggleEditCommand;}}

		private void ToggleEdit (){
			Debug.WriteLine ("TODO: edit "+Item);
			if(IsInEditMode && IsNew){
				Save ();
				IsNew = false;
			}
			IsInEditMode = !IsInEditMode;
		}

		private MvxCommand _saveCommand;
		public IMvxCommand SaveCommand{get{ return _saveCommand;}}

		private void Save (){
			Debug.WriteLine ("TODO: save "+Item);
		}

		#endregion
	}
}

