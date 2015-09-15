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
    public class HydrateXml
    {
        public static void SerializeToFile<T>(object typeInstance, string xmlFileName, bool append=false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StreamWriter file = new StreamWriter(xmlFileName, append))
            {
                xmlSerializer.Serialize(file, typeInstance);
            }
        }

        public static string SerializeToString<T>(object typeInstance)
        {
            string result = string.Empty;

            if (typeInstance != null)
            {
                var xmlSerializer = new XmlSerializer(typeof(T));

                using (var stringwriter = new System.IO.StringWriter())
                {
                    xmlSerializer.Serialize(stringwriter, typeInstance);
                    result = stringwriter.ToString();
                }
            }

            return result;
        }

        public static T DeSerializeFromFile<T>(string xmlFileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T hydratedObject;

            using (FileStream fileStream = new FileStream(xmlFileName, FileMode.Open))
            {
                hydratedObject = (T) xmlSerializer.Deserialize(fileStream);
            }

            return (T) hydratedObject;
        }

        public static T DeSerializeFromEmbeddedResource<T>(string xmlResourceName)
        {
            var assembly = Assembly.GetCallingAssembly();

            var result = DeSerializeFromEmbeddedResource<T>(assembly, xmlResourceName);

            return result;
        }

        public static T DeSerializeFromEmbeddedResource<T>(Assembly assembly, string xmlResourceName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T hydratedObject;

            using (Stream stream = assembly.GetManifestResourceStream(xmlResourceName))
            {
                StreamReader reader = new StreamReader(stream);
                hydratedObject = (T) xmlSerializer.Deserialize(reader);
            }

            return (T) hydratedObject;
        }

        public static T DeSerializeFromString<T>(string serializedXmlString, Encoding encoding = null)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T hydratedObject;

            using (Stream stream = ConvertStringToMemoryStream(encoding, serializedXmlString))
            {
                StreamReader reader = new StreamReader(stream);
                hydratedObject = (T)xmlSerializer.Deserialize(reader);
            }

            return (T)hydratedObject;
        }

        public static T DeSerializeFromXmlDoc<T>(XmlDocument xmlDoc, Encoding encoding = null)
        {
            var serializedXmlString = xmlDoc.InnerXml;

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            T hydratedObject = default(T);

            if (xmlDoc != null)
            {
                hydratedObject = DeSerializeFromString<T>(serializedXmlString, encoding);
            }

            return (T)hydratedObject;
        }

        private static MemoryStream ConvertStringToMemoryStream(Encoding encoding, string payload)
        {
            if(encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var memStream = new MemoryStream(encoding.GetBytes(payload ?? ""));

            return memStream;
        }
    }
}
