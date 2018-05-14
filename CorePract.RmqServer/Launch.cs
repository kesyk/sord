using System;
using System.Text;
using System.Collections.Generic;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using CorePract.RmqServer.Messaging.Consuming;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace CorePract.RmqServer
{
    class Launch
    {
        static void Main(string[] args)
        {
            try
            {
                Dictionary<string, string> rmqConfig;
                Dictionary<object, object> config;

                StreamReader r = new StreamReader("appsettings.json");
                var json = r.ReadToEnd();
                r.Close();

                config = JsonConvert.DeserializeObject<Dictionary<object, object>>(json);
                rmqConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(config["RabbitMq"].ToString());

                RmqServerIssuesConsumer consumer = new RmqServerIssuesConsumer(
                    rmqConfig["user"],
                    rmqConfig["vHost"],
                    rmqConfig["password"],
                    rmqConfig["host"],
                    rmqConfig["queueNew"],
                    rmqConfig["exchange"],
                    rmqConfig["routingKeyProcessed"]);

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
