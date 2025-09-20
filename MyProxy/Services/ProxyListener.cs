using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;
using MyProxy.Models;

public class ProxyListener : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        TcpListener listener = new TcpListener(IPAddress.Loopback, 8080);
        listener.Start();
        Console.WriteLine("Proxy running in " + IPAddress.Loopback + ":8080");

        while (!stoppingToken.IsCancellationRequested)
        {
            TcpClient client = await listener.AcceptTcpClientAsync(stoppingToken);
            _ = Task.Run(() => HandleClient(client));
        }
    }


    private static async Task HandleClient(TcpClient client)
    {
        using (var clientStream = client.GetStream())
        {
            try
            {
                StreamReader reader = new StreamReader(clientStream);
                string requestLine = await reader.ReadLineAsync();

                if (string.IsNullOrEmpty(requestLine)) return;

                string[] parts = requestLine.Split(' ');
                string method = parts[0];
                string target = parts[1];
                string host = target.Split(':')[0];

                bool allow = false;

                ProxyWhiteListHttps proxy = new ProxyWhiteListHttps();

                foreach (var site in proxy.allowedSites)
                {
                    if (host.Contains(site))
                    {
                        allow = true;
                        break;
                    }
                }

                if (!allow)
                {
                    byte[] blockedMsg = Encoding.UTF8.GetBytes("HTTP/1.1 403 Forbidden\r\n\r\nConteúdo Bloqueado");
                    await clientStream.WriteAsync(blockedMsg, 0, blockedMsg.Length);
                    Console.WriteLine($"Bloqueado: {host}");
                    return;
                }

                Console.WriteLine($"Permitido: {host}");

                if (method.ToUpper() == "CONNECT")
                {
                    byte[] connectOk = Encoding.UTF8.GetBytes("HTTP/1.1 200 Connection Established\r\n\r\n");
                    await clientStream.WriteAsync(connectOk, 0, connectOk.Length);

                    using (TcpClient server = new TcpClient(host, 443))
                    using (NetworkStream serverStream = server.GetStream())
                    {
                        await Task.WhenAny(
                           clientStream.CopyToAsync(serverStream),
                           serverStream.CopyToAsync(clientStream)
                        );
                    }
                }

                else
                {
                    byte[] msg = Encoding.UTF8.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: text/plain\r\n\r\nHTTP Proxy running.");
                    await clientStream.WriteAsync(msg, 0, msg.Length);
                }
            }
            
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
        }
    }
}
