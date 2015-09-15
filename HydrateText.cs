using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace DataStreamTools
{
    public class HydrateText
    {
        public static string ReadStringFromEmbeddedResource(string xmlResourceName)
        {
            var assembly = Assembly.GetCallingAssembly();

            string result = ReadStringFromEmbeddedResource(assembly, xmlResourceName);

            return result;
        }
        public static string ReadStringFromEmbeddedResource(Assembly assembly, string xmlResourceName)
        {
            string result = string.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(xmlResourceName))
            {
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
