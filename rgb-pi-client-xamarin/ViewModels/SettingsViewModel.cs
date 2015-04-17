using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using System.Collections.Generic;
using RGBPi.Core.Model.Commands;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;
using Cirrious.CrossCore;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RGBPi.Core.ViewModels
{
	public class SettingsViewModel : RemoteControlViewModel
	{
		private ISettings settings;


		public SettingsViewModel ()
		{
			settings = Mvx.Resolve<ISettings> ();
			SetupCommands ();
			AddButtonVisible = false;
		}

		protected override async void InitFromBundle (IMvxBundle parameters)
		{
			base.InitFromBundle (parameters);
			await LoadData ();
		}

		private ObservableCollection <HostViewModel> _hosts;
		public ObservableCollection <HostViewModel> Hosts {
			get{ return _hosts; }
			set{
				_hosts = value;
				RaisePropertyChanged (() => Hosts);
				RaisePropertyChanged (() => NoHosts);
			}
		}

		private async Task LoadData(){
			Hosts = new ObservableCollection <HostViewModel> ();
			var hosts = settings.GetHosts();
			foreach (var h in hosts) {
				Hosts.Add (new HostViewModel(h, this));
			}
			RaiseAllPropertiesChanged ();

			await Task.Delay (500);
			AddButtonVisible = true;
		}

		public void ReloadActiveState(){
			if(Hosts != null){
				foreach (var hvm in Hosts) {
					hvm.ReloadActiveState ();
				}
			}
		}

		private bool _addButtonVisible = false;
		public bool AddButtonVisible{
			get{ return _addButtonVisible; }
			set{ 
				_addButtonVisible = value;
				RaisePropertyChanged (()=>AddButtonVisible);
			}
		}

		public bool NoHosts{get{ return Hosts.Count == 0; }}

		private void SetupCommands(){
			_toolbarGoBackCommand = new MvxCommand (() => GoBack());
			_toolbarAddHostCommand = new MvxCommand (() => AddHost());
		}

		#region Commands

		private MvxCommand _toolbarGoBackCommand;
		public IMvxCommand ToolbarGoBackCommand{get{ return _toolbarGoBackCommand; }}

		private void GoBack (){
			Close (this);
		}

		private MvxCommand _toolbarAddHostCommand;
		public IMvxCommand ToolbarAddHostCommand{get{return _toolbarAddHostCommand;}}

		private void AddHost (Host host=null)
		{
			host = host ?? new Host ();
			Debug.WriteLine ("adding host "+host);
			HostViewModel hvm = new HostViewModel (host, this);
			hvm.IsNew = hvm.IsInEditMode = true;
			Hosts.Add (hvm);
			RaiseAllPropertiesChanged ();
		}

		public bool RemoveHost(HostViewModel hvm)
		{
			bool success = Hosts.Remove (hvm);
			RaiseAllPropertiesChanged ();
			return success;
		}

		#endregion
	}
}

