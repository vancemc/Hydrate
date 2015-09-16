using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hydrate
{
    public class Hydrate
    {
        /// <summary>
        /// Extracts a file name from the full embedded resource manifest name
        /// </summary>
        /// <param name="embeddedResourceName">Full embedded resource manifest name</param>
        /// <returns>File name as a string</returns>
        public static string FileNameFromEmbeddedResourceName(string embeddedResourceName)
        {
            string fullFileName = embeddedResourceName;

            if (String.IsNullOrWhiteSpace(embeddedResourceName) == false)
            {
                string[] stringSegments = embeddedResourceName.Split('.');

                if (stringSegments.Length > 1)
                {
                    string fileName = stringSegments[stringSegments.Length - 2];

                    string fileExtension = stringSegments[stringSegments.Length - 1];

                    fullFileName = String.Format(@"{0}.{1}", fileName, fileExtension);
                }
            }

            return fullFileName;
        }
    }
}
