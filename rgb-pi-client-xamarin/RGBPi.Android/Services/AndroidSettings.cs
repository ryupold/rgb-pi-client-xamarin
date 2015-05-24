using System;
using RGBPi.Core;
using System.Collections.Generic;
using Android.Preferences;
using Android.Content;
using Newtonsoft.Json;
using System.Diagnostics;



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
					var hosts = GetHosts ();
					if(hosts != null && hosts.Count > 0){
						string hostJSON = prefs.GetString ("active_host", string.Empty);
						if (hostJSON != string.Empty) {
							Host activeH = JsonConvert.DeserializeObject<Host> (hostJSON);
							if (hosts.Contains (activeH)) {
								_activeHost = activeH;
							}
						}
					}
				}

				return _activeHost;
			} 
			set {
				if (value != null && value.IsValid) {
					var editor = prefs.Edit ();
					editor.PutString ("active_host", JsonConvert.SerializeObject (value));
					if (editor.Commit ()) {
						_activeHost = value;
					}
				}
			}
		}

		public List<Host> GetHosts ()
		{ 

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
			//validate
			if (!host.IsValid) {
				Debug.WriteLine (string.Format ("Error adding {0} => not valid", host));
				return false;
			}

			var hosts = GetHosts ();
			foreach (Host h in hosts) {
				if (h.Equals (host)) {
					Debug.WriteLine (string.Format ("Error adding {0} => already in hosts", host));
					return false;
				}
			}

			hosts.Add (host);
			var editor = prefs.Edit ();
			editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
			bool success = editor.Commit ();
			if (success && hosts.Count == 1) {
				ActiveHost = host;
			}
			return success;
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
				bool success = editor.Commit ();
				if (success && ActiveHost != null && ActiveHost.name == name) {
					ActiveHost = null;
				}
				return success;
			} else {
				Debug.WriteLine (string.Format ("Error removing {0} => no host with that name", name));
				return false;
			}
		}

		public bool UpdateHost (string name, Host host)
		{
			//validate
			if (!host.IsValid) {
				Debug.WriteLine (string.Format ("Error updating {0} => not valid", host));
				return false;
			}

			var hosts = GetHosts ();
			bool found = false;
			bool anotherWithTheSameName = false;
			for (int i = 0; i < hosts.Count; i++) {
				if (hosts [i].name == name) {
					hosts [i] = host;
					found = true;
				} else if (hosts [i].Equals(host)) {
					Debug.WriteLine (string.Format ("Error updating {0} => already in hosts", host));
					anotherWithTheSameName = true;
				}
			}

			if (found && !anotherWithTheSameName) {
				var editor = prefs.Edit ();
				editor.PutString ("hosts", JsonConvert.SerializeObject (hosts));
				bool success = editor.Commit ();
				if (success && ActiveHost != null && name == ActiveHost.name) {
					ActiveHost = host;
				}
				return success;
			}
			return false;
		}
	}
}

