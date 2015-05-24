using Newtonsoft.Json;
using RGBPi.Core;
using RGBPi.Core.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RGB.Services
{
    public abstract class IWP8Socket
    {
        protected Queue<MessageAndAnswer> commandQ = new Queue<MessageAndAnswer>();
        protected bool stop = false;
        protected Task task;
        protected Action worker;
        protected JsonSerializerSettings serializationSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
        protected ISettings settings;


        public IWP8Socket()
        {
            //get settings
            settings = new WP8Settings();

            //background thread
            worker = () => {
                while (!stop)
                {
                    try
                    {
                        if (commandQ.Count > 0 && settings.ActiveHost != null)
                        {
                            while (commandQ.Count > 0)
                            {
                                try
                                {
                                    MessageAndAnswer cmd;
                                    lock (commandQ)
                                    {
                                        cmd = commandQ.Dequeue();
                                    }

                                    ConnectNative(settings.ActiveHost);

                                    SendNative(JsonConvert.SerializeObject(cmd.message, serializationSettings));

                                    Answer answer = JsonConvert.DeserializeObject<Answer>(ReceiveNative(), serializationSettings);

                                    if (cmd.answerCallback != null)
                                        cmd.answerCallback(answer);

                                }
                                catch (Exception ex)
                                {
                                    Debug.WriteLine("ERROR: " + ex + ": " + ex.Message);
                                }
                                finally
                                {
                                    CloseNative();
                                }
                            }
                        }

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e + ": " + e.Message);
                    }
                    finally
                    {
                        CloseNative();
                    }

                    lock (commandQ)
                    {
                        if (commandQ.Count == 0)
                            Monitor.Wait(commandQ, 1000);
                    }
                }
            };

            task = Task.Factory.StartNew(worker);
        }

        public virtual void Send(Message command, Action<Answer> answerCallback = null)
        {
            lock (commandQ)
            {
                commandQ.Clear();
                if (settings.ActiveHost != null)
                {
                    commandQ.Enqueue(new MessageAndAnswer(command, answerCallback));
                    Monitor.Pulse(commandQ);
                }
            }
        }

        public class MessageAndAnswer
        {
            public readonly Message message;
            public readonly Action<Answer> answerCallback;

            public MessageAndAnswer(Message m, Action<Answer> a)
            {
                message = m;
                answerCallback = a;
            }
        }

        protected abstract bool ConnectNative(string ip, int port);
        protected abstract bool ConnectNative(Host host);
        protected abstract bool SendNative(string clientMessage);
        protected abstract string ReceiveNative();
        protected abstract void CloseNative();
    }
}
