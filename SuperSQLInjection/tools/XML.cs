using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using SuperSQLInjection.model;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;

namespace SuperSQLInjection.tools
{
    class XML
    {
        public static Boolean SaveMyConfig(){
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xdl= doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(xdl);
            return true;
        }

        public static void saveConfig(String fileName,Config config)
        {
            Stream fStream = null;
            try
            {
                fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                //创建XML序列化器，需要指定对象的类型
                XmlSerializer xmlFormat = new XmlSerializer(typeof(Config));
                xmlFormat.Serialize(fStream, config);

            }
            catch (Exception e)
            {

                throw e;
            }
            finally {
                if(fStream!=null){
                    fStream.Close();
                }
            
            }
        }

        public static Config readConfig(String configPath)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(Config));
                //创建XML序列化器，需要指定对象的类型
                fStream = new FileStream(configPath, FileMode.Open, FileAccess.ReadWrite);
                XmlTextReader reader = new XmlTextReader(fStream);
                reader.Normalization = false;
                Config config = (Config)xml.Deserialize(reader);
                return config;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally {
                if (fStream != null) {

                    fStream.Close();
                }
            }
        }

        public static void saveDBS(String fileName, DataBase dbs)
        {
            Stream fStream = null;
            try
            {
                fStream = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite);
                //创建XML序列化器，需要指定对象的类型
                XmlSerializer xmlFormat = new XmlSerializer(typeof(DataBase));
                xmlFormat.Serialize(fStream, dbs);
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                if (fStream != null)
                {
                    fStream.Close();
                }

            }
        }

        public static DataBase readDBS(String path)
        {
            Stream fStream = null;
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(DataBase));
                //创建XML序列化器，需要指定对象的类型
                fStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
                XmlTextReader reader = new XmlTextReader(fStream);
                reader.Normalization = false;
                DataBase config = (DataBase)xml.Deserialize(reader);
                return config;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                if (fStream != null)
                {

                    fStream.Close();
                }
            }
        }
    }
}
