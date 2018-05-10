using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPWebserver
{
	public class MyThread
	{

		public void Thread1()
		{
			for (int i = 0; i < 10; i++)
			{
				Console.WriteLine("Hello world " + i);
				Thread.Sleep(1);
			}
		}
	}

    class Program
    {

        static void Main(string[] args)
        {
			var protocol = new Protocol();
	        CancellationTokenSource cts = new CancellationTokenSource();
            //Create a TCPListener, locally.
            var sl = new TcpListener(IPAddress.Parse("127.0.0.1"), 8080);
	        //Start listening for connections.
	        sl.Start();
            while (true)
            {
	            AcceptClientsAsync(sl, protocol);
	            
            }
        }

	    static async Task AcceptClientsAsync(TcpListener sl, Protocol protocol)
	    {
		    TcpClient client = await sl.AcceptTcpClientAsync();
			await protocol.TryConnectAsync(client);
        }

    }
}
