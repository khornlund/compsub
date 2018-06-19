using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Xml;

namespace WatiN
{
    public class RssReader
    {
        private readonly string _url;
        private SyndicationFeed _feed;

        public RssReader(String url)
        {
            Logger.Log("<init>: " + url);
            _url = url;
        }

        public RssReader Initialise()
        {
            Logger.Log($"Reading: {_url}");

            XmlReader reader = XmlReader.Create(_url);
            _feed = SyndicationFeed.Load(reader);
            reader.Close();

            return this;
        }

        public IEnumerable<Competition> GetItems()
        {
            Logger.Log("..");

            return _feed.Items
                .Where(i => i.Categories.Any(c => c.Name.Contains("Email") || c.Name.Contains("Lucky"))).Select(Competition.Parse);
        }
    }

}
