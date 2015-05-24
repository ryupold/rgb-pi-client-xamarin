using System;
using RGBPi.Core.Model;

namespace RGBPi.Core
{
	public interface IToaster
	{
		void ToastString (string message, ToastShowLength length=ToastShowLength.Short);
		void ToastErrors (Answer answer);
	}

	public enum ToastShowLength{
		Short = 1,
		Medium = 2,
		Long = 3
	}
}

