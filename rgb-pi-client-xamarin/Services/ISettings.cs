using System;
using System.Collections.Generic;

namespace RGBPi.Core
{
	public interface ISettings
	{
		Host ActiveHost{ get; set;}

		List<Host> GetHosts ();

		bool AddHost (Host host);

		bool RemoveHost (string name);

		bool UpdateHost (string name, Host changedHost);
	}
}

