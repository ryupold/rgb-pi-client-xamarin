using Newtonsoft.Json;
using RGBPi.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RGB.Services
{
    public class WP8Settings : ISettings
    {

        private const string ACTIVE_HOST_KEY = "active_host";
        private const string HOSTS_KEY = "hosts";

        private IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public WP8Settings()
        {
            settings = IsolatedStorageSettings.ApplicationSettings;
        }

        private Host _activeHost = null;

        public Host ActiveHost
        {
            get
            {
                if (_activeHost == null)
                {
                    var hosts = GetHosts();
                    if (hosts != null && hosts.Count > 0)
                    {
                        string hostJSON = settings.Contains(ACTIVE_HOST_KEY) ? settings[ACTIVE_HOST_KEY] + string.Empty : string.Empty;
                        if (hostJSON != string.Empty)
                        {
                            Host activeH = JsonConvert.DeserializeObject<Host>(hostJSON);
                            if (hosts.Contains(activeH))
                            {
                                _activeHost = activeH;
                            }
                        }
                    }
                }

                return _activeHost;
            }
            set
            {
                if (value != null && value.IsValid)
                {
                    Put(ACTIVE_HOST_KEY, JsonConvert.SerializeObject(value));
                    _activeHost = value;
                }
            }
        }

        public List<Host> GetHosts()
        {

            List<Host> hosts = null;

            string hostsJSON = settings.Contains(HOSTS_KEY) ? settings[HOSTS_KEY] + string.Empty : string.Empty;
            if (hostsJSON != string.Empty)
            {
                hosts = JsonConvert.DeserializeObject<List<Host>>(hostsJSON);
            }
            else
            {
                hosts = new List<Host>();
                Put(HOSTS_KEY, JsonConvert.SerializeObject(hosts));
            }

            return hosts;
        }

        public bool AddHost(Host host)
        {
            //validate
            if (!host.IsValid)
            {
                Debug.WriteLine(string.Format("Error adding {0} => not valid", host));
                return false;
            }

            var hosts = GetHosts();
            foreach (Host h in hosts)
            {
                if (h.Equals(host))
                {
                    Debug.WriteLine(string.Format("Error adding {0} => already in hosts", host));
                    return false;
                }
            }

            hosts.Add(host);
            Put(HOSTS_KEY, JsonConvert.SerializeObject(hosts));
            if (hosts.Count == 1)
            {
                ActiveHost = host;
            }
            return hosts.Count > 0;
        }

        public bool RemoveHost(string name)
        {
            var hosts = GetHosts();
            Host toRemove = null;
            foreach (Host h in hosts)
            {
                if (h.name == name)
                {
                    toRemove = h;
                }
            }

            if (toRemove != null)
            {
                hosts.Remove(toRemove);
                Put(HOSTS_KEY, JsonConvert.SerializeObject(hosts));
                if (ActiveHost != null && ActiveHost.name == name)
                {
                    ActiveHost = null;
                }
                return true;
            }
            else
            {
                Debug.WriteLine(string.Format("Error removing {0} => no host with that name", name));
                return false;
            }
        }

        public bool UpdateHost(string name, Host host)
        {
            //validate
            if (!host.IsValid)
            {
                Debug.WriteLine(string.Format("Error updating {0} => not valid", host));
                return false;
            }

            var hosts = GetHosts();
            bool found = false;
            bool anotherWithTheSameName = false;
            for (int i = 0; i < hosts.Count; i++)
            {
                if (hosts[i].name == name)
                {
                    hosts[i] = host;
                    found = true;
                }
                else if (hosts[i].Equals(host))
                {
                    Debug.WriteLine(string.Format("Error updating {0} => already in hosts", host));
                    anotherWithTheSameName = true;
                }
            }

            if (found && !anotherWithTheSameName)
            {
                Put(HOSTS_KEY, JsonConvert.SerializeObject(hosts));
                if (ActiveHost != null && name == ActiveHost.name)
                {
                    ActiveHost = host;
                }
                return true;
            }
            return false;
        }

        private void Put(string key, object value)
        {
            if (settings.Contains(key))
            {
                settings[key] = value;
            }
            else
            {
                settings.Add(key, value);
            }
            settings.Save();
        }
    }
}
