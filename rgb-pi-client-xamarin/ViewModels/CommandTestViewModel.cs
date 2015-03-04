using System;
using Cirrious.MvvmCross.ViewModels;
using RGBPi.Core.Model;
using RGBPi.Core.Model.DataTypes;
using System.Diagnostics;
using RGBPi.Core.Model.Commands;
using System.Collections.Generic;


namespace RGBPi.Core.ViewModels
{
	public class CommandTestViewModel : RemoteControlViewModel
	{
		private MvxCommand _testCCWhiteCommand;
		public IMvxCommand TestCCWhiteCommand{get{ return _testCCWhiteCommand;}}

		private void CCWhite (){
			Message msg = new Message (new List<Command> {
				new CC (new Color (1f, 1f, 1f))
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testCCPlusCommand;
		public IMvxCommand TestCCPlusCommand{get{ return _testCCPlusCommand;}}

		private void CCPlus (){
			Message msg = new Message (new List<Command> {
				new CC (new Color (0.1f, 0.1f, 0.1f), "+")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testCCMinusCommand;
		public IMvxCommand TestCCMinusCommand{get{ return _testCCMinusCommand;}}

		private void CCMinus (){
			Message msg = new Message (new List<Command> {
				new CC (new Color (0.1f, 0.1f, 0.1f), "-")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testCCRandomCommand;
		public IMvxCommand TestCCRandomCommand{get{ return _testCCRandomCommand;}}

		private void CCRandom (){
			Message msg = new Message (new List<Command> {
				new CC ("{r:0-1,0-1,0-1}")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testFadeRedCommand;
		public IMvxCommand TestFadeRedCommand{get{ return _testFadeRedCommand;}}

		private void FadeRed (){
			Message msg = new Message (new List<Command> {
				new Fade (2f, new Color(1f, 0, 0))
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testFadeBlueGreenCommand;
		public IMvxCommand TestFadeBlueGreenCommand{get{ return _testFadeBlueGreenCommand;}}

		private void FadeBlueGreen (){
			Message msg = new Message (new List<Command> {
				new Fade (4f, new Color(0f, 1f, 0f), "{b:0,0,255}")
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testFadeRandomCommand;
		public IMvxCommand TestFadeRandomCommand{get{ return _testFadeRandomCommand;}}

		private void FadeRandom (){
			Message msg = new Message (new List<Command> {
				new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1))
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testListCommand;
		public IMvxCommand TestListCommand{get{ return _testListCommand;}}

		private void List (){
			var list = new RGBPi.Core.Model.Commands.List (new List<Command> ());

			//off
			list.commands.Add (new Fade (1f, new Color(0)));
			//random
			list.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			list.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			list.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			list.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			//off
			list.commands.Add (new CC (new Color(0)));

			Message msg = new Message (new List<Command> {
				list
			}); 
			SendMessage (msg);
		}

		private MvxCommand _testWaitCommand;
		public IMvxCommand TestWaitCommand{get{ return _testWaitCommand;}}

		private void Wait (){
			Message msg = new Message (new List<Command>());

			//off
			msg.commands.Add (new CC (new Color(0)));
			//random
			msg.commands.Add (new RGBPi.Core.Model.Commands.Wait(1f));
			msg.commands.Add (new CC (new Color(0f, 1f, 0f, 1, 0, 1)));
			msg.commands.Add (new RGBPi.Core.Model.Commands.Wait(new Time(0.5f, 3f)));
			msg.commands.Add (new CC (new Color(0f, 1f, 0f, 1, 0, 1)));
			msg.commands.Add (new RGBPi.Core.Model.Commands.Wait("{c:2}"));
			msg.commands.Add (new CC (new Color(0f, 1f, 0f, 1, 0, 1)));
			msg.commands.Add (new RGBPi.Core.Model.Commands.Wait(new Time(5)));
			//off
			msg.commands.Add (new Fade (1f, new Color(0)));

			SendMessage (msg);
		}

		private MvxCommand _testLoopCommand;
		public IMvxCommand TestLoopCommand{get{ return _testLoopCommand;}}

		private void Loop (){
			var loop = new RGBPi.Core.Model.Commands.Loop (new Condition(5), new List<Command> ());

			//off
			loop.commands.Add (new Fade (1f, new Color(0)));
			//random
			loop.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			loop.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			loop.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			loop.commands.Add (new Fade (1f, new Color(0f, 1f, 0f, 1, 0, 1)));
			//off
			loop.commands.Add (new CC (new Color(0)));
			loop.commands.Add (new RGBPi.Core.Model.Commands.Wait(new Time(0.1f, 3f)));

			Message msg = new Message (new List<Command> {
				loop,
				new CC(new Color(0,0,0))
			});

			SendMessage (msg);
		}


		public CommandTestViewModel(){
			_testCCWhiteCommand = new MvxCommand (()  => CCWhite());
			_testCCPlusCommand = new MvxCommand (()  => CCPlus());
			_testCCMinusCommand = new MvxCommand (()  => CCMinus());
			_testCCRandomCommand = new MvxCommand (()  => CCRandom());
			_testFadeRedCommand = new MvxCommand (()  => FadeRed());
			_testFadeBlueGreenCommand = new MvxCommand (()  => FadeBlueGreen());
			_testFadeRandomCommand = new MvxCommand (()  => FadeRandom());
			_testListCommand = new MvxCommand (()  => List());
			_testWaitCommand = new MvxCommand (()  => Wait());
			_testLoopCommand = new MvxCommand (()  => Loop());
		}


	}

}

