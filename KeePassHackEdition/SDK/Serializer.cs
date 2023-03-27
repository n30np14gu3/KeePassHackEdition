using System.IO;
using System.Xml.Serialization;
using System.Xml;

namespace KeePassHackEdition.SDK
{
    public class Serializer<T>
    {
        public static string Serialize(T obj)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (var sww = new StringWriter())
            {
                using (XmlTextWriter writer = new XmlTextWriter(sww))
                {
                    xsSubmit.Serialize(writer, obj);
                    return sww.ToString();
                }
            }
        }

        public static object Deserialize(string xml)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(T));
            using (StringReader sr = new StringReader(xml))
            {
                return xsSubmit.Deserialize(sr);
            }
        }
    }
}