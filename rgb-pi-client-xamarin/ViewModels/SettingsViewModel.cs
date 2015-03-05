using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using System.Collections.Generic;
using RGBPi.Core.Model.Commands;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;

namespace RGBPi.Core.ViewModels
{
	public class SettingsViewModel : RemoteControlViewModel
	{
		public SettingsViewModel ()
		{
			SetupCommands ();
		}

		private List<Host> _hosts;
		public List<Host> Hosts {
			get{ return _hosts; }
			set{
				_hosts = value;
				RaisePropertyChanged (() => Hosts);
			}
		}


		private void SetupCommands(){
			_toolbarShowSettingsCommand = new MvxCommand (() => ShowSettings());
		}

		#region Commands

		private MvxCommand _toolbarShowSettingsCommand;
		public IMvxCommand ToolbarShowSettingsCommand{get{ return _toolbarShowSettingsCommand;}}

		private void ShowSettings (){
			Debug.WriteLine ("TODO: Open Settings");
		}

		#endregion
	}
}

