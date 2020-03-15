using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceBusMessageGenerator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string connectionString = "";

            string message = "Hello KEDA : ";
            int iterationCount = 10;
            int threadCount = 10;
            QueueClient client = new QueueClient(connectionString, "myqueue", ReceiveMode.PeekLock);
            for (int i = 0; i < iterationCount; i++)
            {
                var result = Parallel.For(1, threadCount, (x) =>
                            {
                                try
                                {
                                    byte[] messageBody = System.Text.Encoding.UTF8.GetBytes($"{message} {System.Threading.Thread.CurrentThread.ManagedThreadId}");
                                    client.SendAsync(new Message(messageBody)).Wait();
                                    Console.WriteLine($"{System.Threading.Thread.CurrentThread.ManagedThreadId} : Messages posted successfully to Service Bus");
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine(ex);
                                }
                            });
                Console.WriteLine("Waiting for 2 seconds to continue iteration ...");
                Thread.Sleep(2000);
            }

        }
    }
}
