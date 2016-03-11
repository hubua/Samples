using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplicationUnity
{
    public interface IDAL
    {
        string GetName();
    }

    public class DAL : IDAL
    {
        public string GetName()
        {
            return "John";
        }
    }

    public class Writer
    {
        private IDAL dal;

        public Writer(IDAL dal)
        {
            this.dal = dal;
        }

        public void WriteName()
        {
            Console.WriteLine(dal.GetName());
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var c = new UnityContainer();
            c.LoadConfiguration();
            //c.RegisterType(typeof(IDAL), typeof(DAL));
            var dal = c.Resolve<IDAL>();

            new Writer(dal).WriteName();

        }
    }
}
