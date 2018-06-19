using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using WatiN.Core;

namespace CompSub
{
    /// <summary>
    /// This class is repsonsible for filling out a web form
    /// with relevant details and submitting the form
    /// </summary>
    public class FormFiller
    {
        private readonly FormFillerReport.Builder _reportBuilder;
        private readonly string _url;
        private readonly List<DictionaryEntry> _dictionary = DictionaryEntry.GetAll();

        public FormFiller(Competition competition)
        {
            Logger.Log($"{competition}");

            _reportBuilder = FormFillerReport.Builder.Competition(competition);
            _url = competition.Url;
        }
        
        [STAThread]
        public FormFillerReport Start(bool debug)
        {
            Logger.Log("..");

            IE ie = new IE();
            ie.GoTo(_url);

            _reportBuilder.Html(ie.Html);

            foreach (var form in ie.Forms)
            {
                Logger.Log($"\r\nFilling form name={form.Name}\r\n");

                foreach (var element in form.Elements)
                {
                    // default element handling
                    HandleElement(element);

                    // specific handling
                    HandleElementContainer(element as ElementContainer<Element>);
                    HandleTextField(element as TextField);
                }

                if (!debug) // allow bypassing actually submitting the forms
                {
                    _reportBuilder.FormSubmitted(form);
                    form.Submit();
                }
            }

            var report = _reportBuilder.Build();

            Logger.Log($"Produced: {report}");
            return report;
        }

        private void HandleElement(Element element)
        {
            foreach (var entry in _dictionary)
            {
                if (MatchElement(element, entry.Regex))
                {
                    Logger.Log($"HandleElement: Writing {entry.Value} to field {element.Name}");
                    _reportBuilder.ElementModified(element, entry.Value);
                    element.SetAttributeValue("value", entry.Value);
                }
            }
        }

        private void HandleElementContainer(ElementContainer<Element> container, bool force = false)
        {
            if (container == null)
                return;

            Logger.Log($"HandleElementContainer: name={container.Name}, id={container.Id}, type={container.GetType()}");

            if (!force)
            {
                foreach (var entry in _dictionary)
                {
                    if (MatchElement(container, entry.Regex))
                    {
                        Logger.Log($"Found match with {entry.Regex}!");
                        force = true;
                        _reportBuilder.ElementModified(container, entry.Value);
                        container.SetAttributeValue("value", entry.Value);
                    }
                }
            }

            foreach (var element in container.Elements)
            {
                Logger.Log($"HandleElementContainer: Handling child: name={element.Name}, id={element.Id}, type={element.GetType()}");
                HandleTextField(element as TextField, force);
                HandleElementContainer(element as ElementContainer<Element>, force);
            }
        }

        private void HandleTextField(TextField field, bool force = false)
        {
            if (field == null)
                return;

            Logger.Log($"HandleTextField: name={field.Name}, id={field.Id}, type={field.GetType()}");

            foreach (var entry in _dictionary)
            {
                if (!MatchElement(field, entry.Regex) && !force) continue;

                Logger.Log($"Writing {entry.Value} to field {field.Name}");
                _reportBuilder.ElementModified(field, entry.Value);
                field.ClickNoWait();
                field.Value = entry.Value;
            }
        }

        private bool MatchElement(Element element, Regex regex)
        {
            if (Match(element.Description, regex))
                return true;
            if (Match(element.Id, regex))
                return true;
            if (Match(element.InnerHtml, regex))
                return true;
            if (Match(element.Name, regex))
                return true;
            if (Match(element.OuterHtml, regex))
                return true;
            if (Match(element.OuterText, regex))
                return true;
            if (Match(element.TagName, regex))
                return true;
            if (Match(element.TextAfter, regex))
                return true;
            if (Match(element.TextBefore, regex))
                return true;
            if (Match(element.Title, regex))
                return true;
            if (Match(element.GetAttributeValue("placeholder"), regex))
                return true;

            return false;
        }

        private bool Match(String field, Regex regex)
        {
            if (field == null)
                return false;

            return regex.IsMatch(field);
        }
    }
}
