using Client.Netcode;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace GameClient
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Enter the server IP Address:");
			string serverIP = "192.168.29.11";

			Console.WriteLine("Enter the server Port:");
			int serverPort = 5000;

			try
			{
				TcpClient client = new TcpClient(serverIP, serverPort);
				Console.WriteLine("Connected to the server.");

				Thread thread = new Thread(o => ReceiveData((TcpClient)o));
				thread.Start(client);

				string s;
				while (!string.IsNullOrEmpty((s = Console.ReadLine())))
				{
					Packet paket = new Packet();
					paket.WriteString(s);
					byte[] buffer = paket.ToArray();
					client.GetStream().Write(buffer, 0, buffer.Length);
				}

				client.Client.Shutdown(SocketShutdown.Send);
				thread.Join();
			}
			catch (Exception ex)
			{
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
			finally
			{
				Console.WriteLine("Disconnected from server.");
			}
		}

		static void ReceiveData(TcpClient client)
		{
			try
			{
				NetworkStream stream = client.GetStream();
				byte[] receivedBytes = new byte[1024];
				int byte_count;

				while ((byte_count = stream.Read(receivedBytes, 0, receivedBytes.Length)) > 0)
				{
					Packet paket = new Packet();
					paket.WriteBytes(receivedBytes);
					Console.WriteLine(paket.ReadString());
				}
			}
			catch
			{
				// Handle any exceptions here, such as a server disconnect
			}
			finally
			{
				client.Close();
			}
		}
	}
}
