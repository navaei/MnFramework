using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Mn.Framework.Serialization
{
    public class XmlSerializeHelper
    {        
        /// <summary>
        /// deserialize an xdocument to xml string
        /// </summary>
        /// <typeparam name="T">T is type of output</typeparam>
        /// <param name="doc">source document used in deserialization</param>
        /// <returns>object with type T</returns>
        public static T Deserialize<T>(XDocument doc)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = doc.Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// deserialize an xdocument string to xml string
        /// </summary>
        /// <typeparam name="T">T is type of output</typeparam>
        /// <param name="doc">XDocument as string</param>
        /// <returns>object with type T</returns>
        public static T Deserialize<T>(string doc)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var reader = XDocument.Parse(doc).Root.CreateReader())
            {
                return (T)xmlSerializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Serialize an object as xml document
        /// </summary>
        /// <typeparam name="T">T is type object you want to serialize</typeparam>
        /// <param name="value">value of T ( use in serialization )</param>
        /// <returns>filled XDocument</returns>
        public static XDocument Serialize<T>(T value)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            ns.Add(string.Empty, string.Empty);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            XDocument doc = new XDocument();
            using (var writer = doc.CreateWriter())
            {
                xmlSerializer.Serialize(writer, value, ns);
            }

            return doc;
        }

        

        // deserialize List
        public static List<T> DeserializeList<T>(XDocument doc)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<T>));

            System.Xml.XmlReader reader = doc.CreateReader();

            List<T> result = (List<T>)serializer.Deserialize(reader);
            reader.Close();

            return result;
        }

        public static string SerializeListToXml<T>(List<T> source)
        {
            XmlSerializer serializer = new XmlSerializer(source.GetType());

            StringWriter stringWriter = new StringWriter();
          
            //Setting for xml
            var settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.OmitXmlDeclaration = true;

            XmlWriter xmlWriter = XmlWriter.Create(stringWriter, settings);

            //Remove Qualifier feolds from nodes
            var emptyNs = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });

            serializer.Serialize(xmlWriter, source, emptyNs);

            return stringWriter.ToString();
        }

        [Serializable]
        [XmlType(TypeName = "Item")]
        public struct KeyValueSerializable<TK, TV>
        {
            public TK Key
            { get; set; }

            public TV Value
            { get; set; }
        }
    }
    
}