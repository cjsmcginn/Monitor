using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitor.ConsoleRunner
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Out.Write("Starting Console Runner");
            EventPoster.PostWebsiteErrors();
            EventPoster.PostPayments();
            Console.In.Read();
        }
    }
}
