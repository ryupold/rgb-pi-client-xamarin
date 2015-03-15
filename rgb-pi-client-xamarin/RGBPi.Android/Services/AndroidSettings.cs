using System;
using RGBPi.Core;
using System.Collections.Generic;
using Android.Preferences;
using Android.Content;
using Newtonsoft.Json;



namespace RGBPi.Android
{
	public class AndroidSettings : ISettings
	{
		private ISharedPreferences prefs;

		public AndroidSettings (Context appContext)
		{
			prefs = PreferenceManager.GetDefaultSharedPreferences (appContext);
		}

		private Host _activeHost = null;

		public Host ActiveHost {
			get { 
				if (_activeHost == null) {

					string hostJSON = prefs.GetString ("active_host", string.Empty);
					if (hostJSON != string.Empty) {
						_activeHost = JsonConvert.DeserializeObject<Host> (hostJSON);
					}
				}

				return _activeHost;
			} 
			set {
				var editor = prefs.Edit ();
				editor.PutString ("active_host", JsonConvert.SerializeObject (_activeHost));
				if (editor.Commit ()) {
					_activeHost = value;
				}
			}
		}

		public List<Host> GetHosts() { 

			List<Host> hosts = null;
				
					string hostsJSON = prefs.GetString ("hosts", string.Empty);
					if (hostsJSON != string.Empty) {
						hosts = JsonConvert.DeserializeObject<List<Host>> (hostsJSON);
					} else {
						hosts = new List<Host> ();
						var editor = prefs.Edit ();
						editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
						editor.Commit ();
					}
				

				return hosts;
		}

		public bool AddHost (Host host)
		{
			var hosts = GetHosts ();
			foreach (Host h in hosts) {
				if (h.Equals (host)) {
					return false;
				}
			}

			hosts.Add (host);
			var editor = prefs.Edit ();
			editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
			return editor.Commit ();
		}

		public bool RemoveHost (string name)
		{
			var hosts = GetHosts ();
			Host toRemove = null;
			foreach (Host h in hosts) {
				if (h.name == name) {
					toRemove = h;
				}
			}

			if (toRemove != null) {
				hosts.Remove (toRemove);
				var editor = prefs.Edit ();
				editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
				return editor.Commit ();
			} else {
				return false;
			}
		}

		public bool UpdateHost (string name, Host host)
		{
			var hosts = GetHosts();
			bool found = false;
			for (int i=0; i<hosts.Count; i++) {
				if (hosts[i].name == name) {
					hosts [i] = host;
					found = true;
				}
			}

			if (found) {
				var editor = prefs.Edit ();
				editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
				return editor.Commit ();
			}
			return false;
		}
	}
}

