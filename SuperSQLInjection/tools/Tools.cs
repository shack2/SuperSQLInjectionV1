using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Threading;
using tools;
using model;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;
using SuperSQLInjection.model;
using SuperSQLInjection;
using SuperSQLInjection.tools;

namespace tools
{
    class Tools
    {
        public const String httpLogPath = "logs/http/";

        //由于计数器有误差（可能客户端计数小于服务端，，如果页面正常响应时间非常快，可能导致返回时间可能提前，所以考虑设置一个误差值）
        public const int deviation = 20;

        public static long currentMillis()
        {
            return (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;
        }
        public static bool ThreadPoolIsEnd()
        {
            int workerThreads = 0;
            int maxWordThreads = 0;
            //int 
            int compleThreads = 0;
            ThreadPool.GetAvailableThreads(out workerThreads, out compleThreads);
            ThreadPool.GetMaxThreads(out maxWordThreads, out compleThreads);

            if (maxWordThreads == workerThreads)
            {
                return true;
            }
            else {
                return false;
            }
        }

        public static void SysLog(String log)
        {
            FileTool.AppendLogToFile("logs/" + DateTime.Now.ToLongDateString() + ".log.txt", log + "----" + DateTime.Now);
        }
        
        /// <summary>
        /// 随机生成小写字母
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static String RandStr(int len)
        {
            StringBuilder str = new StringBuilder();
            Random rd = new Random();
            for (int i=0;i<len;i++) {
                char c=(char)rd.Next(97, 122);
                str.Append(c);
            }
            return str.ToString();
        }

        /// <summary>
        /// 随机生成小写字母
        /// </summary>
        /// <param name="len"></param>
        /// <returns></returns>
        public static String RandNum(int len)
        {
            StringBuilder str = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < len; i++)
            {
                char c = (char)rd.Next(49, 58);
                str.Append(c);
            }
            return str.ToString();
        }

        public static String fomartTime(String time)
        {
            try
            {
                DateTime dt = Convert.ToDateTime(time);
                String newtime = dt.ToLocalTime().ToString();
                return newtime;
            }
            catch (Exception e)
            {
                SysLog(e.Message);
            }
            return time;

        }

        /// <summary>
        /// 二分法取较大整数，用于盲注判断
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int getLargeNum(int start,int end){
  
            int sum=start+end;
            if (sum == 1) {
                return 0;
            }
            if (sum % 2 == 0)
            {
                return sum / 2;
             }
             else {
                    return sum / 2;
             }
        
        }


        public static String unHexByUnicode(int unicode,String encoding){

            int c = Tools.UnicodeInt2UTF8Int(unicode);
            return Tools.unHex(Convert.ToString(c, 16), encoding);
        
        }

        public static String hexToRaw(string str,String encoding){
            if (str.Length % 2 == 0)
            {
                byte[] b = new byte[str.Length / 2];
                int j = 0;  
                for (int i = 0; i < str.Length; i += 2){
                    byte by = Convert.ToByte(str.Substring(i, 2), 16);//取两个字符，转换成对应的字节
                    b[j] = by;
                    j++;
                }
                return Encoding.GetEncoding(encoding).GetString(b);
            }
            else{
                throw new Exception("不能将该字符串转换成String类型!");
            }
        }

        public static void sysHTTPLog(String index ,ServerInfo server)
        {
            FileTool.AppendLogToFile(httpLogPath + index + "-request.txt", server.request);
            FileTool.AppendLogToFile(httpLogPath + index + "-response.txt", server.header + "\r\n\r\n" + server.body);
        }

        public static void delHTTPLog()
        {
            delAllFiles(AppDomain.CurrentDomain.BaseDirectory+"/"+httpLogPath);
        }
        public static void delAllFiles(String path)
        {
            try
            {
                DirectoryInfo din = new DirectoryInfo(path);
                FileInfo[] files = din.GetFiles();
                DirectoryInfo[] dis = din.GetDirectories();
                foreach (FileInfo f in files)
                {
                    f.Delete();
                }
                foreach (DirectoryInfo df in dis)
                {
                    delAllFiles(df.FullName);
                    df.Delete();
                }
            }
            catch (Exception re)
            {
                Tools.SysLog("删除日志发生错误！" + re.Message);
            }
        }

        public static void delFile(String filepath)
        {
            try
            {
                File.Delete(filepath);
            }
            catch (Exception re)
            {
                Tools.SysLog("删除日志发生错误！" + re.Message);
            }
        }

        public static List<String> readAllXmlFile(String dir, List<String> list)
        {
            try
            {
                if (list == null) {
                    list = new List<String>();
                }
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] files = d.GetFiles();//文件
                DirectoryInfo[] directs = d.GetDirectories();//文件夹
                foreach (FileInfo f in files)
                {
                    if (f.Extension.EndsWith("xml")) {
                        list.Add(f.FullName);
                    }
                }
                //获取子文件夹内的文件列表，递归遍历  
                foreach (DirectoryInfo dd in directs)
                {
                    readAllXmlFile(dd.FullName, list);
                }
                
            }
            catch (Exception re)
            {
                Tools.SysLog("读取文件发生错误！" + re.Message);
            }
            return list;
        }


        /// <summary>
        /// Hex解码
        /// </summary>
        /// <param name="hex">Hex编码</param>
        /// <param name="charset">字符编码</param>
        /// <returns></returns>
        public static string unHex(string hex, string charset){
            if (hex == null)throw new ArgumentNullException("hex");
            hex = hex.Replace(",", "");
            hex = hex.Replace("\n", "");
            hex = hex.Replace("\\", "");
            hex = hex.Replace(" ", ""); 
            if (hex.Length % 2 != 0){
                hex += "20";//空格
            }
            // 需要将 hex 转换成 byte 数组。              
            byte[] bytes = new byte[hex.Length / 2];            
            for (int i = 0; i < bytes.Length; i++){ 
            try{
                // 每两个字符是一个 byte。
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                System.Globalization.NumberStyles.HexNumber);
            } catch{
                // Rethrow an exception with custom message.
                SysLog("unHex解码错误---hex is not a valid hex number!");
            }
            }
            Encoding chs = Encoding.GetEncoding(charset);
            return chs.GetString(bytes);
        }

        public static string StringToHexString(string s, Encoding encode)
        {
            byte[] b = encode.GetBytes(s);//按照指定编码将string编程字节数组
            string result = string.Empty;
            for (int i = 0; i < b.Length; i++)//逐字节变为16进制字符
            {
                result += Convert.ToString(b[i], 16);
            }
            return result;
        }


        public static String HexStringToString(String hs, String encodeStr)
        {
            try
            {
                Encoding encode = Encoding.GetEncoding(encodeStr);
                string strTemp = "";
                byte[] b = new byte[hs.Length / 2];
                for (int i = 0; i < hs.Length / 2; i++)
                {
                    strTemp = hs.Substring(i * 2, 2);
                    b[i] = Convert.ToByte(strTemp, 16);
                }
                return encode.GetString(b);
            }
            catch (Exception e)
            {
             
                SysLog("HexStringToString解码错误!"+e.GetBaseException());
            }
            return "";
            
        }

        /// <summary>
        /// 将数组转换成字符串
        /// </summary>
        /// <param name="strs"></param>
        /// <returns></returns>
        public static String convertToString(String[] strs){

            return convertToString(strs,false);


        }

       
        public static String convertToString(String[] strs,bool appendNewLine)
        {
           
            StringBuilder sb = new StringBuilder();
            foreach (String s in strs)
            {
                sb.Append(s);
                if (appendNewLine) {
                    sb.Append("\r\n");
                }  
            }
            return sb.ToString();

        }

        /// <summary>
        /// 将字符串转换成数字，错误返回0
        /// </summary>
        /// <param name="strs">字符串</param>
        /// <returns></returns>
        public static int convertToInt(String str)
        {

            try
            {
                return int.Parse(str);
            }
            catch (Exception e) {
                Tools.SysLog("info:数字转换错误:-"+e.Message);
            }
            return 0;

        }
        /// <summary>
        /// 将16进制转换成10进制
        /// </summary>
        /// <param name="str">16进制字符串</param>
        /// <returns></returns>
        public static int convertToIntBy16(String str)
        {
          try
            {
                return Convert.ToInt32(str,16);
            }
            catch (Exception e)
            {
                
            }
            return 0;

        }

        public static int findKeyCount(String str,String key)
        {
            int count = 0;
            try
            {
                if (!String.IsNullOrEmpty(str))
                {
                    int index = 0;

                    while (index != -1)
                    {
                        index = str.IndexOf(key, index + 1);
                        if (index != -1)
                        {
                            count++;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Tools.SysLog("findKeyCount发生异常！"+e.Message);
            }
            return count;

        }

        public static Boolean checkEmpty(String str) {

            if (str != null && str.Length > 0)
            {
                return false;
            }
            else {
                return true;
            }
        }

        public static String StringArrayToString(String[] array)
        {
            StringBuilder sb = new StringBuilder();
            foreach (String s in array) {

                if (s != null)
                {

                    sb.Append(s);
                }
                else {

                    sb.Append("_");
                }
            
            }
            return sb.ToString();
        }
        /// <summary>
        /// 判断页面注入true或false
        /// </summary>
        /// <param name="server">服务器响应对象ServerInfo</param>
        /// <param name="isUseCode">是否使用状态码判断</param>
        /// <param name="key">关键字</param>
        /// <returns></returns>
        public static Boolean isTrue(ServerInfo server,String key,Boolean reverKey,KeyType keyType,int trueHTTPCode)
        {
            switch (keyType) {
               
                case KeyType.Key:

                    //用关键字判断
                    if (server.body.Length > 0 && server.body.IndexOf(key)!=-1)
                    {
                        if (reverKey)
                        {                    
                                return false;
                        }
                        else
                        {
                            //判断httpcode是否一致
                            if (trueHTTPCode != 0 && server.code == trueHTTPCode) {
                                return true;
                            }
                            return false;
                               
                        }
                       
                    }
                    else
                    {
                        if (reverKey)
                        {
                            //判断httpcode是否一致
                            if (trueHTTPCode != 0 && server.code == trueHTTPCode)
                            {
                                return true;
                            }
                            return true;
                        }
                        return false;
                    }

                case KeyType.Reg:

                    //用正则判断
                    if (server.body.Length > 0 && Regex.IsMatch(server.body, key))
                    {
                        ;
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }
                case KeyType.Code:
                    //用状态码判断
                    if (server.code > 0 && key.Equals(server.code + ""))
                    {
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }
                

                case KeyType.Time:
                    //由于计数器有误差（可能客户端计数小于服务端，，如果页面正常响应时间非常快，可能导致返回时间可能提前，所以考虑设置一个误差值）
                    int time = Tools.convertToInt(key);
                    if (server.runTime > (time*1000-(time*deviation)))
                    {
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }

                case KeyType.EQLen:
                    //用长度判断
                    if (key.Equals(server.length.ToString()))
                    {
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }

                case KeyType.MaxLen:
                    //用长度判断
                    if (server.length>Tools.convertToInt(key))
                    {
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }
                case KeyType.MinLen:
                    //用长度判断
                    if (server.length < Tools.convertToInt(key))
                    {
                        if (reverKey)
                        {
                            return false;
                        }
                        return true;
                    }
                    else
                    {
                        if (reverKey)
                        {
                            return true;
                        }
                        return false;
                    }

            }
            return false;
           
        }
       
        public static String strToHex(String str,String encode)
        {
            try
            {
                
                StringBuilder sb = new StringBuilder();//  存储转换后的编码
                Byte[] strByte=Encoding.GetEncoding(encode).GetBytes(str);
                foreach (Byte s in strByte)
                {
                    sb.Append(s.ToString("x").PadLeft(2, '0'));
                }
                return "0x" + sb.ToString();


            }
            catch (Exception e)
            {
                Tools.SysLog("hex转换错误！"+ e.Message);
            }
            return "";
        }

        /// <summary>
        /// byte[]转hex，udf调用
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String bytesToHex(byte[] bytes)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (bytes != null && bytes.Length > 0) {
                    foreach (Byte s in bytes)
                    {
                        sb.Append(s.ToString("x").PadLeft(2, '0'));
                    }
                   
                }
                return sb.ToString();
            }
            catch (Exception e)
            {
                Tools.SysLog("bytesToHex转换错误！" + e.Message);
            }
            return "";
        }

        /// <summary>
        /// byte[]转hex，udf调用
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static String FileToHex(String path,Encoding encode)
        {
            try
            {
                byte[] filedata=FileTool.readFileToByte(path, encode);
                return bytesToHex(filedata);
            }
            catch (Exception e)
            {
                Tools.SysLog("FileToHex转换错误！" + e.Message);
            }
            return "";
        }

        /// <summary>
        /// 转换chr供postgresql替换库名，防止单引号被拦截或过滤
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static String strToChr(String str, String encode)
        {
            return strToChrOrChar(str, "chr", "||", encode);
        }

       
        public static String strToChrOrChar(String str, String charFunction,String charConcatStr,String encode)
        {
            try
            {

                StringBuilder sb = new StringBuilder();
                Byte[] strByte = Encoding.GetEncoding(encode).GetBytes(str);
                foreach (Byte s in strByte)
                {
                    sb.Append(charFunction+"(" + s + ")"+ charConcatStr);
                }
                return sb.Remove(sb.Length - charConcatStr.Length, charConcatStr.Length).ToString();
            }
            catch (Exception e)
            {
                Tools.SysLog("strToChrOrChar错误！" + e.Message);
            }
            return "";
        }

        
        public static String chrOrCharToStr(String str, String charFunction, String encode)
        {
            try
            {
                String[] chars = str.Split(' ');
                if (chars.Length > 0) {

                    Byte[] bs = new Byte[chars.Length];
                    int index = 0;
                    foreach (String s in chars)
                    {
                        String cs = s.Replace(charFunction,"").Replace(charFunction + "(", "").Replace(charFunction + ")", "");
                        Byte b = (Byte)Tools.convertToInt(cs);
                        bs[index] = b;
                        index++;
                    }
                    return Encoding.GetEncoding(encode).GetString(bs);
                   
                }
                    
            }
            catch (Exception e)
            {
                Tools.SysLog("strToChrOrChar错误！" + e.Message);
            }
            return "";
        }


        public static String strToChar(String str,String encode,String joinStr)
        {
            return strToChrOrChar(str, "char", joinStr, encode);
        }

        public static String strToChr(String str, String encode, String joinStr)
        {
            return strToChrOrChar(str, "chr", joinStr, encode);
        }
        public static String informixStrToChr(String randstr)
        {
            return "to_char("+ randstr + ")";
        }

        /// <summary>
        /// 转换chr供SQLServer替换库名，防止单引号被拦截或过滤
        /// </summary>
        /// <param name="str"></param>
        /// <param name="encode"></param>
        /// <returns></returns>
        public static String strToChar(String str, String encode)
        {
            return strToChrOrChar(str, "char", "+", encode);
        }
        public static int UnicodeInt2UTF8Int(int UnicodeInt)
        {
            if (UnicodeInt < 128)
            {
                return UnicodeInt;
            }
            int num = UnicodeInt >> 12 & 15;
            int num2 = UnicodeInt >> 6 & 63;
            int num3 = UnicodeInt & 63;
            return (num + 224 << 16) + (num2 + 128 << 8) + (num3 + 128);
        }

        public static int UTF8Int2UnicodeInt(int UTF8Int)
        {
            if (UTF8Int < 128)
            {
                return UTF8Int;
            }
            int num = UTF8Int >> 16 & 15;
            int num2 = UTF8Int >> 8 & 63;
            int num3 = UTF8Int & 63;
            return (num << 12) + (num2 << 6) + num3;
        }

        public static String randIP()
        {
            Random rd = new Random();

            String ip = rd.Next(1, 255) + "." + rd.Next(1, 255) + "." + rd.Next(1, 255) + "." + rd.Next(1, 255);

            return ip;
        }

        public static String stringToAscii(String str)
        {
            char[] cstr = str.ToCharArray();
            StringBuilder sb = new StringBuilder();
            foreach (char c in cstr) {
                sb.Append(Convert.ToInt32(c) + " ");
            }
            if (sb.Length > 1) {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

        public static String asciiToString(String str)
        {
            try
            {
                String[] sstr = str.Split(' ');
                StringBuilder sb = new StringBuilder();
                foreach (String c in sstr)
                {
                    sb.Append(((char)(int.Parse(c))));
                }
                return sb.ToString();
            }
            catch (Exception e) {

                Tools.SysLog("waring:asciiToString发生错误，"+e.Message);
            
            }
            return "";
        }
        /**
         字符转unicode
         */
        public static string String2Unicode(string source)
        {
            var bytes = Encoding.Unicode.GetBytes(source);
            var stringBuilder = new StringBuilder();
            for (var i = 0; i < bytes.Length; i += 2)
            {
                stringBuilder.AppendFormat("%u{0}{1}", bytes[i + 1].ToString("x").PadLeft(2, '0'), bytes[i].ToString("x").PadLeft(2, '0'));
            }
            return stringBuilder.ToString();
        }
        public static decimal getLike(String body1, String body2)
        {

            String[] keys1 = Regex.Split(body1, "[^\\u0080-\\uFFFF\\w\\-\\d]+");
            String[] keys2 = Regex.Split(body2, "[^\\u0080-\\uFFFF\\w\\-\\d]+");

            HashSet<String> hash1 = new HashSet<String>();
            HashSet<String> hash2 = new HashSet<String>();
            foreach (String key in keys1)
            {
                if (!hash1.Contains(key))
                {
                    hash1.Add(key);
                }
            }
            foreach (String key in keys2)
            {
                if (!hash2.Contains(key))
                {
                    hash2.Add(key);
                }
            }
            int count = 0;
            foreach (String key in hash2)
            {
                if (hash1.Contains(key))
                {
                    count++;
                }
            }
            decimal p = 0;
            if (hash1.Count > 0)
            {
                decimal cc = (decimal)((float)count * 100 / hash1.Count);
                p = decimal.Round(cc, 2);
            }
            return p;
        }
         
        public static decimal getLike2(String body1, String body2)
        {

            String[] keys1 = Regex.Split(body1, "[^\\u0080-\\uFFFF\\w\\-\\d]+");
            String[] keys2 = Regex.Split(body2, "[^\\u0080-\\uFFFF\\w\\-\\d]+");

            decimal p = 0;
            if (keys1.Length > 0)
            {
                decimal cc = (decimal)((float)keys2.Length * 100 / keys1.Length);
                p = decimal.Round(cc, 2);
            }
            return p;
        }

        public static String findKeyByStr(String trueString, String falseString, String oldString)
        {
            try
            {   //以时间判断
                String key = "";

               String[] Keys = Regex.Split(oldString, "[^\\u0080-\\uFFFF\\w\\d]+");
               Array.Sort(Keys, new StringLengthComparer());
               foreach (String ckey in Keys) {
                    if (falseString.IndexOf(ckey) == -1 && trueString.IndexOf(ckey) >= 0) {
                        return ckey;
                    }
               }
                for (int length = 5; length >= 1; length--)
                {
                        for (int i = 0; i < trueString.Length - length; i++)
                        {
                            if (trueString.Length <= length && !trueString.Equals(falseString))
                            {
                                return trueString;
                            }
                            String tempKey = trueString.Substring(i, length);
                            if (falseString.IndexOf(tempKey) == -1&& oldString.IndexOf(key)>=0)
                            {
                                key = tempKey;
                                Regex regex = new Regex("[\\S]+");
                                //非制表符，返回结果，否则继续查看是否还有其他关键词
                                if (regex.IsMatch(key)) {
                                    return key;
                                }
                            }

                        }
                    
                }
                return key;

            }
            catch (Exception e)
            {

                Tools.SysLog("warin：查找注入关键字发生错误，" + e.Message);

            }
            return "";
        }
        public static int findKeyByCode(int trueCode, int falseCode)
        {
            if (trueCode != falseCode) {
                return trueCode;
            }
            return 0;

        }

        public static int findKeyByTime(int trueTime, int falseTime,int maxTime)
        {
            if (trueTime > maxTime&&falseTime<maxTime) {
                return maxTime;
            }
   
                return 0;
        }

        public static String clearURLParams(String url)
        {
                int index = url.IndexOf("?");
                if (index > 0)
                {
                    return url.Substring(0,index);

                }
                else {

                    return url;
                }
        }

        public static String getCurrentPath(String url)
        {
            int index =url.LastIndexOf("/");

            if (index != -1)
            {
                return url.Substring(0,index)+"/";
            }
            else {
                return "";
            }
        }

        public static String getRootDomain(String domain)
        {
            int index = domain.LastIndexOf(".");

            if (index>0)
            {
                int index2 = domain.LastIndexOf(".", index - 1);
                if (index2 != -1)
                {
                    return domain.Substring(index2+1);
                }
               
            }
            return domain;
        }

        public static String md5_16(String str){
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            String t2 = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(str)), 4, 8);
            t2 = t2.Replace("-", "");
            t2 = t2.ToLower();
            return t2;
        }
        public static String md5_32(String str)
        {
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(str));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            String pwd = "";
            for (int i = 0; i < s.Length; i++)
            {
                //将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;

        }
        public static bool isExistsNode(TreeNodeCollection tvws, String key)
        {

            foreach (TreeNode tn in tvws)
            {

                if (tn.Text.Equals(key))
                {
                    return true;
                }
            }

            return false;

        }


        public static String changeRequestMethod(String datapack)
        {
            if (datapack.StartsWith("GET"))
            {
                int pl = datapack.IndexOf("?");
                if (pl != -1)
                {
                    int el = datapack.IndexOf(" ", pl);
                    if (el != -1)
                    {

                        String cparams = datapack.Substring(pl + 1, el - pl - 1);
                        datapack = datapack.Replace("?" + cparams, "");
                        int sl = datapack.IndexOf("\r\n");
                        datapack = datapack.Insert(sl, "\r\nContent-Type: application/x-www-form-urlencoded\r\nContent-Length: 0");
                        int ssl = datapack.IndexOf("\r\n\r\n");
                        if (!datapack.EndsWith("\r\n\r\n"))
                        {

                            datapack += "\r\n\r\n";
                        }
                        datapack += cparams;

                        int me = datapack.IndexOf(" ");
                        if (me != -1)
                        {

                            datapack = "POST" + datapack.Substring(me, datapack.Length - me);
                        }

                        return datapack;
                    }
                }
            }
            else if (datapack.IndexOf("Transfer-Encoding: chunked") != -1) {
                return datapack;
            }
            else if (datapack.StartsWith("POST"))
            {
                int ssl = datapack.IndexOf("\r\n\r\n");

                if (ssl != -1)
                {


                    String cparams = datapack.Substring(ssl + 4, datapack.Length - ssl - 4);
                    datapack = datapack.Substring(0, ssl + 1);
                    int cys = datapack.IndexOf("Content-Type");
                    int cye = datapack.IndexOf("\r\n", cys);

                    if (cye > cys)
                    {
                        datapack = datapack.Remove(cys, cye - cys + 2);
                    }
                    int cls = datapack.IndexOf("Content-Length");
                    int cle = datapack.IndexOf("\r\n", cls + 1);
                    if (cle > cls)
                    {
                        datapack = datapack.Remove(cls, cle - cls + 2);
                    }

                    int hl = datapack.IndexOf(" HTTP");
                    if (hl != -1)
                    {

                        datapack = datapack.Insert(hl, "?" + cparams);
                    }

                    int me = datapack.IndexOf(" ");

                    if (me != -1)
                    {

                        datapack = "GET" + datapack.Substring(me, datapack.Length - me);
                    }
                }
            }
            
            return datapack;

        }

        public static String substr(String str,String startStr, String endStr) {
            try
            {
                if (!String.IsNullOrEmpty(str))
                {
                    if (String.IsNullOrEmpty(startStr) && String.IsNullOrEmpty(endStr)) {
                        return str;
                    }
                    int start = -1;
                    if (String.IsNullOrEmpty(startStr))
                    {
                        start = 0;
                    }
                    else {
                        start = str.IndexOf(startStr);
                    }
                    if (start != -1)
                    {
                        if (String.IsNullOrEmpty(endStr))
                        {
                            String token = str.Substring(start + startStr.Length);
                            return token;
                        }
                        else {
                            int end = str.IndexOf(endStr, start + startStr.Length);
                            if (end != -1)
                            {
                                String token = str.Substring(start + startStr.Length, end - start - startStr.Length);
                                return token;
                            }
                        }
                        
                    }

                }
            }
            catch (Exception e)
            {
                SysLog("截取字符发生异常！Tools.substr");
            }
            return "";

        }


        public static String getRequestURL(String header,bool isSSL,String ip,int port)
        {
            int start = header.IndexOf(' ');
            String url = "";
            if (start != -1) {
                int end = header.IndexOf(' ', start);
                if (end != -1) {
                    String str = header.Substring(start, end - start);
                    if (str.StartsWith("/"))
                    {
                        if (isSSL)
                        {
                            url = "https://" + ip + ":" + port + "/" + str;
                        }
                        else
                        {
                            url = "http://" + ip + ":" + port + "/" + str;
                        }
                    }
                    else {
                        url = str;
                    }
                }
            }
            return url;
        }

        public static DBType caseDBType(String dbname)
        {
                DBType db= DBType.UnKnow;
                Enum.TryParse(dbname, out db);
                return db;
        }

        public static String getRequestURI(String header)
        {
            int start = header.IndexOf(' ');
            String uri = "";
            if (start != -1)
            {
                int end = header.IndexOf(' ', start + 1);
                if (end != -1)
                {
                    String str = header.Substring(start + 1, end - start);
                    if (str.StartsWith("/"))
                    {
                        int send = str.IndexOf('?');
                        if (send != -1)
                        {
                            uri = str.Substring(0, send);
                        }
                        else
                        {
                            uri = str;
                        }

                    }
                    else
                    {
                        Uri u = new Uri(str);
                        uri = u.AbsolutePath;
                    }
                }
            }
            return uri;
        }

        //DB2的每列是数字或者字符的穷举
        private static String[] DB2_fillStr = { "1", "chr(32)"};


        private static String[] Infomix_fillStr = { "1", "''" };


       
        // 获得DB2的每列是数字或者字符的穷举
 
        public static List<String> getDB2UnionTemplates(int sumCount, int showIndex)
        {
            return getUnionTemplates(DB2_fillStr, sumCount, showIndex);
        }
        // 获得informix的每列是数字或者字符的穷举
        public static List<String> getInformixUnionTemplates(int sumCount, int showIndex)
        {
            return getUnionTemplates(Infomix_fillStr, sumCount, showIndex);
        }

        // 获得数据库的每列是数字或者字符的穷举
        private static List<String> getUnionTemplates(String[] fillStr,int sumCount, int showIndex)
        {
            List < String > list= new List<String>();
            if (sumCount == 1)
            {
                list.Add("{data}");
               
            }
            else {
                int n = sumCount - 1;
                String[] codes = new String[2 << (n - 1)];
                createGrayCode(fillStr,codes, n);
                foreach(String code in codes)
                {
                    String cp = insertShowTemplate(code, showIndex);
                    list.Add(cp);
                    //插入,显示列
                }
            }
            return list;
        }

        private static String insertShowTemplate(String temlate,int showIndex) {
            List<String> list = new List<String>(temlate.Split(','));
            list.Insert(showIndex,"{data}");
            return String.Join(",", list);

        }

        private static void createGrayCode(String[] fillStr, String[] codes, int n)
        {
            if (n == 1)
            {
                codes[0] = fillStr[0];
                codes[1] = fillStr[1];
            }
            else
            {
                createGrayCode(fillStr,codes, n - 1);
                int len = 2 << (n - 1);
                int half = len >> 1;
                for (int i = len - 1, j = 0; i >= 0; i--)
                {
                    if (i < half)
                    {
                        codes[i] = fillStr[0] + "," + codes[--j];
                    }
                    else
                    {
                        codes[i] = fillStr[1] + "," + codes[j++];
                    }
                }
            }
        }

        public static List<String> GetSQLiteColumns(String sql)
        {
            List<String> list = new List<String>(); 
            MatchCollection mc =Regex.Matches(sql, "\"(?<column>\\w+)\"[\\w ]+\\,");
            if (mc!=null&&mc.Count > 0) {
                foreach (Match m in mc) {
                    list.Add(m.Groups["column"].Value);
                }

            }
            return list;
        }

        public static int getAvg(List<int> nums) {
            int sum = 0;
            if (nums.Count > 0) {
                for (int j = 0; j < nums.Count; j++)
                {
                    sum += nums[j];
                }
                double c = sum / (double)nums.Count;
                int avg = (int)(Math.Ceiling(c));
                return avg;
            }
            return 0;
        }

        public static int getMaxSecondByMillisecond(int longMillisecond)
        {
            double c = longMillisecond / (double)1000;
            return (int)(Math.Ceiling(c));
        }


        public static String getUnionStartStrByBoolSleep(String payload)
        {
            String[] sstr ={"and","or"};
            int index = 0;
            foreach (String str in sstr) {
                index=payload.ToLower().IndexOf(str);
                if (index != -1) {
                    break;
                }
            }
            if (index != -1) {
                payload.Substring(0, index);
            }
            return "";
        }
    }


}
