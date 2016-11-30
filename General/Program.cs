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
        static void pause()
        {
            Thread.Sleep(200);
        }

        static Task sample1()
        {
            // Syncronously running code
            Console.WriteLine("S1 initializing");
            var t = new Task(() => { pause(); Console.WriteLine("S1 done"); });
            return t;
        }

        static Task sample2()
        {
            // Syncronously running code
            Console.WriteLine("S2 initializing");
            var t = new Task(() => { pause(); Console.WriteLine("S2 done"); });
            return t;
        }

        static async Task<int> sample3()
        {
            Console.WriteLine("S3 started");
            pause();
            int n = await Task.Factory.StartNew(() => { Console.WriteLine("S3 running"); pause(); return 1; });
            Console.WriteLine("S3 complete");
            return n;
        }

        static async Task<int> sample4()
        {
            Console.WriteLine("S4 started");
            pause();
            int n = await Task.Run(() => { Console.WriteLine("S4 running"); pause(); return 1; });
            Console.WriteLine("S4 complete");
            return n;
        }

        static async Task<int> sampleNamed(string name)
        {
            Console.WriteLine($"{name} started");
            await Task.Delay(3000);
            Console.WriteLine($"{name} complete");
            return 1;
        }


        static void Main(string[] args)
        {
            var t1 = sample1();
            var t2 = sample2();
            t1.Start(); // Tasks starting in parrallel
            t2.Start(); // Tasks starting in parrallel
            Console.WriteLine("samples started");
            Task.WaitAll(new[] { t1, t2 });


            var te1 = Task.Factory.StartNew(() => { Console.WriteLine("generating exception 1"); throw new ApplicationException("some exception 1"); });
            var te2 = Task.Factory.StartNew(() => { Console.WriteLine("generating exception 2"); throw new ApplicationException("some exception 2"); });
            var te3 = Task.Factory.StartNew(() => { Console.WriteLine("not generating exception"); });
            try
            {
                Task.WaitAll(new[] { te1, te2, te3 });
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"EXCEPTION: {ex.Message}");
                foreach (var iex in ex.InnerExceptions)
                {
                    Console.WriteLine($"INNER EXCEPTION: {iex.Message}");
                }
            }

            Task.WaitAll(sample3(), sample4());

           
            Task.WaitAll(sampleNamed("SN1"), sampleNamed("SN2"));
        }
    }
}
