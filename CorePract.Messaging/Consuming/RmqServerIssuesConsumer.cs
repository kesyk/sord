using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using CorePract.Messaging.RabbitMQ;
using CorePract.Messaging.Consuming;
using CorePract.Dto.Enum;
using CorePract.Services;

using Newtonsoft.Json;
using CorePract.Dto;

namespace CorePract.Messaging.Consuming
{
    public class RmqServerIssuesConsumer : RmqIssuesConsumer
    {

        public RmqServerIssuesConsumer ( string queue ) : base( queue )
        {
                _callback = Callback;   
        }

        override
        public void Callback(string message)
        {
            try
            {
                if (message != "")
                {
                    Console.WriteLine(message);

                    IssueDto issue = JsonConvert.DeserializeObject<IssueDto>(message);

                    if (issue.status == IssuesStatus.New)
                    {
                        Random rand = new Random();
                        
                        Array issuesStatuses = Enum.GetValues(typeof(IssuesStatus));
                        issue.status = (IssuesStatus)issuesStatuses.GetValue(rand.Next(1,Enum.GetValues(typeof(IssuesStatus)).Length));

                        var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(issue));

                        _channel.BasicPublish(exchange: _config["RabbitMq:exchange"],
                                             routingKey: _config["RabbitMq:routingKeyProcessed"],
                                             basicProperties: null,
                                             body: body);
                    }
                }
            }
            catch (Exception ex)
            {
                //logging
                Console.WriteLine("Заявка не была обработана");
            }
        }


    }
}
