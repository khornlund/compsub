using System;
using System.Linq;
using System.Threading;

namespace WatiN
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Starting...");

            string ozBargain = "https://www.ozbargain.com.au/competition/all/feed";
            string logFile = @"C:\Users\Karl\Documents\Visual Studio 2017\Projects\WatiN\WatiN\WatiN\data\archive.txt";

            var manager = new Manager(ozBargain, logFile, debug: true);

            manager.Start();
            
            Logger.Log("Returning...");
        }
    }
}
