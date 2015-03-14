// Helpers/Settings.cs
using Refractored.Xam.Settings;
using Refractored.Xam.Settings.Abstractions;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace RGBPi.Core.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings {
			get {
				try{
					return CrossSettings.Current;
				}catch(Exception e){
					Debug.WriteLine (e.Message);
					throw e;
				}
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		#endregion




		private static Host _activeHost = null;

		public static Host ActiveHost {
			get { 
				if (_activeHost == null) {
					string hostJSON = Settings.AppSettings.GetValueOrDefault ("active_host", string.Empty);
					if (hostJSON != string.Empty) {
						_activeHost = JsonConvert.DeserializeObject<Host> (hostJSON);
					}
				}

				return _activeHost;
			} 
			set {
				Settings.AppSettings.AddOrUpdateValue ("active_host", JsonConvert.SerializeObject (_activeHost));
				Settings.AppSettings.Save ();
				_activeHost = value;
			}
		}

		private static List<Host> hosts = null;

		public static List<Host> Hosts { 
			get { 
				//if(hosts == null)
				{
					string hostsJSON = Settings.AppSettings.GetValueOrDefault ("hosts", string.Empty);
					if (hostsJSON != string.Empty) {
						hosts = JsonConvert.DeserializeObject<List<Host>> (hostsJSON);
					} else {
						hosts = new List<Host> ();
						Settings.AppSettings.AddOrUpdateValue ("hosts", JsonConvert.SerializeObject (hosts));
						Settings.AppSettings.Save ();
					}
				}

				return hosts;
			} 
		}

		public static bool AddHost (Host host)
		{
			foreach (Host h in Hosts) {
				if (h.Equals (host)) {
					return false;
				}
			}

			Hosts.Add (host);
			Settings.AppSettings.AddOrUpdateValue ("hosts", JsonConvert.SerializeObject (Hosts));
			Settings.AppSettings.Save ();

			return true;
		}

		public static bool RemoveHost (string name)
		{
			Host toRemove = null;
			foreach (Host h in Hosts) {
				if (h.name == name) {
					toRemove = h;
				}
			}

			if (toRemove != null) {
				Hosts.Remove (toRemove);
				Settings.AppSettings.AddOrUpdateValue ("hosts", JsonConvert.SerializeObject (Hosts));
				Settings.AppSettings.Save ();
				return true;
			} else {
				return false;
			}
		}



				


		public static string GeneralSettings {
			get {
				return AppSettings.GetValueOrDefault (SettingsKey, SettingsDefault);
			}
			set {
				AppSettings.AddOrUpdateValue (SettingsKey, value);
			}
		}

	}
}