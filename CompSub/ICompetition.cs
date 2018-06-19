using System;
using System.Collections.Generic;

namespace WatiN
{
    public interface ICompetition
    {
        string Title { get; }
        string Url { get; }
        IEnumerable<string> Categories { get; }
        DateTimeOffset Published { get; }
    }
}
