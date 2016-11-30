using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Deadlocks
{
    /*
     * http://blog.drorhelper.com/2015/12/did-you-know-visual-studio-can-show.html
     * http://www.interact-sw.co.uk/iangblog/2004/04/26/yetmoretimedlocking
     */
    class Program
    {
        static void Main(string[] args)
        {
            var tasks = new Task[6];
            var autoresetevent = new AutoResetEvent(false);
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = Task.Factory.StartNew(state =>
                {
                    var index = (int)state;
                    if (index < 3)
                    {
                        Console.WriteLine($"T {index} waiting for T {(index + 1) % 3}");
                        tasks[(index + 1) % 3].Wait();
                    }
                    else if (index == 3)
                    {
                        Console.WriteLine($"Task {index} in loop");
                        Task.Factory.StartNew(() => 
                        {
                            while (true) { };
                        }).Wait();
                    }
                    else
                    {
                        Console.WriteLine($"Task {index} waitahnde waitone");
                        autoresetevent.WaitOne();
                    }
                }, i);
            }

            Task.WaitAll(tasks);
            autoresetevent.Set();
        }
    }
}
