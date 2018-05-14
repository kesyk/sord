using System;
using System.Collections.Generic;
using System.Text;

namespace CorePract.Messaging.RabbitMQ
{
    public static class RabbitMqConfig
    {
        public static string user = "lxqhewvj";
        public static string password = "YO0dCnU73PUn99lsJ2pME1-nGMFouc41";
        public static string host = "spider.rmq.cloudamqp.com";
        public static string vHost = "lxqhewvj";

        public static string exchange = "issues_exchange";

        public static string queueProcessed = "processed_issues_queue";
        public static string routingKeyProcessed = "processed.issues";

        public static string queueNew = "new_issues_queue";
        public static string routingKeyNew = "new.issues";
    }
}
