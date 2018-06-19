using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace WatiN
{
    public class Archiver
    {
        public string FileName { get; }

        public IList<FormFillerReport> Buffer { get; }

        private int maxItems = 10;

        public Archiver(string fileName)
        {
            FileName = fileName;

            Buffer = new List<FormFillerReport>();
        }

        public void Archive(FormFillerReport report)
        {
            Buffer.Add(report);

            if (Buffer.Count > maxItems)
                Buffer.RemoveAt(Buffer.Count - 1); // remove oldest item

            WriteToFile(report, FileName);
        }

        public IEnumerable<Competition> NewCompetitions(IEnumerable<Competition> competitions)
        {
            IEnumerable<Competition> processed = Buffer.Select(f => f.Competition);
            return competitions.Except(processed);
        }

        private void WriteToFile(FormFillerReport report, string fileName)
        {
            Logger.Log("..");

            XmlSerializer xs = new XmlSerializer(typeof(FormFillerReport));
            var xml = "";

            using (var sww = new StringWriter())
            {
                using (XmlWriter writer = XmlWriter.Create(sww))
                {
                    xs.Serialize(writer, report);
                    xml = sww.ToString();
                }
            }

            using (StreamWriter writer = new StreamWriter(fileName))
            {
                writer.WriteLine(xml);
            }
        }

        private IReport FromXmlFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);
            try
            {
                var result = FromXmlFile(sr.ReadToEnd());
                return result;
            }
            catch (Exception e)
            {
                Logger.Log("Exception caught: " + e.Message);
                return null;
            }
            finally
            {
                sr.Close();
            }
        }

    }
}
