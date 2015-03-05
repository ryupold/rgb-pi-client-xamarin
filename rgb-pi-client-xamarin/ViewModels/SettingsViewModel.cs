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

		private List<Host> hosts;


		private void SetupCommands(){
			_toolbarOffCommand = new MvxCommand (() => TurnOff());
			_toolbarBrighterCommand = new MvxCommand (() => Brighter());
			_toolbarDarkerCommand = new MvxCommand (() => Darker());
			_toolbarShowSettingsCommand = new MvxCommand (() => ShowSettings());
		}

		#region Toolbar Commands


		private MvxCommand _toolbarOffCommand;
		public IMvxCommand ToolbarOffCommand{get{ return _toolbarOffCommand;}}

		private void TurnOff (){
			Message msg = new Message (new List<Command> {
				new Fade (2f, new Color (0))
			}); 
			SendMessage (msg);
		}


		private MvxCommand _toolbarBrighterCommand;
		public IMvxCommand ToolbarBrighterCommand{get{ return _toolbarBrighterCommand;}}

		private void Brighter (){
			Message msg = new Message (new List<Command> {
				new CC (new Color (0.1f,0.1f,0.1f), "+")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _toolbarDarkerCommand;
		public IMvxCommand ToolbarDarkerCommand{get{ return _toolbarDarkerCommand;}}

		private void Darker (){
			Message msg = new Message (new List<Command> {
				new CC (new Color (0.1f,0.1f,0.1f), "-")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _toolbarShowSettingsCommand;
		public IMvxCommand ToolbarShowSettingsCommand{get{ return _toolbarShowSettingsCommand;}}

		private void ShowSettings (){
			Debug.WriteLine ("TODO: Open Settings");
		}

		#endregion
	}
}

