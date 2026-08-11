using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace GHM.Infrastructure.Helpers
{
    public sealed class SerializationXmlHelper
    {
        /// <summary>
        /// Get data from xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath"></param>
        /// <param name="obj"></param>
        /// <returns>bool</returns>
        public static bool GetDataFromXmlFile<T>(string filePath, out T obj)
        {
            obj = default;
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    obj = (T)serializer.Deserialize(stream);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Save data to xaml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <returns>bool</returns>
        public static bool SaveToXmlFile<T>(T obj, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            try
            {
                using (TextWriter writer = new StreamWriter(filePath))
                {
                    serializer.Serialize(writer, obj);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
