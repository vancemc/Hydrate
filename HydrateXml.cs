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
        /// <summary>
        /// Serializes a serializable xml type (class) instance to a file.
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="typeInstance">The instance of the class of type T</param>
        /// <param name="xmlFileName">Full file path including the file name or just the file name.</param>
        /// <param name="append">If true, appends content but will not de-serialize (appending will most likely break the xml schema). Logging content is a good use for this, otherwise default is false</param>
        public static void SerializeToFile<T>(object typeInstance, string xmlFileName, bool append=false)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (StreamWriter file = new StreamWriter(xmlFileName, append))
            {
                xmlSerializer.Serialize(file, typeInstance);
            }
        }

        /// <summary>
        /// Serializes a type (class) instance out to a string
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="typeInstance">Instance of type T</param>
        /// <returns></returns>
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

        /// <summary>
        /// Reads serialized xml from a file and returns a class instance containing de-serialized values
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="xmlFileName">full xml file path or just file name if file is in the path</param>
        /// <returns></returns>
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

        /// <summary>
        /// De-serializes an xml file embedded as a resource in the executing assembly
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="xmlResourceName">Embedded resource manifest name. Call Assembly.GetResourceManefestNames() to get full name.</param>
        /// <returns>A class instance of type T containing de-serialized values</returns>
        public static T DeSerializeFromEmbeddedResource<T>(string xmlResourceName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            var result = DeSerializeFromEmbeddedResource<T>(assembly, xmlResourceName);

            return result;
        }


        /// <summary>
        /// De-serializes an xml file embedded as a resource in the specified assembly instance
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="assembly">Assembly instance</param>
        /// <param name="xmlResourceName">Embedded resource manifest name. Call Assembly.GetResourceManefestNames() to get full name.</param>
        /// <returns>A class instance of type T containing de-serialized values</returns>
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

        /// <summary>
        /// De-serializes from a string containing xml
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="serializedXmlString">string that contains the serialized xml</param>
        /// <param name="encoding">Specify the text encoding type. Default is null</param>
        /// <returns>Type (class) instance containing de-serialized values</returns>
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

        /// <summary>
        /// De-serialized xml from an XmlDocument instance
        /// </summary>
        /// <typeparam name="T">Type (class name)</typeparam>
        /// <param name="xmlDoc">XmlDocument instance</param>
        /// <param name="encoding">Text encoding. Default is null</param>
        /// <returns></returns>
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
