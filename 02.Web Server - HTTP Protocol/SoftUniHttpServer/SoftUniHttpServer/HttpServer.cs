namespace SoftUniHttpServer
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;

    public class HttpServer : IHttpServer
    {
        private TcpListener tcpListener;
        private bool isWorking;

        public HttpServer()
        {
            this.tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 80);
        }

        public void Start()
        {
            this.tcpListener.Start();
            this.isWorking = true;
            while (this.isWorking)
            {
                TcpClient client = this.tcpListener.AcceptTcpClient();
                var buffer = new byte[10240];
                NetworkStream stream = client.GetStream();
                var readLength = stream.Read(buffer, 0, buffer.Length);
                var requestText = Encoding.UTF8.GetString(buffer, 0, readLength);

                Console.WriteLine(new string('-', 60));
                Console.WriteLine(requestText);

                var responseText = File.ReadAllText("index.html");
                var responseBytes = Encoding.UTF8.GetBytes(
                    "HTTP/1.0 200 OK" + Environment.NewLine
                    ////+ "Location: https://softuni.bg" + Environment.NewLine + Environment.NewLine
                    + "Content-Type: text/html" + Environment.NewLine
                    ////+ "Content-Disposition: attachment; filename=index.html" + Environment.NewLine
                    + "Content-Length: " + responseText.Length + Environment.NewLine + Environment.NewLine
                    + responseText
                    );

                stream.Write(responseBytes); 
            }
        }

        public void Stop()
        {
            this.isWorking = false;
        }
    }
}