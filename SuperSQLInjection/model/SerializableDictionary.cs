using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace SuperSQLInjection.model
{

        [Serializable]
        public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
        {
            public SerializableDictionary() { }
            public void WriteXml(XmlWriter write)       // Serializer
            {
                XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

                foreach (KeyValuePair<TKey, TValue> kv in this)
                {
                    write.WriteStartElement("SerializableDictionary");
                    write.WriteStartElement("key");
                    KeySerializer.Serialize(write, kv.Key);
                    write.WriteEndElement();
                    write.WriteStartElement("value");
                    ValueSerializer.Serialize(write, kv.Value);
                    write.WriteEndElement();
                    write.WriteEndElement();
                }
            }
            public void ReadXml(XmlReader reader)       // Deserializer
            {
                reader.Read();
                XmlSerializer KeySerializer = new XmlSerializer(typeof(TKey));
                XmlSerializer ValueSerializer = new XmlSerializer(typeof(TValue));

                while (reader.NodeType != XmlNodeType.EndElement)
                {
                
                    reader.ReadStartElement("SerializableDictionary");
                    reader.ReadStartElement("key");
                    TKey tk = (TKey)KeySerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadStartElement("value");
                    TValue vl = (TValue)ValueSerializer.Deserialize(reader);
                    reader.ReadEndElement();
                    reader.ReadEndElement();
                    this.Add(tk, vl);
                    reader.MoveToContent();
                }
                reader.ReadEndElement();

            }
            public XmlSchema GetSchema()
            {
                return null;
            }
        }
    }

