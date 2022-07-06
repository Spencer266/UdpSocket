using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
namespace Server
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Udp Server";
            // giá trị Any của IPAddress tương ứng với Ip của tất cả các giao diện mạng trên máy
            var localIp = IPAddress.Any;
            // tiến trình server sẽ sử dụng cổng 1308
            var localPort = 1308;
            // biến này sẽ chứa "địa chỉ" của tiến trình server trên mạng
            var localEndPoint = new IPEndPoint(localIp, localPort);
            // yêu cầu hệ điều hành cho phép chiếm dụng cổng 1308
            // server sẽ nghe trên tất cả các mạng mà máy tính này kết nối tới
            // chỉ cần gói tin udp đến cổng 1308, tiến trình server sẽ nhận được
            // một overload khác của hàm tạo Socket
            // InterNetwork là họ địa chỉ dành cho IPv4
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(localEndPoint);
            Console.WriteLine($"Local socket bind to {localEndPoint}. Waiting for request ...");

            var size = 1024;
            var receiveBuffer = new byte[size];
            var text = "NaN";

            Data sendData = new Data(3.4f, 2, true);
            string jsonData = JsonConvert.SerializeObject(sendData);
            byte[] sendBuffer = Encoding.Default.GetBytes(jsonData);

            while (text != "stop")
            {
                // biến này về sau sẽ chứa địa chỉ của tiến trình client nào gửi gói tin tới
                EndPoint remoteEndpoint = new IPEndPoint(IPAddress.Any, 0);
                // khi nhận được gói tin nào sẽ lưu lại địa chỉ của tiến trình client
                var length = socket.ReceiveFrom(receiveBuffer, ref remoteEndpoint);
                text = Encoding.ASCII.GetString(receiveBuffer, 0, length);
                Console.WriteLine($"Received from {remoteEndpoint}: {text}");
                // chuyển chuỗi thành dạng in hoa
                var result = text.ToUpper();
                // gửi kết quả lại cho client
                socket.SendTo(sendBuffer, remoteEndpoint);
                Array.Clear(receiveBuffer, 0, size);
            }

            socket.Close();
        }
    }
}