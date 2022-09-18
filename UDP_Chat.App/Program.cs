using System.Net;
using System.Net.Sockets;
using System.Text;

const int PORT = 8001;
var ip = IPAddress.Parse("225.5.5.5");

Task.Run(() => ReceiveMessage(ip, PORT));

SendMessage(ip, PORT);

void SendMessage(IPAddress ipAddress, int port)
{
    using var udp = new UdpClient();
    var endPoint = new IPEndPoint(ipAddress, port);
    while (true)
    {
        Console.Write("Введите сообщение: ");
        var message = Console.ReadLine();
        var data = Encoding.UTF8.GetBytes(message);
        udp.Send(data, data.Length, endPoint);
    }
}

void ReceiveMessage(IPAddress endPoint, int port)
{
    using var udp = new UdpClient(port);
    var point = new IPEndPoint(IPAddress.Any, 0);
    udp.JoinMulticastGroup(endPoint);
    var localIp = LocalIPAddress();
    while (true)
    {
        var data = udp.Receive(ref point);
        if (point.Address.Equals(localIp))
        {
            continue;
        }
        var message = Encoding.UTF8.GetString(data);
        Console.WriteLine($"Собеседник: {message}");
    }
}

IPAddress LocalIPAddress()
{
    var  host = Dns.GetHostEntry(Dns.GetHostName());
    foreach (var ipAddress in host.AddressList)
    {
        if (ipAddress.AddressFamily != AddressFamily.InterNetwork) continue;
        return ipAddress;
    }

    return IPAddress.Any;
}