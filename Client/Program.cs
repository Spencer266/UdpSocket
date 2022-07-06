using System;
using System.Net; // để sử dụng lớp IPAddress, IPEndPoint
using System.Net.Sockets; // để sử dụng lớp Socket
using System.Text; // để sử dụng lớp Encoding
namespace Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Udp Client";
            // yêu cầu người dùng nhập ip của server
            Console.Write("Server IP address: ");
            var serverIpStr = Console.ReadLine();
            // chuyển đổi chuỗi ký tự thành object thuộc kiểu IPAddress
            var serverIp = IPAddress.Parse(serverIpStr);
            // yêu cầu người dùng nhập cổng của server
            Console.Write("Server port: ");
            var serverPortStr = Console.ReadLine();
            // chuyển chuỗi ký tự thành biến kiểu int
            var serverPort = int.Parse(serverPortStr);
            // đây là "địa chỉ" của tiến trình server trên mạng
            // mỗi endpoint chứa ip của host và port của tiến trình
            var serverEndpoint = new IPEndPoint(serverIp, serverPort);
            var size = 1024; // kích thước của bộ đệm
            var receiveBuffer = new byte[size]; // mảng byte làm bộ đệm            
            var socket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            while (true)
            {
                // yêu cầu người dùng nhập một chuỗi
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("# Text >>> ");
                Console.ResetColor();
                var text = Console.ReadLine();
                // khởi tạo object của lớp socket để sử dụng dịch vụ Udp
                // lưu ý SocketType của Udp là Dgram (datagram) 
                
                // biến đổi chuỗi thành mảng byte
                var sendBuffer = Encoding.ASCII.GetBytes(text);
                // gửi mảng byte trên đến tiến trình server
                socket.SendTo(sendBuffer, serverEndpoint);
                // endpoint này chỉ dùng khi nhận dữ liệu
                EndPoint dummyEndpoint = new IPEndPoint(IPAddress.Any, 0);
                // nhận mảng byte từ dịch vụ Udp và lưu vào bộ đệm
                // biến dummyEndpoint có nhiệm vụ lưu lại địa chỉ của tiến trình nguồn
                // tuy nhiên, ở đây chúng ta đã biết tiến trình nguồn là Server
                // do đó dummyEndpoint không có giá trị sử dụng 
                var length = socket.ReceiveFrom(receiveBuffer, ref dummyEndpoint);
                // chuyển đổi mảng byte về chuỗi
                var result = Encoding.ASCII.GetString(receiveBuffer, 0, length);
                // xóa bộ đệm (để lần sau sử dụng cho yên tâm)
                Array.Clear(receiveBuffer, 0, size);
                // đóng socket và giải phóng tài nguyên
                // in kết quả ra màn hình
                Console.WriteLine($">>> {result}");
            }
        }
    }
}