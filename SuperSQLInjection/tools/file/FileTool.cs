using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using SuperSQLInjection.model;

namespace tools
{
    class FileTool
    {
        public static List<string> readAllDic(String dic)
        {
            List<string> fs = new List<string>();
            try
            {
                DirectoryInfo din = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory+"/"+dic);
                FileInfo[] files = din.GetFiles();
                foreach (FileInfo f in files)
                {
                    fs.Add(f.Name);
                }
            }
            catch (Exception re)
            {
                Tools.SysLog(dic + "读取错误！" + re.Message);
            }
            return fs;
        }
        public static List<String> readFileToList(String path)
        {
            
            List<String> list = new List<String>();
            FileStream fs_dir = null;
            StreamReader reader = null;
            try
            {
                fs_dir = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/" + path, FileMode.Open, FileAccess.Read);

                reader = new StreamReader(fs_dir);

                String lineStr;

                while ((lineStr = reader.ReadLine()) != null)
                {
                    if (!lineStr.Equals(""))
                    {
                       list.Add(lineStr);
                    }
                }
            } catch (Exception e)
            {
                Tools.SysLog(e.Message);
            }
            finally {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }
            return list;
        }

        public static HashSet<String> readDomainToList(String path,Boolean isCleanExists)
        {

            HashSet<String> list = new HashSet<String>();
            FileStream fs_dir = null;
            StreamReader reader = null;
            try
            {
                fs_dir = new FileStream(path, FileMode.Open, FileAccess.Read);

                reader = new StreamReader(fs_dir);

                String lineStr;

                while ((lineStr = reader.ReadLine()) != null)
                {
                    if (!lineStr.Equals("")&&!lineStr.StartsWith("http")) {
                        lineStr = "http://" + lineStr;
                    }
                    if (list.Contains(lineStr) && isCleanExists) {
                        continue;
                    }
                    list.Add(lineStr);

                }
            }
            catch (Exception e)
            {
                Tools.SysLog(e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }
            return list;
        }

        //读取文件
        public static String readFileToString(String path)
        {
            String str = "";
            FileStream fs_dir=null;
            StreamReader reader = null;
            try
            {
                fs_dir = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/" + path, FileMode.Open, FileAccess.Read);
                reader = new StreamReader(fs_dir);
                str = reader.ReadToEnd();
            }
            catch (Exception e)
            {
                Tools.SysLog("readFileToString发生异常！"+e.Message);
            }finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }
            return str;
            
        }
        //读取文件
        public static Byte[] readFileToByte(String path,Encoding encode)
        {
            Byte[] buffer = null;
            FileStream fs_dir=null;
            StreamReader reader = null;
            try
            {
                fs_dir = new FileStream(path, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs_dir, encode);
                int len = (int)fs_dir.Length;

                buffer = new byte[len];

                int size = br.Read(buffer, 0, len);

                reader.Read();
                
            }
            catch (Exception e)
            {
                Tools.SysLog("readFileToByte-error:读取文件内容发生错误！"+e.Message);
            }finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }
            return buffer;
            
        }
        public static object c = "";
        public static String error = "";
        public static void AppendLogToFile(String path,String log)
        {
            //锁住，防止多线程引发错误
            lock (c)
            {
                List<String> list = new List<String>();
                FileStream fs_dir = null;
                StreamWriter sw = null;
                try
                {
                    fs_dir = new FileStream(AppDomain.CurrentDomain.BaseDirectory + "/" + path, FileMode.Append, FileAccess.Write);

                    sw = new StreamWriter(fs_dir);

                    sw.WriteLine(log);
                }
                catch (Exception e)
                {
                    error = "文件操作发生异常！" + e.Message;
                }
                finally
                {
                    if (sw != null)
                    {
                        sw.Close();
                    }
                    if (fs_dir != null)
                    {
                        fs_dir.Close();
                    }
                }
            }

        }
        /// <summary>
        /// 注意：vals为空或null会清空代理
        /// </summary>
        /// <param name="path"></param>
        /// <param name="vals"></param>
        public static void SaveProxyList(String path, Dictionary<String,Proxy>.ValueCollection vals)
        {
     
            FileStream fs_dir = null;
            StreamWriter sw = null;
            try
            {
                if (vals != null && vals.Count > 0)
                {
                    fs_dir = new FileStream(path, FileMode.Create, FileAccess.Write);
                    sw = new StreamWriter(fs_dir, Encoding.UTF8);
                    foreach(Proxy proxy in vals)
                    {
                        String line = proxy.host + "\t" + proxy.port + "\t" + proxy.proxyType + "\t" + proxy.username + "\t" + proxy.password + "\t" + proxy.isOk + "\t" + proxy.useTime + "\t" + proxy.checkTime;
                        sw.WriteLine(line);
                    }
                }
                else {
                    //如果为空，则删除代理
                    File.Delete(path);
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("保存代理池发生异常！" + e.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }

        }

        public static Dictionary<String,Proxy> ReadProxyList(String path)
        {

            Dictionary<String, Proxy> list = new Dictionary<String, Proxy>();
            FileStream fs_dir = null;
            StreamReader reader = null;
            try
            {
                fs_dir = new FileStream(path, FileMode.Open, FileAccess.Read);

                reader = new StreamReader(fs_dir);

                String lineStr;

                while ((lineStr = reader.ReadLine()) != null)
                {
                    if (!lineStr.Equals(""))
                    {
                        String[] strs = lineStr.Split('\t');
                        if (strs.Length == 8) {
                            Proxy proxy = new Proxy();
                            proxy.host = strs[0];
                            proxy.port= Tools.convertToInt(strs[1]);
                            proxy.proxyType= strs[2];
                            proxy.username = strs[3];
                            proxy.password = strs[4];
                            proxy.isOk = strs[5];
                            proxy.useTime = Tools.convertToInt(strs[6]);
                            proxy.checkTime = strs[7];
                            list.Add(proxy.host + proxy.port, proxy);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("ReadProxyList异常："+e.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (fs_dir != null)
                {
                    fs_dir.Close();
                }
            }
            return list;
        }


    }
}
