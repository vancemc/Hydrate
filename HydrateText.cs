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
        /// <summary>
        /// Streams an embedded ressource text file out to a string. This overload defaults to the executing assembly. You can call Assembly.GetManifestResourceNames() to get a list of available resource names.
        /// </summary>
        /// <param name="embeddedResourceManifestName"></param>
        /// <returns>A string that contains the text of an embedded resource file.</returns>
        public static string ReadEmbeddedResourceText(string embeddedResourceManifestName)
        {
            var assembly = Assembly.GetExecutingAssembly();

            string result = ReadEmbeddedResourceText(assembly, embeddedResourceManifestName);

            return result;
        }

        /// <summary>
        /// Streams an embedded ressource text file within an assembly out to a string. You can call Assembly.GetManifestResourceNames() to get a list of available resource names.
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="embeddedResourceManifestName"></param>
        /// <returns>A string that contains the text of an embedded resource file.</returns>
        public static string ReadEmbeddedResourceText(Assembly assembly, string embeddedResourceManifestName)
        {
            string result = string.Empty;

            using (Stream stream = assembly.GetManifestResourceStream(embeddedResourceManifestName))
            {
                StreamReader reader = new StreamReader(stream);
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
