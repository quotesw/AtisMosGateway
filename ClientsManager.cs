using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using SimpleTCP;

namespace AtisMosGateway
{
    public enum MosPort
    {
        MosLower,
        MosUpper,
        NcsLower,
        NcsUpper
    }

    class MosClientThread
    {
        private Thread mct;
        private MainForm mf;

        public string mosID;
        public string address;
        public int portL = 10540;
        public int portU = 10541;
        public string role;
        public string comType;

        private int msgID_M_L = 0;
        private int msgID_M_U = 0;
        private int msgID_N_L = 0;
        private int msgID_N_U = 0;

        public bool shutdown = false;

        public TcpClient lastConnectionL;
        public TcpClient lastConnectionU;  

        public List<TcpClient> inConL = new List<TcpClient>();
        public List<TcpClient> inConU = new List<TcpClient>();

        public SimpleTcpClient outConL;
        public SimpleTcpClient outConU;

        public MosClientThread(MainForm mf, string mosID, string address, string role, string comType )
        {
            this.mf = mf;
            this.mosID = mosID;
            this.address = address;
            this.role = role;
            this.comType = comType;

            this.mf.logText("Creat obiect MosClientThread pentru id: " + mosID);

            outConL = new SimpleTcpClient();
            outConU = new SimpleTcpClient();

            outConL.DataReceived += tcpClientDataReceived;
            outConU.DataReceived += tcpClientDataReceived;

            mct = new Thread(new ThreadStart(processMessages));
            mct.Start();
            mct.IsBackground = true;

            //mf.logText(outConL.TcpClient.Connected.ToString());


        }

        private void tcpClientDataReceived(object sender, SimpleTCP.Message msg)
        {
            var msgData = Encoding.BigEndianUnicode.GetString(msg.Data);

            if (((SimpleTcpClient)sender) == outConL)
            {
                sendNcsMessage(MosPort.MosLower, msgData);
            }

            if (((SimpleTcpClient)sender) == outConU)
            {
                sendNcsMessage(MosPort.MosUpper, msgData);
            }
        }

        private void processMessages()
        {

            try
            {
                outConL.Connect(address, portL);
            } catch (System.Net.Sockets.SocketException e)
            {
                mf.logText($"eroare conectare la {this.address}:{this.portL}: {e.Message}");
            }
            try
            {
                outConU.Connect(address, portU);
            }
            catch (System.Net.Sockets.SocketException e)
            {
                mf.logText($"eroare conectare la {this.address}:{this.portU}: {e.Message}");
            }

            while (!shutdown)
            {


                if (outConL.TcpClient != null)
                {
                    if ((outConL.TcpClient.Connected == false) | (isSocketConnected(outConL.TcpClient.Client) == false))
                    {
                        try
                        {
                            outConL.Disconnect();
                            outConL.Connect(address, portL);
                        }
                        catch (System.Net.Sockets.SocketException e)
                        {
                            mf.logText($"eroare conectare la {this.address}:{this.portL}: {e.Message}");
                            
                        }

                    }
                    else
                    {
                        mf.logText("conectat la " + mosID + " L");
                        if (mosID == "XPRESSION") {
                            //mf.setXPR_lower_on();
                        }
                    }
                } else
                {
                    mf.logText("conexiune NULL " + mosID + " L");
                }

                if (outConU.TcpClient != null)
                {
                    if ((outConU.TcpClient.Connected == false) | (isSocketConnected(outConU.TcpClient.Client) == false))
                    //if (outConU.TcpClient.Connected == false)
                    {
                        try
                        {
                            outConU.Disconnect();
                            outConU.Connect(address, portU);
                        }
                        catch (System.Net.Sockets.SocketException e)
                        {
                            mf.logText($"eroare conectare la {this.address}:{this.portU}: {e.Message}");
                        }
                        
                    }
                    else
                    {
                        mf.logText("conectat la " + mosID + " U");
                        if (mosID == "XPRESSION")
                        {
                            mf.setXPR_upper_on();
                        }
                    }
                }
                else
                {
                    mf.logText("conexiune NULL " + mosID + " U");
                }

                mf.logText("iteram threadu!");
                mf.logText(inConU.Count.ToString()); 

                System.Threading.Thread.Sleep(10000);
            }
        }

        public void sendMosMessage(MosPort mosPort, string message)
        {
            TcpClient client = null;
            int msgID = 0;

            if (this.comType == "XPRESSION")
            {
                if ((mosPort == MosPort.MosLower) && (outConL != null))
                {
                    client = outConL.TcpClient;
                    msgID = msgID_N_L++;
                }
                if ((mosPort == MosPort.MosUpper) && (outConU != null))
                {
                    client = outConU.TcpClient;
                    msgID = msgID_N_U++;

                }
            } else if (this.comType == "PORTAPROMPT")
            {
                if ((mosPort == MosPort.MosLower) && (lastConnectionL != null))
                {
                    client = lastConnectionL;
                    msgID = msgID_M_L++;

                }
                if ((mosPort == MosPort.MosUpper) && (lastConnectionU != null))
                {
                    client = lastConnectionU;
                    msgID = msgID_M_U++;
                }
            }

            if (client != null)
            {
                //replace MESSAGEID cu msgID;
                message = message.Replace("MESSAGEID", msgID.ToString());
                Byte[] data = System.Text.Encoding.BigEndianUnicode.GetBytes(message);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);

                // Receive the TcpServer.response.
                mf.logText("Trimis catre client " + this.mosID + ", port: " + mosPort.ToString() + ":");
                mf.logText(message);

                // Buffer to store the response bytes.
                //data = new Byte[256];

                // String to store the response ASCII representation.
                //String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                //Int32 bytes = stream.Read(data, 0, data.Length);
                //responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                //mf.logText("RE: " + responseData);

                // Close everything.
                //stream.Close();
            }

        }

        public void sendNcsMessage(MosPort mosPort, string message)
        {
            mf.logText1(mosID + "(" + mosPort.ToString() + ")" + ":");
            mf.logText1(message);

            string ncsres = mf.HttpPostMos(mosID, mosPort, message);

            mf.logText1("Rezultat raspuns NCS (dupa mos cerere MOS):");
            mf.logText1(ncsres);

        }

        public bool isSocketConnected(Socket client) 
        {
            bool connected = true;

            bool blockingState = client.Blocking;
            try
            {
                byte[] tmp = new byte[1];

                client.Blocking = false;
                client.Send(tmp, 0, 0);
                Console.WriteLine("Connected!");
            }
            catch (SocketException e)
            {
                // 10035 == WSAEWOULDBLOCK
                if (e.NativeErrorCode.Equals(10035))
                {
                    Console.WriteLine("Still Connected, but the Send would block");
                }
                else
                {
                    Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
                    connected = false;
                }
            }
            finally
            {
                client.Blocking = blockingState;
            }
            return connected;
        }
        
    }

    internal class ClientsManager
    {
        private List<MosClientThread> clients = new List<MosClientThread>();

        public void addClient(MosClientThread client)
        {
            clients.Add(client);
        }

        public MosClientThread getClientByIp(string Address)
        {
            MosClientThread result = null;
            foreach (MosClientThread client in clients)
            {
                if (client.address.Equals(Address))
                {
                    result = client;
                    break;
                }
            }
            return result;
        }

        public MosClientThread getClientByMosID(string mosID)
        {
            MosClientThread result = null;
            foreach (MosClientThread client in clients)
            {
                if (client.mosID.Equals(mosID))
                {
                    result = client;
                    break;
                }
            }
            return result;
        }
    }
}
