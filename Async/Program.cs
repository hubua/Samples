using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncREST
{

    /*
    http://openweathermap.org/current#name khubua:12345678
    */
    class Program
    {
        //http://sergeyteplyakov.blogspot.com/2015/06/lazy-trick-with-concurrentdictionary.html
        //http://sergeyteplyakov.blogspot.com/2015/06/process-tasks-by-completion.html
        //http://sergeyteplyakov.blogspot.com/2015/07/foreachasync.html

        static void Main(string[] args)
        {
            var tl = new TextWriterTraceListener("TextWriterOutput.txt", "MyListener");
            tl.TraceOutputOptions = TraceOptions.DateTime | TraceOptions.Timestamp | TraceOptions.ProcessId;
            Trace.Listeners.Add(tl);
            Trace.TraceInformation("Starting");
            Trace.Flush();

            /* Less information
            var tl = new TextWriterTraceListener("TextWriterOutput.txt", "MyListener");
            tl.WriteLine("Hello");
            tl.Flush();
            */


            string key = "ae577e294adc3ab5afc704f045a01428"; //2de143494c0b295cca9337e1e96b00e0
            WebClient client = new WebClient();
            client.BaseAddress = key;
        }
    }
}
