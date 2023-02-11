using System;
using SimpleTCP;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace AtisMosGateway
{
    public partial class MainForm : Form
    {
        WebServerThread ws;
        public int numHttpRequests = 0;
        private ClientsManager clientsManager;

        SimpleTcpServer serverL;
        SimpleTcpServer serverU;

        public MainForm()
        {
            InitializeComponent();

            if (!startServers())
            {
                Environment.Exit(1);
            }

            ws = new WebServerThread(this, logBox);

            clientsManager = new ClientsManager();

            initMosClients();
        }

        delegate void SetTextCallback(string text);

        public void logText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (logBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(logText);
                //this.Invoke(d, new object[] { text });
                logBox.Invoke(d, new object[] { text });
            }
            else
            {
                //logBox.Text += text + "\r\n";
                logBox.AppendText(text + "\r\n");
            }
        }
        public void logText1(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (textBox1.InvokeRequired)
            {
                SetTextCallback d1 = new SetTextCallback(logText);
                //this.Invoke(d, new object[] { text });
                textBox1.Invoke(d1, new object[] { text });
            }
            else
            {
                //logBox.Text += text + "\r\n";
                textBox1.AppendText(text + "\r\n");
            }
        }

        public void setXPR_upper_on() {
            if (checkBox_xpr_upper.InvokeRequired)
            {
                //SetTextCallback
            }
            //checkBox_xpr_upper.Checked = true;
        }
        public void setXPR_lower_on()
        {
            //checkBox_xpr_lower.Checked = true;
        }
        private bool startServers()
        {
            bool startOk = true;

            serverL = new SimpleTcpServer();
            serverL.ClientConnected += tcpClientConnected;
            serverL.ClientDisconnected += tcpClientDisconnected;
            serverL.DataReceived += tcpServerDataReceived;
            try
            {
                serverL.Start(10540);
            }
            catch (InvalidOperationException e)
            {
                startOk = false;
                MessageBox.Show("Port 10540 is in use. AtisMosGateway will now exit!");
            }

            serverU = new SimpleTcpServer();
            serverU.ClientConnected += tcpClientConnected;
            serverU.ClientDisconnected += tcpClientDisconnected;
            serverU.DataReceived += tcpServerDataReceived;
            try
            {
                serverU.Start(10541);
            }
            catch (InvalidOperationException e)
            {
                startOk = false;
                MessageBox.Show("Port 10541 is in use. AtisMosGateway will now exit!");

            }

            return startOk;
        }

        private void initMosClients()
        {
            // XPRESSION MOS GW address 
            string mosgw_xpression_ip = Properties.Settings.Default.xpression_ip; //get ip from config
            if (mosgw_xpression_ip != "") { 
                MosClientThread xpMos = new MosClientThread(this, "XPRESSION", mosgw_xpression_ip, "CG", "XPRESSION");
                clientsManager.addClient(xpMos);
                label_xpr_ip.Text = mosgw_xpression_ip;
            }

            // PROMPTER MOS address
            string mosgw_prompter_ip = Properties.Settings.Default.prompter_ip; //get ip from config
            if (mosgw_prompter_ip != "") {
                MosClientThread xpPrpt = new MosClientThread(this, "PROMPTER", mosgw_prompter_ip, "CG", "PORTAPROMPT");
                clientsManager.addClient(xpPrpt);
            }

            // EVS MOSGW address
            string mosgw_evs_ip = Properties.Settings.Default.evs_ip; //get ip from config
            if (mosgw_evs_ip != "") {
                MosClientThread evsMos = new MosClientThread(this, "ipd.evs.mos", mosgw_evs_ip, "VIDEO", "EVS-XS");
                clientsManager.addClient(evsMos);
                label_evs_ip.Text = mosgw_evs_ip;
            }


        }

        private void tcpClientConnected(object sender, TcpClient tcpClient)
        {
            logText($"Client ({tcpClient.Client.RemoteEndPoint}) connected!");
            MosClientThread client = clientsManager.getClientByIp(getIPfromEndpoint(tcpClient.Client.RemoteEndPoint));
            if (client != null)
            {
                if (((SimpleTcpServer)sender) == serverL)
                {
                    client.inConL.Add(tcpClient);
                }

                if (((SimpleTcpServer)sender) == serverU)
                {
                    client.inConU.Add(tcpClient);
                }
            }
        }

        private void tcpClientDisconnected(object sender, TcpClient tcpClient)
        {
            logText($"Client ({tcpClient.Client.RemoteEndPoint}) disconnected!");
            MosClientThread client = clientsManager.getClientByIp(getIPfromEndpoint(tcpClient.Client.RemoteEndPoint));
            if (client != null)
            {
                if (((SimpleTcpServer)sender) == serverL)
                {
                    client.inConL.Remove(tcpClient);    
                }

                if (((SimpleTcpServer)sender) == serverU)
                {
                    client.inConU.Remove(tcpClient);
                }
            }
        }

        private void tcpServerDataReceived(object sender, SimpleTCP.Message msg)
        {
            //var ep = e.TcpClient.Client.RemoteEndPoint;
            MosClientThread client = clientsManager.getClientByIp(getIPfromEndpoint(msg.TcpClient.Client.RemoteEndPoint));
            if (client != null)
            {
                //avem mesaj de la clientul X (lower sau upper)

                var msgData = Encoding.BigEndianUnicode.GetString(msg.Data);

                if (((SimpleTcpServer)sender) == serverL)
                {
                    client.lastConnectionL = msg.TcpClient;
                    client.sendNcsMessage(MosPort.NcsLower, msgData);
                }
                
                if (((SimpleTcpServer)sender) == serverU)
                {
                    client.lastConnectionU = msg.TcpClient;
                    client.sendNcsMessage(MosPort.NcsUpper, msgData);
                }


            }
        }

        private string getIPfromEndpoint(EndPoint endPoint)
        {
            return ((IPEndPoint)endPoint).Address.ToString();
            //return IPAddress.Parse(((IPEndPoint)endPoint.Address.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ws = new WebServerThread(this, logBox);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblNumReq.Text = "Number of HTTP requests: " + numHttpRequests;
        }

        public string HttpPostMos(string mosID, MosPort mosPort, string mosData)
        {
            string result = "";
            //string url = "http://192.168.30.70/newsroom/mos/";
            string url = Properties.Settings.Default.newsroom_url;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            var postData = "mosID=" + Uri.EscapeDataString(mosID);
            postData += "&mosPort=" + Uri.EscapeDataString(mosPort.ToString());
            postData += "&mosData=" + Uri.EscapeDataString(mosData);

            var data = Encoding.ASCII.GetBytes(postData);

            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }


            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                response.Close();
            } catch (System.Net.WebException ex)   
            {
                logText(ex.Message);
                /*if (ex.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = ex.Response as HttpWebResponse;
                    if (response != null)
                    {
                        Console.WriteLine("HTTP Status Code: " + (int)response.StatusCode);
                    }
                    else
                    {
                        // no http status code available
                    }
                }
                else
                {
                    // no http status code available
                }*/
            } /*finally
            {
                    
            }*/

                



            return result;
        }

        public void NcsToMosMessage(string mosID, MosPort mosPort, string mosData)
        {
            MosClientThread client = clientsManager.getClientByMosID(mosID);

            if (client != null) {
                client.sendMosMessage(mosPort, mosData);
            }
        }

    }
}
