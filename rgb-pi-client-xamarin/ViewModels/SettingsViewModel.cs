using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using System.Collections.Generic;
using RGBPi.Core.Model.Commands;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;
using Cirrious.CrossCore;

namespace RGBPi.Core.ViewModels
{
	public class SettingsViewModel : RemoteControlViewModel
	{
		private ISettings settings;


		public SettingsViewModel ()
		{
			settings = Mvx.Resolve<ISettings> ();
			SetupCommands ();
			LoadData ();
		}

		private List<HostViewModel> _hosts;
		public List<HostViewModel> Hosts {
			get{ return _hosts; }
			set{
				_hosts = value;
				RaisePropertyChanged (() => Hosts);
			}
		}

		private void LoadData(){
			Hosts = new List<HostViewModel> ();
			var hosts = settings.GetHosts();
			foreach (var h in hosts) {
				Hosts.Add (new HostViewModel(h, this));
			}
			RaiseAllPropertiesChanged ();
		}

		public void ReloadActiveState(){
			if(Hosts != null){
				foreach (var hvm in Hosts) {
					hvm.ReloadActiveState ();
				}
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

		private void AddHost (Host host=null){
			host = host ?? new Host ();
			Debug.WriteLine ("adding host "+host);
			HostViewModel hvm = new HostViewModel (host, this);
			hvm.IsNew = hvm.IsInEditMode = true;
			Hosts.Add (hvm);
			RaiseAllPropertiesChanged ();
		}

		#endregion
	}
}

