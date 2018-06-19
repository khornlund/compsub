using System;
using System.Collections.Generic;
using System.Text;
using WatiN.Core;

namespace CompSub
{
    /// <summary>
    /// This class contains information about competitions that have been entered
    /// </summary>
    public class FormFillerReport : IReport
    {
        public Competition Competition { get; }
        public IEnumerable<Form> FormsSubmitted { get; }
        public IEnumerable<Tuple<Element, string>> ElementsModified { get; }
        public string Html { get; }

        public FormFillerReport(
            Competition competition, 
            IEnumerable<Form> formsSubmittedList, 
            IEnumerable<Tuple<Element, string>> elementsModified,
            string html)
        {
            Competition = competition;
            FormsSubmitted = formsSubmittedList;
            ElementsModified = elementsModified;
            Html = html;
        }

        /// <summary>
        /// Paramaterless ctor required for XML serialization
        /// </summary>
        private FormFillerReport()
        {
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Competition + ". ");
            sb.Append("Forms: [" + string.Join(",", FormsSubmitted) + "]. ");
            foreach (var tuple in ElementsModified)
            {
                sb.Append($"Modified: {tuple.Item1} <- {tuple.Item2}. ");
            }

            return sb.ToString();
        }

        public class Builder
        {
            private readonly Competition _competition;
            private IList<Tuple<Element, string>> _elementsModified;
            private IList<Form> _formsList;
            private string _html;

            private Builder(Competition competition)
            {
                _competition = competition;
                _elementsModified = new List<Tuple<Element, string>>();
                _formsList = new List<Form>();
            }

            public static Builder Competition(Competition competition)
            {
                return new Builder(competition);
            }

            public Builder Html(string html)
            {
                _html = html;
                return this;
            }

            public Builder ElementModified(Element element, string value)
            {
                _elementsModified.Add(new Tuple<Element, string>(element, value));
                return this;
            }

            public Builder FormSubmitted(Form form)
            {
                _formsList.Add(form);
                return this;
            }

            public FormFillerReport Build()
            {
                return new FormFillerReport(
                    _competition, 
                    _formsList, 
                    _elementsModified, 
                    _html);
            }

        }
    }


}
