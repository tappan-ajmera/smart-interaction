using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmlgrammar;
using Console = System.Console;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreNLP cr = new CoreNLP();
            cr.Init();
            cr.POSTagger("Hello world");
            Console.WriteLine("Done");
        }
    }
}
