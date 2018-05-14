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
        IssuesStorage _issuesStorage;

        public RmqRestServerIssuesConsumer( IssuesStorage issuesStorage, string queue ) : base( queue )
        {
            _issuesStorage = issuesStorage;
            _callback = Callback;
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
