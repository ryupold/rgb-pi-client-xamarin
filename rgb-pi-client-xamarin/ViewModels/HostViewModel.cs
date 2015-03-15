using System;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;
using Cirrious.CrossCore;
using RGBPi.Core.ViewModels;
using RGBPi.Core.Model.DataTypes;

namespace RGBPi.Core
{
	public class HostViewModel : MvxViewModel
	{
		private ISettings settings;
		private string oldName;
		private SettingsViewModel parent;
		private static readonly int NO_ERROR_COLOR = new Color(0.8f, 0.8f, 0.8f);
		private static readonly int ERROR_COLOR = new Color(0.8f, 0.4f, 0.4f);

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
			get{ return Item.port==0 ? string.Empty: Item.port.ToString(); }
			set{
				int.TryParse(value, out Item.port);
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

		//to lazy to write a converter xD
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

		//to lazy to write a converter xD
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

		private Color _lastError = NO_ERROR_COLOR;
		public Color LastError {
			get{ return _lastError;}
			set{ 
				_lastError = value;
				RaisePropertyChanged (() => LastError);
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

		private bool Remove (){
			Debug.WriteLine ("remove "+Item);
			bool success = false;
			if (IsNew) {
				Debug.WriteLine ("remove new " + Item);
				success = true;
			} else if (settings.RemoveHost (Item.name)) {
				Debug.WriteLine ("remove old " + Item);
				success = true;
			} 

			if(success){
				parent.RemoveHost (this);
			}

			return success;
		}

		private MvxCommand _toggleEditCommand;
		public IMvxCommand ToggleEditCommand{get{ return _toggleEditCommand;}}

		private void ToggleEdit (){
			Debug.WriteLine ("edit "+Item);
			if (IsInEditMode && Save ()) {
				IsInEditMode = false;
			}else{
				IsInEditMode = true;
			}
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

		private bool Save (){
			Debug.WriteLine ("save "+Item);
			if (IsNew && settings.AddHost (Item)) {
				Debug.WriteLine ("new "+Item);
				oldName = Item.name;
				IsNew = false;
				LastError = NO_ERROR_COLOR;
				return true;
			} else if (settings.UpdateHost (oldName, Item)) {
				Debug.WriteLine ("update "+Item);
				oldName = Item.name;
				LastError = NO_ERROR_COLOR;
				return true;
			} else {
				LastError = ERROR_COLOR;
				return false;
			}
		}

		#endregion
	}
}

