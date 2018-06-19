using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatiN.Core;

namespace WatiN
{
    public interface IReport
    {
        Competition Competition { get; }
        IEnumerable<Form> FormsSubmitted { get; }
        IEnumerable<Tuple<Element, string>> ElementsModified { get; }
        string Html { get; }
    }
}
