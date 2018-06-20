using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;

namespace CompSub
{
    /// <summary>
    /// This class encapsulates required information about a competition
    /// </summary>
    public class Competition
    {
        public string Title { get; }
        public string Url { get; }
        public IEnumerable<string> Categories { get; }
        public DateTimeOffset Published { get; }

        /// <summary>
        /// url is contained in this element extension
        /// </summary>
        private const string MetaElement = "meta";

        /// <summary>
        /// name of attribute containing competition link
        /// </summary>
        private const string UrlAttribute = "url";

        private Competition(string title, string url, IEnumerable<string> categories, DateTimeOffset published)
        {
            Title = title;
            Url = url;
            Categories = categories;
            Published = published;
        }

        private Competition()
        {
        }

        public override bool Equals(object obj)
        {
            var candidate = obj as Competition;
            return obj != null && Url.Equals(candidate?.Url);
        }

        /// <summary>
        /// Parses an item of the OzBargain RSS feed and creates a <see cref="Competition"/> object
        /// </summary>
        /// <param name="item"></param>
        /// <returns>
        /// An instance of <see cref="Competition"/>
        /// </returns>
        public static Competition Parse(SyndicationItem item)
        {
            string title = item.Title.Text;
            IEnumerable<string> categories = item.Categories.Select(c => c.Name);
            DateTimeOffset published = item.PublishDate;

            string url = item.ElementExtensions
                .Where(e => e.OuterName.Equals(MetaElement))
                .Select(e => e.GetReader().GetAttribute(UrlAttribute)).FirstOrDefault();

            return new Competition(title, url, categories, published);
        }

        public override string ToString()
        {
            return $"URL={Url}, " +
                   $"Title={Title}, " +
                   $"Published={Published}, " +
                   $"Categories={string.Join(",", Categories)}";
        }
    }
}
