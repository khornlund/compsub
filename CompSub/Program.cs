namespace CompSub
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.Log("Starting...");

            string ozBargain = "https://www.ozbargain.com.au/competition/all/feed";
            string logFile = @"../../../output/archive.txt";

            // initialising manager in debug mode will cause it to not submit the forms it fills
            var manager = new Manager(ozBargain, logFile, debug: false);

            manager.Start();
            
            Logger.Log("Returning...");
        }
    }
}
