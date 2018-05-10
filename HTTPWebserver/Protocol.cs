using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HTTPWebserver
{
    public class Protocol
    {
        public async Task TryConnectAsync(TcpClient client)
        {

            //Get the stream to read from the client.
            NetworkStream stream = client.GetStream();
            Boolean statusLineReceived = false;
            Boolean badRequest = false;
            String response = "";
            String methodTokenGet = "GET";
            String statusLine = "";
            String contentType = "";
            String contentLength = "";
            String entityBody = "";

	        for (int i = 0; i < 20; i++)
	        {
		        Console.WriteLine(i);
	        }
            //Use a StreamReader and make sure it gets disposed properly.
            using (StreamReader sr = new StreamReader(stream))
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    string line;
                    //Read while we haven't reached the end of the stream yet.
                    while ((line = await sr.ReadLineAsync()) != null)
                    {
                        Console.WriteLine("Received: {0}", line);

                        if (!statusLineReceived)
                        {
                            // TODO check for relative paths /URI
                            statusLineReceived = true;
                            var statusWords = line.Split(" ");
                            // check if request method is GET otherwise this is a bad request
                            if (!statusWords[0].ToLower().Equals(methodTokenGet.ToLower()))
                            {
                                badRequest = true;
                                statusLine = "HTTP/1.0 400 Bad Request";
                            }
                            else
                            {
                                statusLine = "HTTP/1.0 200 OK";
                            }
                        }

                        if (!badRequest)
                        {
                            using (StreamReader str = new StreamReader("E:\\Avans\\jaar2\\2.4\\Computernetwerken2\\les3\\HTTPWebserverResponse.html"))
                            {
                                String allLines = await str.ReadToEndAsync();
                                contentLength = String.Format("Content-Length: {0}", ToBytesInInt(allLines));
                                contentType = "Content-Type: text/html; charset=UTF-8";
                                entityBody = allLines;
                            }

                        }


                        if (line.Equals("") || badRequest)
                        {
                            // TODO send one string instead of different lines
                            // now 3 CRLF are send as an entitybody with a bad request
                            await sw.WriteLineAsync(statusLine);
                            await sw.WriteLineAsync(contentType);
                            await sw.WriteLineAsync(contentLength);
                            await sw.WriteLineAsync("");
                            await sw.WriteLineAsync(entityBody);
                            //					        Console.WriteLine(sr.ReadLine());
                            break;
                        }

                    }
                }

            }

            //Properly close the stream and client, and stop listening for 	connections.
//            stream.Close();
//            client.Close();

            //            // try to accept a new pending request
            //            TryConnect(client);
        }

	    private int ToBytesInInt(String line)
	    {
		    return System.Text.Encoding.ASCII.GetByteCount(line);
	    }
    }
}
