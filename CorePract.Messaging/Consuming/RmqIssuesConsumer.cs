using System;
using System.Collections.Generic;
using System.Text;
using RabbitMQ.Client;
using CorePract.Messaging.RabbitMQ;
using CorePract.Messaging.Consuming;
using CorePract.Dto.Enum;
using CorePract.Services;

namespace CorePract.Messaging.Consuming
{
    abstract public class RmqIssuesConsumer
    {
        protected Action<string> __callback;
        protected string __queue;
        protected IModel __channel;
        protected RabbitMqConnector __rabbitCon;

        protected string __user;
        protected string __vHost;
        protected string __password;
        protected string __host;

        public abstract void Callback(string message);

        public RmqIssuesConsumer( string user, string vHost, string password, string host, string queue )
        {
            __user = user;
            __vHost = vHost;
            __password = password;
            __host = host;

            __queue = queue;
            __rabbitCon = new RabbitMqConnector();
        }

        public void Consume()
        {
            try
            {
                __rabbitCon.Connect(__user, __vHost, __password, __host);
                __channel = __rabbitCon.conn.CreateModel();

                __rabbitCon.Receive(__channel, __queue, Callback);
            }
            catch (Exception ex)
            {
                //logging 
                Stop();
            }
        }

        public void Stop()
        {
            try
            {
                __rabbitCon.Disconnect();
            }
            catch (Exception ex)
            {
                //logging
            }
        }


    }
}
