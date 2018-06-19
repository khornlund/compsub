using System;
using System.Diagnostics;
using System.IO;

namespace CompSub
{
    public class Logger
    {
        private static string filename = @"../../../output/log.txt";

        public static void Log(string m)
        {
            String callingClass = new StackTrace().GetFrame(1).GetFileName();
            String callingMethod = new StackTrace().GetFrame(1).GetMethod().Name;

            string s = $"{callingClass}.{callingMethod}: {m}";
            Console.WriteLine(s);
            save(s);
        }

        private static void save(string s)
        {
            if (!File.Exists(filename))
            {
                File.Create(filename);
            }

            using (StreamWriter writer = File.AppendText(filename))
            {
                writer.WriteLine(s);
            }
        }
    }
}
