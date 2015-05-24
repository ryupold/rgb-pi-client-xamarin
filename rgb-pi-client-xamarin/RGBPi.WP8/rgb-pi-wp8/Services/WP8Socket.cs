﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RGBPi.Core;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace RGB.Services
{
    public class WP8Socket : IWP8Socket
    {

        private Socket _socket = null;


        // Signaling object used to notify when an asynchronous operation is completed
        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        // Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        // timeout period, the call is aborted.
        const int TIMEOUT_MILLISECONDS = 5000;

        // The maximum size of the data buffer to use with the asynchronous socket methods
        const int MAX_BUFFER_SIZE = 2048;



        protected override void CloseNative()
        {
            if (_socket != null)
            {
                _socket.Close();
            }
        }

        protected override bool ConnectNative(Host host)
        {
            return ConnectNative(host.ip, host.port);
        }

        protected override bool ConnectNative(string ip, int port)
        {
            try {
                string result = string.Empty;

                // Create DnsEndPoint. The hostName and port are passed in to this method.
                DnsEndPoint hostEntry = new DnsEndPoint(ip, port);

                // Create a stream-based, TCP socket using the InterNetwork Address Family. 
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create a SocketAsyncEventArgs object to be used in the connection request
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = hostEntry;

                // Inline event handler for the Completed event.
                // Note: This event handler was implemented inline in order to make this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                // Retrieve the result of this request
                result = e.SocketError.ToString();

                // Signal that the request is complete, unblocking the UI thread
                _clientDone.Set();
                });

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Connect request over the socket
                _socket.ConnectAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override string ReceiveNative()
        {
            string response = "Operation Timeout";

            // We are receiving over an established socket connection
            if (_socket != null)
            {
                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;

                // Setup the buffer to receive the data
                socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

                // Inline event handler for the Completed event.
                // Note: This even handler was implemented inline in order to make 
                // this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        // Retrieve the data from the buffer
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

                    _clientDone.Set();
                });

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Receive request over the socket
                _socket.ReceiveAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
                return response;
            }
            else
            {
                return null;
            }
        }

        protected override bool SendNative(string clientMessage)
        {
            // We are re-using the _socket object initialized in the Connect method
            if (_socket != null)
            {
                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                // Set properties on context object
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                socketEventArg.UserToken = null;

                // Inline event handler for the Completed event.
                // Note: This event handler was implemented inline in order 
                // to make this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    // Unblock the UI thread
                    _clientDone.Set();
                });

                // Add the data to be sent into the buffer
                byte[] payload = Encoding.UTF8.GetBytes(clientMessage);
                socketEventArg.SetBuffer(payload, 0, payload.Length);

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Send request over the socket
                _socket.SendAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
