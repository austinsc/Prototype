using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prototype.Core;

namespace Prototype.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Engine.RetrieveMatchDetails().Result;
            Console.ReadKey();
        }
    }
}
