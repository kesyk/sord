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
    public class RmqRestServerIssuesConsumer : RmqIssuesConsumer
    {
        private IssuesStorage _issuesStorage;

        public RmqRestServerIssuesConsumer( IssuesStorage issuesStorage, string user, string vHost, string password, string host, string queue, string exchange, string routingKey ) : base( user, vHost, password, host, queue )
        {
            __callback = Callback;
            _issuesStorage = issuesStorage;
        }

        override
        public void Callback( string message )
        {
            try
            {
                if (message != "")
                {
                    IssueDto processedIssue = JsonConvert.DeserializeObject<IssueDto>(message);

                    _issuesStorage.UpdateStatus(
                        processedIssue.IssueId,
                        processedIssue.status
                        );
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
