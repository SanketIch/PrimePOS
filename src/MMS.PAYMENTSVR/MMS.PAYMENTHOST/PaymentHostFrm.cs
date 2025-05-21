///Author : Ritesh 
///CopyRight: MMS 2008
///Functionality Desciption : The purpose of this class is to start the Host.
///External functions:None   
///Known Bugs : None
///Start Date : 22 Feb 2008.


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace MMS.PAYMENTHOST
{


    public partial class PaymentHostFrm : Form
    {
        private delegate void AddListItem(String myString);


        #region constants
        private const int MAX_DATA_SIZE = 1024;
        private const String PORTNO = "PORTNO";
        private const String MAX_CONNECTIONS = "MAX_CONNECTIONS";
        private const String MAX_BACKLOG = "MAX_BACKLOG";
        private const String PYMT_ADDRESS = "PYMT_ADDRESS";
        private const String PYMT_PORT = "PYMT_PORT";
        private const String PYMT_SVR_CONNECT_FAILURE = "Could not establish connection with Payament Server";
        private const String PYMT_SVR_SEND_FAIL = "Counld not send data to Payment Server";
        private const String PYMT_SVR_RECEIVE_FAIL = "Counld not receive data from Payment Server";
        private const String SVR_DOWN = "Server Down";
        private const String SVR_STARTED = "Server Started";
        private const string INVALIDPAYMENTPROCESSOR = "INVALID PAYMENT PROCESSOR SELECTION"; //Added By Dharmendra (SRT) on Dec-15-08
        #endregion

        #region variables
        private Thread ListenerThread; //Thread to lisen for new connections
        private Thread PaymentThread; //Thread to process payment requests for new connections
        private Socket Listener;      // Listener Socket  
        private bool isStopped = false;
        private MMSQueue ClientQueue = null; //Synchronous Queue object.
        private String PymntServerAddress = null; //PaymentServer IP
        private int PymntServerPort = 0; //PaymentServer Port
        private AddListItem myDelegate;
        private bool exitApplication = false;

        String ProcessorName = string.Empty;

        #endregion

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This is the Main Form that controls the MMS Payment Server.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        public PaymentHostFrm()
        {
            InitializeComponent();
            ClientQueue = new MMSQueue();
            //myDelegate = new AddListItem(AddListItemMethod);
            ProcessorName = ConfigurationManager.AppSettings.GetValues("PYMT_PROCESSOR")[0];
            this.Text = this.Text + " : " + ProcessorName;
        }

        public void AddListItemMethod(String myString)
        {
            listStatus.Items.Add(myString);
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This is click event of button that starts the Socket Server. This method starts the socket server
        /// thread and the payment thread.
        /// External functions:Methods of Thread, Socket Class.  
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <param name="sender">object</param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        private void StartServer()
        {
            try
            {
                btnStart.Enabled = false;
                btnStop.Enabled = true;
                isStopped = false;
                //Create a new ServerSocket
                Listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Listener.Bind(new IPEndPoint(IPAddress.Any, Convert.ToInt32(ConfigurationManager.AppSettings.GetValues(PORTNO)[0])));
                Listener.Listen(Convert.ToInt32(ConfigurationManager.AppSettings.GetValues(MAX_BACKLOG)[0]));
                Listener.Blocking = true;
                //Spawn a thread to ensure the blocking is done in separate thread then the main thread.
                ListenerThread = new Thread(new ParameterizedThreadStart(StartListening));
                ListenerThread.Start(Listener);

                PymntServerAddress = ConfigurationManager.AppSettings.GetValues(PYMT_ADDRESS)[0];
                PymntServerPort = Convert.ToInt32(ConfigurationManager.AppSettings.GetValues(PYMT_PORT)[0]);
                //Spawn a thread to ensure the processing of the requests is done besides the main thread or Server thread.
                PaymentThread = new Thread(new ThreadStart(PaymentProcessing));
                PaymentThread.Start();
                listStatus.Items.Add(SVR_STARTED + "  : " + DateTime.Now.ToString());
            }
            catch (Exception)
            {
                CleanUp();
                isStopped = false;
                btnStart.Enabled = true;
                btnStop.Enabled = false;
                SetStatus(SVR_DOWN);
            }
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This is a Thread function for processing the Payment processing.
        /// This function checks for any outstanding requests and ensures that the request is processed by the Payment Server.
        /// External functions:Methods of SocketClient, Socket,MMSQueue Class.  
        /// Known Bugs : None
        /// Start Date : 23 February 2008.
        /// </summary>
        protected void PaymentProcessing()
        {
            String responseMessage = null;
            Byte[] data = new Byte[MAX_DATA_SIZE];
            while (!isStopped)
            {
                if (ClientQueue.Count() == 0)
                    Thread.Sleep(1); //Changed to 1 from 0 to avoid CPU HOG (SRT).
                else
                {
                    //Create a connection to the PaymentServer
                    SocketClient paymentSocket = new SocketClient(PymntServerAddress, PymntServerPort);
                    PaymentClient pmtClient = (PaymentClient)ClientQueue.Dequeue();

                    //Connect the Payment Server.
                    if (!paymentSocket.Connect(PymntServerAddress, PymntServerPort))
                    {
                        SetStatus(PYMT_SVR_CONNECT_FAILURE);
                        paymentSocket.Dispose();
                        pmtClient.ClientSocket.Close();
                        paymentSocket = null;
                        pmtClient.ClientSocket = null;
                        isStopped = true;
                        continue;
                    }

                    //Send the request of POS Client to the PaymentServer.
                    if (paymentSocket.Send(pmtClient.ReqMessage) == 0)
                    {
                        SetStatus(PYMT_SVR_SEND_FAIL);
                        paymentSocket.Disconnect();
                        paymentSocket.Dispose();
                        pmtClient.ClientSocket.Close();
                        pmtClient.ClientSocket = null;
                        paymentSocket = null;
                        isStopped = true;
                        continue;
                    }

                    //Receive the response from the PaymentServer.
                    if (paymentSocket.Receive(ref responseMessage) <= 0)
                    {
                        SetStatus(PYMT_SVR_RECEIVE_FAIL);
                        paymentSocket.Disconnect();
                        paymentSocket.Dispose();
                        pmtClient.ClientSocket.Close();
                        pmtClient.ClientSocket = null;
                        paymentSocket = null;
                        continue;
                    }
                    data = System.Text.Encoding.ASCII.GetBytes(responseMessage);

                    //if the Client socket is connected then only send the data.
                    if (pmtClient.ClientSocket.Connected)
                    {
                        if (pmtClient.ClientSocket.Send(data) == 0)
                        {
                            //errorMessage += "SEND FAILED:-" + PaymentConn.Error;
                            pmtClient.ClientSocket.Close();
                            paymentSocket.Disconnect();
                            paymentSocket.Dispose();
                            pmtClient.ClientSocket = null;
                            paymentSocket = null;
                            continue;
                        }

                        pmtClient.ClientSocket.Close();
                    }

                    if (!paymentSocket.Disconnect())
                    {
                        //errorMessage += "DISCONNECT FAILED:-" + PaymentConn.Error;
                        paymentSocket.Dispose();
                        paymentSocket = null;
                        pmtClient.ClientSocket = null;
                        return;
                    };
                    pmtClient.ClientSocket = null;
                    paymentSocket = null;
                }
                if (isStopped)
                {
                    if (ClientQueue != null)
                    {
                        while (ClientQueue.Count() > 0)
                        {
                            PaymentClient client = (PaymentClient)ClientQueue.Dequeue();
                            client.ClientSocket.Close();
                            client.ClientSocket = null;
                            client = null;
                        }

                    }
                }
            }

            //btnStart.Enabled = true;
            //btnStop.Enabled = false;
            SetStatus(SVR_DOWN);

        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This is a Thread function for processing the POS Client Requests for Payment Processing.
        /// This function blocks for any connections being called and accepts the connections and queues the request and connections for PaymentServer to process.
        /// External functions:Methods of SocketClient, Socket,MMSQueue Class.  
        /// Known Bugs : None
        /// Start Date : 23 February 2008.
        /// </summary>
        protected void StartListening(object serverSocketObj)
        {

            Socket serverSocket = (Socket)serverSocketObj;
            while (!isStopped)
            {
                Socket clientSocket = null;
                try
                {
                    //wait for a client request
                    clientSocket = serverSocket.Accept();
                    //client request has arrived
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        //run the client request
                        Byte[] bytes = null;
                        String data = null;
                        bytes = new byte[MAX_DATA_SIZE];
                        //receive data.
                        int bytesRec = clientSocket.Receive(bytes);
                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
                        if (data.Length > 0)
                        {
                            // Added By Dharmendra (SRT) on Dec-15-08 to check the request payment server port address & payment host server port address
                            string[] requestValueArray = data.Split(Convert.ToChar("|"));
                            if (requestValueArray.Length > 1)
                            {
                                if (requestValueArray[0].ToString() != ProcessorName.Trim()) //if Payment Processor Names are different, then send error message back to client
                                {
                                    Byte[] bytesSend = System.Text.Encoding.ASCII.GetBytes(INVALIDPAYMENTPROCESSOR);
                                    clientSocket.Send(bytesSend);
                                    clientSocket.Close();
                                    clientSocket = null;
                                }
                                else
                                {
                                    //Create new object of PaymentClient and push it in the request Queue.
                                    PaymentClient client = new PaymentClient(clientSocket, requestValueArray[1].ToString());
                                    ClientQueue.Enqueue(client);
                                }
                            }
                            else
                            {
                                //Create new object of PaymentClient and push it in the request Queue.
                                PaymentClient client = new PaymentClient(clientSocket, requestValueArray[0].ToString());
                                ClientQueue.Enqueue(client);
                            }
                            //Added & Modified Till Here Dec-15-08                          

                        }
                    }
                }
                catch (SocketException)//exception thrown when closing a socket
                {
                    isStopped = true;
                    clientSocket = null;
                }
            }

        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This function is used to cleanup the threads etc
        /// thread and the payment thread.
        /// External functions:Methods of Thread, Socket Class.  
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        private void CleanUp()
        {
            if (Listener != null)
            {
                Listener.Close();
                Listener = null;
            }

            if (ListenerThread != null)
            {
                //ListenerThread.Join();
                ListenerThread.Abort();
                ListenerThread = null;
            }

            if (PaymentThread != null)
            {
                //PaymentThread.Join();
                PaymentThread.Abort();
                PaymentThread = null;
            }
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This is click event of button that stops the Socket Server. This method stops the socket server
        /// thread and the payment thread.
        /// External functions:Methods of Thread, Socket Class.  
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        /// <param name="e">EventArgs</param>
        /// <param name="sender">object</param>
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopServer();
        }

        private void StopServer()
        {
            btnStart.Enabled = true;
            btnStop.Enabled = false;

            isStopped = true;
            CleanUp();
            listStatus.Items.Add(SVR_DOWN + "  : " + DateTime.Now.ToString());
            if (ClientQueue != null)
            {
                while (ClientQueue.Count() > 0)
                {
                    PaymentClient client = (PaymentClient)ClientQueue.Dequeue();
                    client.ClientSocket.Close();
                }
            }
        }

        /// <summary>
        /// Author : Ritesh
        /// Functionality Desciption : This is click event of button that clears the list of messages
        /// External functions:None.  
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearStatusList();
        }

        private void ClearStatusList()
        {
            listStatus.Items.Clear();
        }

        /// <summary>
        /// Author : Ritesh 
        /// Functionality Desciption : This function is used to set status cross multiple threads.
        /// External functions:None
        /// Known Bugs : None
        /// Start Date : 23 Feb 2008.
        /// </summary>
        private void SetStatus(string text)
        {
            if (this.listStatus.InvokeRequired)
            {
                this.Invoke(this.myDelegate, new object[] { text });
            }
            else
            {
                this.listStatus.Items.Add(text);
            }

        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method calls StartServer method to start payment server
        /// External functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sTARTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartServer();
        }
        /// <summary>
        /// Author: Dharmendra
        /// Functionality Description : This method calls StopServer() method stop to stop the payment server
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sTOPToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopServer();
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Desctiption : This method calls ClearStatusList() method to clear the rich text box
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cLEARToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearStatusList();
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method maximizes the payment server window
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mAXIMIZEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Visible = true;
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.PerformLayout();
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method hides & minimize the server window
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mINIMIZEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.ShowInTaskbar = false;
            this.WindowState = FormWindowState.Minimized;
            notifyIconHstPmtSrv.BalloonTipText = "PrimeCharge Payment server is minimized in system tray. To close the application, please right click the icon and click on EXIT";
            notifyIconHstPmtSrv.BalloonTipTitle = "PrimeCharge Payment Server";
            notifyIconHstPmtSrv.ShowBalloonTip(10);

        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method stops the payment server & exit from application
        /// External Functions :None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void eXITToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopServer();
            exitApplication = true;
            this.Close();
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method hides the server window when it is minimized
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PmtHstServer_Minimized(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                this.Hide();
            }
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : 
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void FrmClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.WindowsShutDown || exitApplication == true)
            {
                e.Cancel = false;
            }
            else
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
                notifyIconHstPmtSrv.BalloonTipText = "PrimeCharge Payment server is minimized in system tray. To close the application, please right click the icon and click on EXIT";
                notifyIconHstPmtSrv.BalloonTipTitle = "PrimeCharge Payment Server";
                notifyIconHstPmtSrv.ShowBalloonTip(10);
                Visible = false;
            }
        }
        /// <summary>
        /// Author : Dharmendra
        /// Functionality Description : This method kills the payment server process thread
        /// from the task manager
        /// External Functions : None
        /// Known Bugs : None
        /// Start Date : 06 May 08
        /// </summary>
        private void KillProcess()
        {
            Process[] pArry = Process.GetProcesses();

            foreach (Process p in pArry)
            {
                string s = p.ProcessName;
                //s = s.ToLower();
                if (s.CompareTo("MMS.PAYMENT_CCPROCESS") == 0)
                {
                    p.Kill();
                }
            }

        }

        private void PaymentHostFrm_Load(object sender, EventArgs e)
        {
            StartServer();
            this.ShowInTaskbar = false; //Added this line to make it minimized.
            this.WindowState = FormWindowState.Minimized;
            notifyIconHstPmtSrv.BalloonTipText = "MMS payment processing server is started & is minimized in system tray. To close the application, please right click the icon and click on EXIT";
            notifyIconHstPmtSrv.BalloonTipTitle = "MMS Payment Processing Server";
            notifyIconHstPmtSrv.ShowBalloonTip(10);
            Visible = false;
        }

    }
}