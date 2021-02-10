using System;

namespace ATAG.ConsoleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach(var arg in args)
            {
                Console.WriteLine(arg);
            }
        }
    }
}
