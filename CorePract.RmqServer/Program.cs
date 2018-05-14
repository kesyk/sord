using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CorePract.Dto;
using CorePract.Messaging.RabbitMQ;
using CorePract.Messaging.Consuming;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
namespace CorePract.RmqServer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                RmqServerIssuesConsumer consumer = new RmqServerIssuesConsumer(_config["RabbitMq:queueNew"]);
                consumer.Consume();
           
            }
            catch (Exception ex)
            {
                //logging 
                Console.WriteLine(ex.Message);
            }
            

        }
       
    }
}
