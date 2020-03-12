using RabbitMQ.Client;
using System;

namespace RabbitMqTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //.NET/C# Client API Guide
            //https://www.rabbitmq.com/dotnet-api-guide.html


            ConnectionFactory factory = new ConnectionFactory();
          
            factory.UserName = "";
            factory.Password = "";
            factory.VirtualHost = "";
            factory.HostName = "";
            factory.Port = 5672;

            IConnection conn = factory.CreateConnection();

            //open a channel
            IModel channel = conn.CreateModel();



            channel.Close();
            conn.Close();

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
