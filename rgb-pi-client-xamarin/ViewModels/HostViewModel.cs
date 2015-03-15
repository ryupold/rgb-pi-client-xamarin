using System;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;
using Cirrious.CrossCore;
using RGBPi.Core.ViewModels;

namespace RGBPi.Core
{
	public class HostViewModel : MvxViewModel
	{
		private ISettings settings;
		private string oldName;
		private SettingsViewModel parent;

		#region Host
		private Host _item;

		public Host Item {
			get{ return _item; }
			set{ 
				_item = value;
				_isActive = _item.Equals(settings.ActiveHost);
				oldName = _item.name;
				RaisePropertyChanged (() => Item);
				RaisePropertyChanged (() => IsActive);
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
				RaisePropertyChanged (()=>IsInViewMode);
			}
		}

		public bool IsInViewMode{get{ return !IsInEditMode;}}

		private bool _isNew;
		public bool IsNew{
			get{ return _isNew;}
			set{ 
				_isNew = value;
				if (_isNew)
					IsInEditMode = true;
				RaisePropertyChanged (() => IsNew);
				RaisePropertyChanged (() => IsOld);
			}
		}

		public bool IsOld{get{ return !IsNew;}}

		private bool _isActive;
		public bool IsActive{get{ return _isActive; }
			set{ 
				_isActive = value;
				if(_isActive){
					settings.ActiveHost = Item;
				}

				RaisePropertyChanged (() => IsActive);
			}
		}

		#endregion 



		public HostViewModel (SettingsViewModel p)
		{
			settings = Mvx.Resolve<ISettings> ();
			parent = p;
			SetupCommands ();
		}

		public HostViewModel (Host item, SettingsViewModel p)
		{
			settings = Mvx.Resolve<ISettings> ();
			parent = p;
			Item = item;
			SetupCommands ();
		}

		public void Init(Host item){
			Item = item;
		}

		private void SetupCommands(){
			_saveCommand = new MvxCommand (() => Save());
			_toggleEditCommand = new MvxCommand (() => ToggleEdit());
			_setActiveCommand = new MvxCommand (() => SetActive());
			_removeCommand = new MvxCommand (() => Remove());
			_toggleActiveCommand = new MvxCommand (() => ToggleActive());
		}

		public void ReloadActiveState(){
			_isActive = _item.Equals(settings.ActiveHost);
			RaisePropertyChanged (() => IsActive);
		}

		private void CheckOthersActiveState(){
			parent.ReloadActiveState ();
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
			Debug.WriteLine ("edit "+Item);
			if(IsInEditMode) Save ();
			IsInEditMode = !IsInEditMode;
		}

		private MvxCommand _toggleActiveCommand;
		public IMvxCommand ToggleActiveCommand{get{ return _toggleActiveCommand;}}

		private void ToggleActive (){
			Debug.WriteLine ("set active "+Item);
			if(!IsActive)IsActive = !IsActive;
			CheckOthersActiveState ();
		}

		private MvxCommand _saveCommand;
		public IMvxCommand SaveCommand{get{ return _saveCommand;}}

		private void Save (){
			Debug.WriteLine ("save "+Item);
			if (IsNew && settings.AddHost (Item)) {
				oldName = Item.name;
				IsNew = false;
			}
			else if (settings.UpdateHost (oldName, Item)) {
				oldName = Item.name;
			}
		}

		#endregion
	}
}

