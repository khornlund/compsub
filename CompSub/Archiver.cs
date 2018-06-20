using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;

namespace CompSub
{
    /// <summary>
    /// This class is responsible for recording which competitions have been entered
    /// </summary>
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
            using (StreamReader sr = new StreamReader(FileName))
            {
                foreach (var competition in competitions)
                {
                    var processed = sr.ReadToEnd();
                    if (!processed.Contains(competition.Url))
                        yield return competition;
                }
            }
        }

        private void WriteToFile(FormFillerReport report, string filename)
        {
            Logger.Log("..");

            if (!File.Exists(filename))
            {
                File.Create(filename);
            }

            using (StreamWriter writer = File.AppendText(filename))
            {
                //writer.WriteLine(report.ToXml().OuterXml);
                writer.WriteLine(report.Competition.Url);
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
