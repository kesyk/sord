using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;

namespace CorePract.Messaging.RabbitMQ
{
    public class RabbitMqConnector
    {
        public IConnection conn;

        public RabbitMqConnector(){}

        public void Connect( string userName = "guest", string virtualHost="guest", string password = "guest", string hostName = "localhost" )
        {
            ConnectionFactory factory = new ConnectionFactory();
            factory.UserName = userName;
            factory.Password = password;
            factory.HostName = hostName;
            factory.VirtualHost = virtualHost;
            factory.Port = AmqpTcpEndpoint.UseDefaultPort;
            factory.Protocol = Protocols.DefaultProtocol;



            conn = factory.CreateConnection();
        }
        
        public void Send( IModel channel,string exchangeName, string queueName, string routingKeyName,string message )
        {
            var body = Encoding.UTF8.GetBytes(message);
           
            channel.BasicPublish(exchangeName,
                                 routingKey: routingKeyName,
                                 basicProperties: null,
                                 body: body);

        }

        public string Receive( IModel channel, string queueName, Action<string> callback )
        {
            string message="";
            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                message = Encoding.UTF8.GetString(body);

                callback(message);
            };

            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);

            return message;
        }

        public void BindExchangeAndQueue(IModel channel, string queue,string exchange, string routingKey)
        {
            channel.ExchangeDeclare(exchange, ExchangeType.Direct);
            channel.QueueDeclare(queue, false, false, false, null);
            channel.QueueBind(queue,exchange,routingKey, null);

        }

        public void Disconnect()
        {
            conn.Close();
        }
    }
}
