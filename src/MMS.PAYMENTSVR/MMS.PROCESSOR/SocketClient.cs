//Author : Ritesh 
////Author : Ritesh 
//Copy Right : © Micro Merchant Systems, Inc 2008
//Functionality Desciption : The purpose of this class is to make base processor for transactions.
//External functions:None   
//Known Bugs : None
//Start Date : 2 Feb 2008.
//Functionality Desciption : The purpose of this class is to make connection with PC Charge Payment Server with Socket 
//External functions:None   
//Known Bugs : None
//Start Date : 04 January 2008.
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using NLog;

namespace MMS.PROCESSOR
{
    //Author : Ritesh 
    //CopyRight: MMS 2008
    //Functionality Desciption : The purpose of this class is to make connection with PC Charge Payment Server with Socket 
    //External functions:None   
    //Known Bugs : None
    //Start Date : 04 January 2008.
    public class SocketClient: IDisposable
    {
        private ILogger logger = LogManager.GetCurrentClassLogger();
        #region variables
        private String IpAddress = String.Empty;
        private int PortNumber = 0;
        //private Socket Connection = null;
        private TcpClient Connection = null;
        private Byte[] Data = null;
        private bool Connected = false;
        private String ErrorMessage = String.Empty;
        private Stream XMLSendRecv = null;
        private Boolean Disposed = false;
        #endregion

        #region constants
        private const int MSG_LENGTH = 1024;
        private const String CONNECTION_MSG = "Please connect to the server first";
        
        #endregion
        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This is constructor of SocketClient class
        /// Known Bugs : None
        /// Start Date : 04 November 2007.
        /// </summary>
        /// <param name="String address"></param>
        /// <param name="int portNo"></param>
        public SocketClient(String address, int portNo)
        {
            logger.Trace("In SocketClient() constructor");
            IpAddress = address;
            PortNumber = portNo;
            Connected = false;
            //Connection = new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.IP);
            Connection = new TcpClient();

            Data = new Byte[MSG_LENGTH];
        }
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This connects to the PCCharge Server
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <param name="address"></param>
        /// <param name="portNo"></param>
        /// <returns>bool success</returns>
        public bool Connect(String address, int portNo)
        {
            logger.Trace("In Connect("+address+","+portNo.ToString()+")");
            if (Connection == null)
                return false;
            if (IsConnected())
                return true;
            try
            {
                //Check for validity of the parameters
                if (address != "")
                    IpAddress = address;
                else if (IpAddress == "")
                    return false;
                if (portNo != 0)
                    PortNumber = portNo;
                else if (PortNumber == 0)
                    return false;

                Connection.Connect(IpAddress, PortNumber);
                if (Connection.Connected)
                {
                    Connected = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //Log error
                //E//rrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;

                return false;
            }
            return Connected; 
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This disconnects from the PCCharge Server
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <returns>bool success</returns>
        public bool Disconnect()
        {
            logger.Trace("In Disconnect()");
            if (Connection == null)
                return false; 
            try
            {
                Connection.Close();
                Connected = false;
            }
            catch (Exception ex)
            {
                //Log error
                //ErrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;
                logger.Error(ex, ex.Message);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This sends data ot the PCCharge Server
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <param name="request"></param>
        /// <returns>int length</returns>
        public int Send(String request)
        {
            logger.Trace("In Send("+request+")");
            if (Connection == null)
                return 0;
            Byte[] sendData = null;
            //check whether connected
            if (!IsConnected())
            {
                ErrorMessage = CONNECTION_MSG;
                return 0;
            }
            try
            {
                sendData = System.Text.Encoding.ASCII.GetBytes(request);
                //Connection.Send(sendData);              
                XMLSendRecv = Connection.GetStream();
                XMLSendRecv.Write(sendData, 0, sendData.Length);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //Log exception
                //ErrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;
                return 0;
            }
            return sendData.Length;
        }
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This receives data from the PCCharge Server
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <param name="responseData"></param>
        /// <returns>int length</returns>
        public int Receive(ref String responseData)
        {
            logger.Trace("In Recieve()");
            if (Connection == null)
                return 0;
            Int32 recBytes = 0;
            //Check for validity of the connection
            if (!IsConnected())
            {
                ErrorMessage = CONNECTION_MSG;
                return 0;
            }
            try
            {
                Data.Initialize();
                // String to store the response ASCII representation.
                responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                //recBytes = Connection.Receive(Data);
                recBytes = XMLSendRecv.Read(Data, 0, Data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(Data, 0, recBytes);

            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //Log exception
                //ErrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;
                return 0;
            }
            return recBytes;
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method tries to reconnect to the PCCharge Server
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <returns>bool status</returns>
        public bool Reconnect()
        {
            logger.Trace("In Reconnect()");
            if (Connection == null)
                return false;
            try
            {
                if (IsConnected())
                    Disconnect();
                Connect(IpAddress,PortNumber);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                //ErrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;
                return false;
            }
            return Connected;
        }
        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This method checks whether the connection is alive
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        /// <returns>bool status</returns>        
        public bool IsConnected()
        {
            logger.Trace("In IsConnected()");
            if (Connection == null)
                return false;
            try
            {
                Connected = Connection.Connected;
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
               // ErrorMessage = "Source:=" + ex.Source + "Message:-" + ex.Message;
                return false;
            }
            return Connected;
        }
        //Property Error;

        public string Error
        {
            get
            {
                return ErrorMessage;
            }
            set
            {
                ErrorMessage = value;
            }
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : Destructor
        /// External functions:Methods of Socket Class.  
        /// Known Bugs : None
        /// Start Date : 04 January 2008.
        /// </summary>
        ~SocketClient()
        {
            logger.Trace("SocketClient destructor\n");
            Dispose(false);
        }


        #region IDisposable Members
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(Boolean disposing)
        {
            if (Disposed)
                return;
            if (disposing)
            {
                IpAddress = String.Empty;
                PortNumber = 0;
                if (XMLSendRecv != null)
                {
                    XMLSendRecv.Close();
                    XMLSendRecv = null;
                }
                if (Connection != null)
                {
                    Connection.Close();
                    Connection = null;
                }
                Connected = false;
                ErrorMessage = null;
            }
            // Unmanaged cleanup code here
            Disposed = true;
        }

        #endregion
    }

 }
