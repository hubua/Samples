# Visitor pattern example

```cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatternVisitor
{
    class BaseClass
    {
        public virtual void AcceptVisitor(IClassVisitor obj)
        {
            obj.Visit(this);
        }
    }

    class DerivedClassA : BaseClass
    {
        public override void AcceptVisitor(IClassVisitor obj)
        {
            obj.Visit(this);
        }
    }

    class DerivedClassB : BaseClass
    {
        public override void AcceptVisitor(IClassVisitor obj)
        {
            obj.Visit(this);
        }
    }

    interface IClassVisitor
    {
        void Visit(BaseClass obj);
        void Visit(DerivedClassA obj);
        void Visit(DerivedClassB obj);
    }

    class Resolver : IClassVisitor
    {
        public void Visit(BaseClass obj)
        {
            Console.WriteLine("In BaseClass");
        }

        public void Visit(DerivedClassA obj)
        {
            Console.WriteLine("In DerivedClassA");
        }

        public void Visit(DerivedClassB obj)
        {
            Console.WriteLine("In DerivedClassB");
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            var list = new List<BaseClass>()
            {
                new BaseClass(),
                new DerivedClassA(),
                new DerivedClassB()
            };

            foreach (var item in list)
            {
                var r = new Resolver();
                //r.Visit(item); does not work
                item.AcceptVisitor(r);
            }
            
        }
    }
}
```
