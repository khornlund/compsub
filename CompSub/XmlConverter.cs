using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CompSub
{
    public static class XmlConverter
    {
        public static XmlDocument ToXml(this IReport report)
        {
            var doc = new XmlDocument();

            // Competition element 

            XmlElement competitonElement = doc.CreateElement("competition");

            competitonElement.SetAttribute("title", report.Competition.Title);
            competitonElement.SetAttribute("url", report.Competition.Url);
            competitonElement.SetAttribute("published", report.Competition.Published.ToString("s"));

            XmlElement categories = doc.CreateElement("categories");

            int count = 0;
            foreach (string category in report.Competition.Categories)
            {
                categories.SetAttribute($"category_{count}", category);
                count++;
            }

            competitonElement.AppendChild(categories);

            Logger.Log("Competition element:" + competitonElement.OuterXml);

            // Forms element

            XmlElement formsElement = doc.CreateElement("forms");

            count = 0;
            foreach (var form in report.FormsSubmitted)
            {
                formsElement.SetAttribute($"form_{count}", form.Name);
                count++;
            }

            Logger.Log("Forms element:" + formsElement.OuterXml);

            // Modified element

            XmlElement modifiedElement = doc.CreateElement("modified");

            count = 0;
            foreach (var element in report.ElementsModified)
            {
                XmlElement modified = doc.CreateElement($"modified_{count}");
                modified.SetAttribute($"element", element.Item1.Name);
                modified.SetAttribute($"value", element.Item2);
                modifiedElement.AppendChild(modified);
                count++;
            }

            Logger.Log("Modified element:" + modifiedElement.OuterXml);

            return doc;
        }
    }
}
