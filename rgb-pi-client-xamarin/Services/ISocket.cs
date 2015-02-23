using System;
using System.Threading.Tasks;
using RGBPi.Core.Model;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using RGBPi.Core.Model.Commands;
using Newtonsoft.Json;

namespace RGBPi.Core
{
	public abstract class ISocket
	{
		public EventHandler<Answer> OnResponse;

		protected Queue<Message> commandQ = new Queue<Message> ();
		protected bool stop = false;
		protected Task task;
		protected Action worker;
		protected JsonSerializerSettings serializationSettings = new JsonSerializerSettings {NullValueHandling = NullValueHandling.Ignore };


		public ISocket ()
		{

			//background thread
			worker = () => {
				while (!stop) {
					try {
						if (commandQ.Count > 0) {							
							while (commandQ.Count > 0) {
								try {
									Message cmd;
									lock (commandQ) {
										cmd = commandQ.Dequeue ();
									}

									ConnectNative("192.168.1.125", 4321);

									SendNative(JsonConvert.SerializeObject(cmd, serializationSettings));

									Answer answer = JsonConvert.DeserializeObject<Answer>(ReceiveNative(), serializationSettings);

									if (OnResponse != null)
										OnResponse (this, answer);

								} catch (Exception ex) {
									Debug.WriteLine("ERROR: " + ex + ": " + ex.Message);
								} finally {
									CloseNative();
								}
							}
						}

					} catch (Exception e) {
						Debug.WriteLine(e + ": " + e.Message);
					} finally {
						CloseNative();
					}

					lock (commandQ) {
						if (commandQ.Count == 0)
							Monitor.Wait (commandQ, 1000);
					}
				}
			};

			task = Task.Factory.StartNew (worker);
		}

		public virtual void Send (Message command)
		{
			lock (commandQ) {
				commandQ.Clear ();
				commandQ.Enqueue (command);
				Monitor.Pulse (commandQ);
			}
		}

		protected abstract bool ConnectNative (string ip, int port);
		protected abstract bool ConnectNative (Host host);
		protected abstract bool SendNative (string clientMessage);
		protected abstract string ReceiveNative ();
		protected abstract void CloseNative ();
	}
}

