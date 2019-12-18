using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Neis.ProductKeyManager.Data
{
    public static class DataUtility<T> where T : NotifiableBase
    {
        /// <summary>
        /// Loads data from a given file
        /// </summary>
        /// <param name="filepath">Path to file to load data from</param>
        /// <returns>If valid, this will return an object containing data</returns>
        public static T LoadFromFile(string filepath)
        {
            var retVal = default(T);

            using (var filestream = new FileStream(filepath, FileMode.Open))
            {
                var serializer = new XmlSerializer(typeof(T));
                var reader = new XmlTextReader(filestream);

                retVal = serializer.Deserialize(reader) as T;
            }

            return retVal;
        }
        /// <summary>
        /// Saves an object to a given file
        /// </summary>
        /// <param name="filepath">Path to file to save data to</param>
        /// <param name="obj">Object containing data to save</param>
        public static void SaveToFile(string filepath, T obj)
        {
            using (var filestream = new FileStream(filepath, FileMode.Create))
            {
                var serializer = new XmlSerializer(typeof(T));
                
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = true;

                var writer = XmlTextWriter.Create(filestream, settings);

                serializer.Serialize(writer, obj);
            }
        }

        /// <summary>
        /// Builds an object from an XML string
        /// </summary>
        /// <param name="xml">XML string to build object from</param>
        /// <returns>New object from XML string</returns>
        public static T FromXmlString(string xml)
        {
            var retVal = default(T);

            using (var sReader = new StringReader(xml))
            {
                var serializer = new XmlSerializer(typeof(T));
                var xReader = new XmlTextReader(sReader);

                retVal = serializer.Deserialize(xReader) as T;
            }

            return retVal;
        }
        /// <summary>
        /// Builds an object from a JSON string
        /// </summary>
        /// <param name="json">JSON string to build object from</param>
        /// <returns>New object from JSON string</returns>
        public static T FromJsonString(string json)
        {
            var retVal = default(T);

            using (var sReader = new StringReader(json))
            {
                var serializer = new JsonSerializer();
                var jReader = new JsonTextReader(sReader);

                retVal = serializer.Deserialize<T>(jReader);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the XML text representation of an object
        /// </summary>
        /// <param name="obj">Object to tranform into XML</param>
        public static string ToXmlString(T obj)
        {
            var sb = new StringBuilder();
            var serializer = new XmlSerializer(typeof(T));

            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            using (var writer = XmlTextWriter.Create(sb, settings))
            {
                serializer.Serialize(writer, obj);
            }

            return sb.ToString();
        }
        /// <summary>
        /// Gets the JSON text representation of an object
        /// </summary>
        /// <param name="obj">Object to tranform into JSON</param>
        public static string ToJsonString(T obj)
        {
            var sb = new StringBuilder();

            var serializer = new JsonSerializer();

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, obj);
            }
            
            return sb.ToString();
        }
    }
}
