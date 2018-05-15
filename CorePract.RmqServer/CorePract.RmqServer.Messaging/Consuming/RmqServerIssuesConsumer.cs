using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using CorePract.RmqServer.Messaging.RabbitMQ;
using CorePract.RmqServer.Messaging.Consuming;
using CorePract.Dto.Enum;
using CorePract.Services;

using Newtonsoft.Json;
using CorePract.Dto;

namespace CorePract.RmqServer.Messaging.Consuming
{
    public class RmqServerIssuesConsumer : RmqIssuesConsumer
    {
        private string _exchange;
        private string _routingKey;  

        public RmqServerIssuesConsumer ( string user, string vHost, string password, string host, string queue, string exchange, string routingKey ) : base( user, vHost, password, host, queue)
        {
            _exchange = exchange;
            _routingKey = routingKey;   
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
                        issue.status = (IssuesStatus)issuesStatuses.GetValue(rand.Next(1, Enum.GetValues(typeof(IssuesStatus)).Length));

                        __rabbitCon.Send(__channel, _exchange, _routingKey, JsonConvert.SerializeObject(issue));
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
