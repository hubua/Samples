using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace General
{
         
    class Program
    {

        static void Log(string message)
        {
            System.Diagnostics.Debug.WriteLine(message);
            Console.WriteLine(message);
        }

        static void pause()
        {
            Thread.Sleep(200);
        }

        static Task sampleNonAsync(int id)
        {
            // Syncronously running code
            Log($"NonAsyncSample{id} initializing");
            var t = new Task(() => { pause(); Log($"NonAsyncSample{id} done"); });
            return t;
        }

        static async Task<int> sampleAsync(int id)
        {
            Log($"AsyncSample{id} started");
            await Task.Delay(id * 100);
            Log($"AsyncSample{id} complete");
            return id;
        }

        static async Task<int> sampleTaskFactoryStartNew()
        {
            Console.WriteLine("TaskFactoryStartNewSample started");
            pause();
            int n = await Task.Factory.StartNew(() => { Console.WriteLine("TaskFactoryStartNewSample running"); pause(); return 1; });
            Console.WriteLine("TaskFactoryStartNewSample complete");
            return n;
        }

        static async Task<int> sampleTaskRun()
        {
            Console.WriteLine("TaskRunSample started");
            pause();
            int n = await Task.Run(() => { Console.WriteLine("TaskRunSample running"); pause(); return 1; });
            Console.WriteLine("TaskRunSample complete");
            return n;
        }

        static async Task<int> sampleTaskRunWithException()
        {
            Console.WriteLine("TaskRunSample started");
            pause();
            try
            {
                int n = await Task.Run(() => { Console.WriteLine("TaskRunSample running"); throw new ApplicationException("some exception"); return 1; });

            }
            catch (Exception ex)
            {
                throw;
            }
            Console.WriteLine("TaskRunSample complete");
            return 0;
        }

        static void Main(string[] args)
        {
            // NonAsync tasks execution

            var t1 = sampleNonAsync(1);
            var t2 = sampleNonAsync(2);
            t1.Start(); // Execution starting in parrallel
            t2.Start();
            Log("NonAsync task samples started");
            Task.WaitAll(new[] { t1, t2 });

            Console.WriteLine();

            // Async tasks execution

            var a = sampleAsync(20); // Execution starts as soon as async method is invoked
            var b = sampleAsync(10);
            var ar = a.Result;
            var br = b.Result;
            Log($"Async Result: {ar}, {br}");

            var sdr = Task.WhenAll(sampleAsync(40), sampleAsync(30)).Result;
            foreach (var item in sdr)
            {
                Log($"Async WhenAll: {item}");
            }

            Console.WriteLine();

            // Advanced Async tasks execution

            Task.WaitAll(sampleTaskFactoryStartNew(), sampleTaskRun());

            Console.WriteLine();

            // Unobsereved exceptions

            var te1 = Task.Factory.StartNew(() => { Log("generating exception 1"); throw new ApplicationException("some exception 1"); });
            var te2 = Task.Factory.StartNew(() => { Log("generating exception 2"); throw new ApplicationException("some exception 2"); });
            var te3 = Task.Factory.StartNew(() => { Log("not generating exception"); });
            try
            {
                Task.WaitAll(new[] { te1, te2, te3 });
            }
            catch (AggregateException ex)
            {
                Log($"EXCEPTION: {ex.Message}");
                foreach (var iex in ex.InnerExceptions)
                {
                    Log($"INNER EXCEPTION: {iex.Message}");
                }
            }

            // async exception

            sampleTaskRunWithException().Wait();
        }
    }
}
