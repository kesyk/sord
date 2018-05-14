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
        protected Action<string> _callback;
        protected string _queue;
        protected RabbitMqConnector _rabbitCon;
        protected IModel _channel;

        public abstract void Callback( string message );

        public RmqIssuesConsumer( string queue )
        {
            _callback = Callback;
            _queue = queue;
            _rabbitCon = new RabbitMqConnector();
        }


        public void Consume()
        {
            try
            {
                _rabbitCon.Connect(_config["RabbitMq:user"], _config["RabbitMq:vHost"], _config["RabbitMq:password"], _config["RabbitMq:host"]);
                _channel = _rabbitCon.conn.CreateModel();

                _rabbitCon.Receive(_channel, _queue, _callback);

                _rabbitCon.Disconnect();
            }
            catch (Exception ex)
            {
                //logging 
                Console.WriteLine(ex.Message);
            }
        }

        
    }
}
