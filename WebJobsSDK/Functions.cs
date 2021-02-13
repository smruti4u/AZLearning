using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebJobsSDK
{
    public class Functions
    {
        public static void ProcessOrder([QueueTrigger("test")] Order order, ILogger logger)
        {
            logger.LogInformation($"Message Received At {DateTime.Now} for resturant {order.ResturantName}");
        }


        public static void TimerTrigger([TimerTrigger("0 */2 * * * *", RunOnStartup = true)] TimerInfo info, ILogger logger)
        {
            logger.LogInformation($"Message Received At {DateTime.Now}");
        }
    }

    public class Order
    {
        public string OrderNo { get; set; }
        public string ResturantName { get; set; }
        public Adress Adress { get; set; }
    }

    public class Adress
    {
        public string zipCode { get; set; }
        public string city { get; set; }
    }
}
