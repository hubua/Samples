using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternSingleton
{
    public sealed class Singleton
    {
        public static readonly Singleton instance = new Singleton();

        static Singleton()
        {
            // Explicit static constructor to tell compiler not to mark class as beforefieldinit
        }

        Singleton()
        {

        }
    }

    public sealed class SingletonLazy
    {
        SingletonLazy()
        {

        }

        public static SingletonLazy Instance
        {
            get { return Nested.instance; }
        }

        class Nested
        {
            internal static readonly SingletonLazy instance = new SingletonLazy();

            static Nested()
            {
                // Explicit static constructor to tell compiler not to mark class as beforefieldinit
            }
        }

    }
        class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
