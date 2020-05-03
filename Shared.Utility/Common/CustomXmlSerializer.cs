using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Shared.Utility.Common
{
    public static class CustomXmlSerializer
    {
        public static (string errorMsg, string result) SerializeToXml<T>(IEnumerable<T> ts)
        {
            if (ts == null) return ("The supplied parameter is null", null);

            var xs = new XmlSerializer(typeof(List<T>));
            string path = Path.Combine(Environment.CurrentDirectory, "output.xml");
            using (FileStream stream = File.Create(path))
            {
                xs.Serialize(stream, ts);
            }
            var result = File.ReadAllText(path);
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            return (null, result);
        }

        public static (string errorMsg, IEnumerable<K> resultingObj) DeserializeFromXml<K>(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return ("Path cannot be NULL or EMPTY", null);
            if (path.ValidateExternalDataSourcePath()) return ("Invalid Path", null);
            
                try
                {
                    var xs = new XmlSerializer(typeof(List<K>));
                    using (FileStream xmlLoad = File.Open(path, FileMode.Open))
                    {
                        var loadedResult = (IEnumerable<K>)xs.Deserialize(xmlLoad);
                        return (null, loadedResult);
                    }
                }
                catch (FileNotFoundException)
                {

                    return ("File Not Found", null);
                }
           

        }

    }
}
