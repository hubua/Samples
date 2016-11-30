using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentCollections
{
    /*
     * http://www.codeguru.com/csharp/article.php/c20229/Working-with-Concurrent-Collections-in-NET-Framework-40.htm
     * https://msdn.microsoft.com/en-us/library/dd997373(v=vs.110).aspx
     * https://referencesource.microsoft.com/#mscorlib/system/collections/queue.cs
     */
    class Program
    {
        static void PlaceOrder(ConcurrentQueue<string> queue, string worker)
        {
            foreach (var i in Enumerable.Range(1, 9))
            {
                Thread.Sleep(10);
                string ordername = $"{worker} - #{i:00} - {DateTime.Now.ToString("mm:ss.fff")}";
                queue.Enqueue(ordername);
            }
        }

        static void ProcessStock(ConcurrentDictionary<string, int> stock, string worker, ref int total)
        {
            var products = new[] { "C", "D", "E", "F" };

            var rnd = new Random(worker.GetHashCode());
            var start = DateTime.Now;
            var length = TimeSpan.FromSeconds(1);
            while (DateTime.Now - start < length)
            {
                Thread.Sleep(rnd.Next(100));

                var product = products[rnd.Next(products.Count())];
                bool buy = rnd.Next(4) == 0;
                if (buy)
                {
                    var quantity = rnd.Next(40) + 1;
                    var newvalue = stock.AddOrUpdate(product, quantity, (key, oldvalue) => oldvalue + quantity);
                    Console.WriteLine($"{worker} + {quantity:00} {product} | now: {newvalue}");
                    Interlocked.Add(ref total, quantity);
                }
                else // sell
                {
                    var quantity = rnd.Next(10) + 1;
                    var newvalue = stock.AddOrUpdate(product, -quantity, (key, oldvalue) => oldvalue - quantity);
                    Console.WriteLine($"{worker} - {quantity:00} {product} | now: {newvalue}");
                    Interlocked.Add(ref total, -quantity);
                }
            
            }

        }

        static void ProcessOrder(string order)
        {
            Console.WriteLine(order + " FROM " + Thread.CurrentThread.ManagedThreadId);
        }

        static void WorkWithOrders()
        {
            var queue = new ConcurrentQueue<string>(); // Queueu<string> will have unpredictable results

            var taskQ1 = Task.Run(() => PlaceOrder(queue, "Anna"));
            var taskQ2 = Task.Run(() => PlaceOrder(queue, "Bill"));
            Task.WaitAll(taskQ1, taskQ2);

            foreach (var item in queue)
            {
                Console.WriteLine(item);
            }

            /*
            Parallel.ForEach(queue, ProcessOrder);

            bool c = true;
            while (c)
            {
                string item;
                c = queue.TryDequeue(out item);
                Console.WriteLine(item);
            }
            */
        }

        static void WorkWithStock()
        {
            var stock = new ConcurrentDictionary<string, int>();
            int total = 0;

            var taskD1 = Task.Run(() => ProcessStock(stock, "Anna", ref total));
            var taskD2 = Task.Run(() => ProcessStock(stock, "Bill", ref total));
            var taskD3 = Task.Run(() => ProcessStock(stock, "John", ref total));
            var taskD4 = Task.Run(() => ProcessStock(stock, "Paul", ref total));
            Task.WaitAll(taskD1, taskD2, taskD3, taskD4);

            foreach (var item in stock)
            {
                Console.WriteLine($"{item.Value} {item.Key} in stock");
            }
            Console.WriteLine($"Total: {total}");
        }


        static void Main(string[] args)
        {
            // WorkWithOrders();
            WorkWithStock();
        }
    }
}
