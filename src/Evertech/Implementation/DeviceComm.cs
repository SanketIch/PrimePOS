using Evertech.Interface;
using Microsoft.VisualBasic;
using NLog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Evertech.Implementation
{
    public class DeviceComm : IDeviceComm
    {
        #region Variable
        private string iPAdress = "0.0.0.0";
        private int portNum = 0;
        public bool isConnected = false;
        public Socket client = null;

        public String sECRResponse = String.Empty;
        private int iConnectionTimeout = 10000;
        private int iSendTimeout = 3000;
        private int iReceiveTimeout = 32000;
        private ManualResetEvent connectDone = new ManualResetEvent(false);
        private ManualResetEvent sendDone = new ManualResetEvent(false);
        private ManualResetEvent receiveDone = new ManualResetEvent(false);

        StateObject state = new StateObject();
        public string sImgData = "";
        public bool blExpectingImg = false;

        #endregion

        ILogger logger = LogManager.GetCurrentClassLogger();
        #region properties


        public int Port
        {
            get
            {
                return portNum;
            }

            set
            {
                portNum = value;
            }
        }

        public string IPaddress
        {
            get
            {
                return iPAdress;
            }

            set
            {
                iPAdress = value;
            }
        }

        #endregion
        public DeviceComm()
        {
            //state = new StateObject();
        }
        public bool Connect()
        {
            bool retVal = false;
            try
            {
                logger.Trace("ENTERED THE CONNECT METHOD IN DEVICECOMM");

                IPAddress ipAdd = IPAddress.Parse(IPaddress);
                IPEndPoint remoteEP = new IPEndPoint(ipAdd, Port);
                client = new Socket(ipAdd.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                // Connect to the remote endpoint.  
                client.BeginConnect(remoteEP, new AsyncCallback(ConnectCallback), client);
                retVal = connectDone.WaitOne(iConnectionTimeout);
                //reset the event
                connectDone.Reset();
                isConnected = retVal;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                //throw ex;
            }
            return retVal;
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                logger.Trace("Sent {0} bytes to server.", bytesSent); //?? Log TRACE MODE NLOT

                // Signal that all bytes have been sent.  
                sendDone.Set();
            }
            catch (Exception e)
            {
                logger.Fatal(e.ToString()); //?? Log ERROR MODE NLOT
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
        }

        public bool SendMessage(byte[] message)
        {
            logger.Trace("ENTERED THE SENDMESSAGE METHOD IN DEVICECOMM");
            bool retVal = false;
            byte[] recBytes = new byte[1024];
            try
            {
                state.sb.Clear();
                // Convert the string data to byte data using ASCII encoding.  
                byte[] byteData = message;

                // Begin sending the data to the remote device.  
                client.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), state);
                retVal = sendDone.WaitOne(iSendTimeout, false);
                sendDone.Reset();
            }
            catch (SocketException e)
            {
                recBytes = null;
                logger.Error(" Error in SendReceiveMessage : " + e.ToString());
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
            catch (Exception e)
            {
                recBytes = null;
                logger.Error(" Error in SendReceiveMessage : " + e.ToString());
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
            return retVal;
        }

        public bool ReceiveMessage(out string response)
        {
            logger.Trace("ENTERED THE RECEIVEMESSAGE METHOD IN DEVICECOMM");
            bool retVal = false;
            sECRResponse = "";
            try
            {
                client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                 new AsyncCallback(ReceiveCallback), state);

                retVal = receiveDone.WaitOne(iReceiveTimeout);
                receiveDone.Reset();
            }
            catch (Exception e)
            {
                logger.Fatal(e.ToString());
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
            response = sECRResponse;
            return retVal;
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            logger.Trace("ENTERED THE RECEIVECALLBACK METHOD IN DEVICECOMM");
            try
            {

                // Read data from the remote device.  
                int bytesRead = client.EndReceive(ar);

                if (bytesRead > 0)
                {
                    state.sb = new StringBuilder();
                    sECRResponse = "";
                    // There might be more data, so store the data received so far.  
                    state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));
                    sECRResponse = state.sb.ToString();
                }
                sImgData = "";
                if (blExpectingImg == true)
                {
                    for (int i = 0; i <= state.buffer.Length - 1; i++)
                    {
                        sImgData += (Strings.Chr(state.buffer[i]));
                    }
                }
                // Signal that all bytes have been received.  
                receiveDone.Set();
            }
            catch (Exception e)
            {
                logger.Fatal(e.ToString());
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
        }

        public void ConnectCallback(IAsyncResult ar)
        {
            logger.Trace("ENTERED THE CONNECTCALLBACK METHOD IN DEVICECOMM");
            try
            {
                // Retrieve the socket from the state object.  
                client = (Socket)ar.AsyncState;

                // Complete the connection.  
                client.EndConnect(ar);

                logger.Trace("Socket connected to {0}", client.RemoteEndPoint.ToString());
                isConnected = true;


                //this.callback.Invoke(ar);
                // Signal that the connection has been made.  
                connectDone.Set();


            }
            catch (Exception e)
            {
                logger.Fatal(e.ToString());
                //MessageBox.Show(e.Message, "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                //throw e;
            }
        }

        public void Disconnect()
        {
            client.Shutdown(SocketShutdown.Both);
            client.Close();

            Thread.Sleep(120);//Because of the Socket Issue
        }
    }
    public class StateObject
    {
        // Client socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 8000;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }
}
