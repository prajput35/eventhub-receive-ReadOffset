using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Consumer;
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eventhub_receive
{
    class Program
    {
        private static string connstring = "Endpoint=sb://eventhubns204.servicebus.windows.net/;SharedAccessKeyName=hubpolicy;SharedAccessKey=VQqTd4lq05C0BYrZdCdoPywLI2JbSwuHBzumF3QHBmk=;EntityPath=apphub";
        private static string hubname = "apphub";

        static void Main(string[] args)
        {
            GetEvents().Wait();
        }

        static private async Task GetEvents()
        {
            EventHubConsumerClient client = new EventHubConsumerClient("$Default", connstring, hubname);

            string _partition = (await client.GetPartitionIdsAsync()).First();

            var cancellation = new CancellationToken();

            EventPosition _position = EventPosition.FromSequenceNumber(5);
            Console.WriteLine("Getting events from a certain position from a particular partition");
            await foreach (PartitionEvent _recent_event in client.ReadEventsFromPartitionAsync(_partition, _position, cancellation))
            {                
                EventData event_data = _recent_event.Data;               
                
                Console.WriteLine(Encoding.UTF8.GetString(event_data.Body.ToArray()));
                Console.WriteLine($"Sequence Number : {event_data.SequenceNumber}");
                

            }

        }
    }
}
