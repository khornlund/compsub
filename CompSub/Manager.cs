using System;
using System.Linq;
using System.Threading;

namespace CompSub
{
    public class Manager
    {
        private readonly string _rssFeed;
        private readonly bool _debug;

        private readonly Archiver _archiver;

        private int sleepTimeSeconds = 10; // 10s

        public Manager(string rssFeed, string logFile, bool debug)
        {
            _rssFeed = rssFeed;
            _debug = debug;

            _archiver = new Archiver(logFile);
        }

        [STAThread]
        public void Start()
        {
            Logger.Log("..");

            //TODO: use a timer to loop this

            Thread t = new Thread(Work);
            t.SetApartmentState(ApartmentState.STA);
            t.Start();

            t.Join();
        }

        [STAThread]
        public void Work()
        {
            Logger.Log("..");

            var competitions = new RssReader(_rssFeed).Initialise().GetItems();

            Logger.Log($"Found: {competitions.Count()} competitions.");

            foreach (var competition in _archiver.NewCompetitions(competitions))
            {
                try
                {
                    Logger.Log($"Entering: {competition}");
                    FormFillerReport report = new FormFiller(competition).Start(debug: _debug);
                    LogReport(report);
                }
                catch (Exception e)
                {
                    Logger.Log("Caught exception: " + e.Message);
                }
            }
        }

        public void LogReport(FormFillerReport report)
        {
            _archiver.Archive(report);
            Logger.Log($"Archived: {report}");
        }
    }
}
