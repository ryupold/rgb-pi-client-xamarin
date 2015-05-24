using System;
using Cirrious.MvvmCross.ViewModels;
using System.Diagnostics;
using Cirrious.CrossCore;
using RGBPi.Core.ViewModels;
using RGBPi.Core.Model.DataTypes;
using Cirrious.CrossCore.UI;

namespace RGBPi.Core
{
	public class FaderItemViewModel : MvxViewModel
	{
		
		private FaderViewModel _parent;

		private Color _item;
		public Color Item{get{ return _item;}
			set{ _item = value; RaisePropertyChanged (() => Item); RaisePropertyChanged (() => IsRandom);}
		}

		public string HexColorString{get{ return "#"+((byte)(Item.R*255)).ToString("X")+((byte)(Item.G*255)).ToString("X")+((byte)(Item.B*255)).ToString("X");}}
		public string RgbColorString{get{ return "("+((int)(Item.R*255))+","+((int)(Item.G*255))+","+((int)(Item.B*255))+")";}}

		public FaderItemViewModel (FaderViewModel p)
		{
			_parent = p;
			SetupCommands ();
		}

		public FaderItemViewModel (Color item, FaderViewModel p)
		{
			_parent = p;
			Item = item;
			SetupCommands ();
		}

		public void Init(Color item){
			Item = item;
		}

		private void SetupCommands(){
			_changeOrderUpCommand = new MvxCommand (() => ChangeOrderUp());
			_changeOrderDownCommand = new MvxCommand (() => ChangeOrderDown());
			_removeCommand = new MvxCommand (() => Remove());
		}


		public void UpdateState(){
			RaisePropertyChanged (()=>CanMoveUp);
			RaisePropertyChanged (()=>CanMoveDown);
		}

		#region Properties
		public bool CanMoveUp{get{ return _parent.GetIndexOfItem (this) > 0; }}
		public bool CanMoveDown{get{ return _parent.GetIndexOfItem (this) < _parent.Colors.Count - 1; }}
		public bool IsRandom{get{ return Item.IsRandom; }}
		public bool IsNotRandom{get{ return !Item.IsRandom; }} //yeah, i'm lazy ^^
		#endregion

		#region Commands

		private MvxCommand _changeOrderUpCommand;
		public IMvxCommand ChangeOrderUpCommand{get{ return _changeOrderUpCommand;}}

		private void ChangeOrderUp(){
			Debug.WriteLine ("change order up");
			_parent.ChangeOrder (this, OrderDirection.Up);
		}

		private MvxCommand _changeOrderDownCommand;
		public IMvxCommand ChangeOrderDownCommand{get{ return _changeOrderDownCommand;}}

		private void ChangeOrderDown(){
			Debug.WriteLine ("change order down");
			_parent.ChangeOrder (this, OrderDirection.Down);
		}

		private MvxCommand _removeCommand;
		public IMvxCommand RemoveCommand{get{ return _removeCommand;}}

		private void Remove(){
			_parent.Remove(this);
		}


		#endregion
	}
}

