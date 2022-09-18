using System.Net;
using System.Net.Sockets;
using System.Text;

Console.Write("Введите ip-адрес: ");
var ip = Console.ReadLine();

Console.Write("Введите порт отправки сообщений: ");
var sendPort = Convert.ToInt32(Console.ReadLine());

Console.Write("Введите порт приёмки сообщений: ");
var receivePort = Convert.ToInt32(Console.ReadLine());

Task.Run(() => ReceiveMessage(receivePort));

SendMessage(ip, sendPort);

void SendMessage(string host, int port)
{
    using var udp = new UdpClient();
    while (true)
    {
        Console.Write("Введите сообщение: ");
        var message = Console.ReadLine();
        var data = Encoding.UTF8.GetBytes(message);
        udp.Send(data, data.Length, host, port);
    }
}

void ReceiveMessage(int port)
{
    using var udp = new UdpClient(port);
    var ip = new IPEndPoint(IPAddress.Any, 0);
    while (true)
    {
        var data = udp.Receive(ref ip);
        var message = Encoding.UTF8.GetString(data);
        Console.WriteLine($"Собеседник: {message}");
    }
}