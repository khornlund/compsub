using System;
using System.Diagnostics;

namespace CompSub
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
