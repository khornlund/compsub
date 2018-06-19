using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatiN
{
    public class Logger
    {
        public static void Log(string m)
        {
            String callingClass = new StackTrace().GetFrame(1).GetFileName();
            String callingMethod = new StackTrace().GetFrame(1).GetMethod().Name;

            Console.WriteLine($"{callingClass}.{callingMethod}: {m}");
        }
    }
}
