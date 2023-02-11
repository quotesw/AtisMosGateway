using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text;
using AtisMosGateway;
//using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Collections.Generic;

public class WebServerThread
{
	private Thread wst;
	private MainForm mf;
    //private TextBox tb;

    private HttpListener listener;
    private bool runServer = true;

    public WebServerThread(MainForm mf, TextBox tb)
	{
		this.mf = mf;
        //this.tb = tb;

        mf.logText("Creat obiect WST");

		wst = new Thread(new ThreadStart(Listen));
		wst.Start();
        wst.IsBackground = true;

    }

    void Listen()
    {
        // Create a Http server and start listening for incoming connections
        listener = new HttpListener();
        listener.Prefixes.Add("http://+:8989/");
        listener.Start();
        //Console.WriteLine("Listening for connections on {0}", url);

        // Handle requests
        Task listenTask = HandleIncomingConnections();
        listenTask.GetAwaiter().GetResult();

        // Close the listener
        listener.Close();
    }

    async Task HandleIncomingConnections()
    {
        bool runServer = true;

        // While a user hasn't visited the `shutdown` url, keep on handling requests
        while (runServer)
        {
            // Will wait here until we hear from a connection
            HttpListenerContext ctx = await listener.GetContextAsync();

            mf.numHttpRequests++;

            // Peel out the requests and response objects
            HttpListenerRequest req = ctx.Request;
            HttpListenerResponse resp = ctx.Response;

            // Print out some info about the request
            //logText("Request #: {0}", ++requestCount);
            mf.logText(req.Url.ToString());
            //logText(req.HttpMethod);
            //logText(req.UserHostName);
            //logText(req.UserAgent);
            //logText(req.Url.Query);


            if ((req.Url.AbsolutePath == "/ams"))
            {
                mf.logText(req.QueryString["action"]);
            }

            string ret = "";

            if ((req.Url.AbsolutePath == "/rtv"))
            {
                ret = HttpGetString("http://romaniatv.net/");
                mf.logText(req.QueryString["action"]);
            }

            if ((req.Url.AbsolutePath == "/mos"))
            {
                if (req.HttpMethod == "POST" && req.HasEntityBody)
                {
                    System.IO.Stream body = req.InputStream; // here we have data

                    //body.Seek(0, SeekOrigin.Begin); 

                    var reader = new System.IO.StreamReader(body, req.ContentEncoding);
                    
                    var postBody = reader.ReadToEnd();

                    //mf.logText("postBody: " + postBody);
                    var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(postBody);

                    string mosID = values["mosID"];
                    MosPort mosPort = MosPort.MosUpper;
                    if (values["mosPort"] == "lower")
                    {
                        mosPort = MosPort.MosLower;
                    }

                    string mosData = values["mosData"];

                    mf.logText($"Post data [{mosID} / {mosPort}]: {mosData}");

                    mf.NcsToMosMessage(mosID, mosPort, mosData);
                    
                    


                }



            }


            // Write the response info
            byte[] data = Encoding.UTF8.GetBytes("Ai accesat: " + req.Url.AbsolutePath);
            resp.ContentType = "text/html";
            resp.ContentEncoding = Encoding.UTF8;
            resp.ContentLength64 = data.LongLength;

            // Write out to the response stream (asynchronously), then close it
            await resp.OutputStream.WriteAsync(data, 0, data.Length);
            resp.Close();
        }
    }

    string HttpGetString(string url)
    {
        string result;

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);


        // Set some reasonable limits on resources used by this request
     //   request.MaximumAutomaticRedirections = 4;
     //   request.MaximumResponseHeadersLength = 4;
        // Set credentials to use for this request.
     //   request.Credentials = CredentialCache.DefaultCredentials;
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

     //   Console.WriteLine("Content length is {0}", response.ContentLength);
     //   Console.WriteLine("Content type is {0}", response.ContentType);

        // Get the stream associated with the response.
        Stream receiveStream = response.GetResponseStream();

        // Pipes the stream to a higher level stream reader with the required encoding format.
        StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

        //Console.WriteLine("Response stream received.");
        //Console.WriteLine(readStream.ReadToEnd());

        result = readStream.ReadToEnd();

        response.Close();
        readStream.Close();

        return result;
    }


    /*public void Loop()
    {
		while (true)
        {
            //print ceva?

            mf.logText(Thread.CurrentThread.ManagedThreadId + " Threadu a rulat!");

			Thread.Sleep(1000);
        }
    }*/

    /*
    delegate void SetTextCallback(string text);
    */

    /*private void logText(string text)
    {
        // InvokeRequired required compares the thread ID of the
        // calling thread to the thread ID of the creating thread.
        // If these threads are different, it returns true.
        if (tb.InvokeRequired)
        {
            SetTextCallback d = new SetTextCallback(logText);
            //this.Invoke(d, new object[] { text });
            tb.Invoke(d, new object[] { text });
        }
        else
        {
            this.tb.Text += text + "\r\n";
        }
    }*/
}
