using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Hosting;


public class ProxyListener : BackgroundService
{

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
            TcpListener listener = new TcpListener(IPAddress.Loopback, 8080);
            listener.Start();
            Console.WriteLine(IPAddress.Loopback);
    }


}






