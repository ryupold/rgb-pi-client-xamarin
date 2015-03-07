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

		private List<HostViewModel> _hosts;
		public List<HostViewModel> Hosts {
			get{ return _hosts; }
			set{
				_hosts = value;
				RaisePropertyChanged (() => Hosts);
			}
		}


		private void SetupCommands(){
			_toolbarGoBackCommand = new MvxCommand (() => GoBack());
			_toolbarAddHostCommand = new MvxCommand (() => AddHost());
		}

		#region Commands

		private MvxCommand _toolbarGoBackCommand;
		public IMvxCommand ToolbarGoBackCommand{get{ return _toolbarGoBackCommand;}}

		private void GoBack (){
			Close (this);
		}

		private MvxCommand _toolbarAddHostCommand;
		public IMvxCommand ToolbarAddHostCommand{get{ return _toolbarAddHostCommand;}}

		private void AddHost (){
			Debug.WriteLine ("TODO: add host");
		}

		#endregion
	}
}

