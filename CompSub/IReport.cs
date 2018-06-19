using System;
using System.Collections.Generic;
using WatiN.Core;

namespace CompSub
{
    public interface IReport
    {
        Competition Competition { get; }
        IEnumerable<Form> FormsSubmitted { get; }
        IEnumerable<Tuple<Element, string>> ElementsModified { get; }
        string Html { get; }
    }
}
