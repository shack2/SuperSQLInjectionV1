using Amib.Threading.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using tools;

namespace SuperSQLInjection.tools.http
{
    class SocketProxy
    {

        private static byte[] s5_hello = new Byte[] { 5, 1, 0 };
        public int CreatProxyUseTime = 0;
        public int ConectProxyUseTime = 0;
        private Stopwatch sw = new Stopwatch();
        public TcpClient creatProxySocket(string host, int port,int timeout) {
            try
            {
                TimeOutSocket ts = new TimeOutSocket();
                TcpClient cilent=ts.Connect(host, port, timeout);
                TcpClient client = new TcpClient();
                client.Connect(host, port);
                CreatProxyUseTime = ts.useTime;
                if (client.Connected)
                {
                    return client;
                }
                else {
                    client.Close();
                }
               
            }
            catch (Exception e) {
                Tools.SysLog("creatProxySocket异常:" + e.Message);
            }
            return null;
        }

        public byte[] GetLoginByte(String username,String password) {
           
            byte[] bUser = Encoding.Default.GetBytes(username);
            byte[] bPass = Encoding.Default.GetBytes(password);

            int len = 3 + bUser.Length + bPass.Length;

            byte[]  data = new Byte[len];
            data[0] = 5;
            data[1] = (byte)bUser.Length;
            Array.Copy(bUser, 0, data, 2, bUser.Length);
            data[2 + bUser.Length] = (byte)bPass.Length;
            Array.Copy(bPass, 0, data, 3 + bUser.Length, bPass.Length);
            return data;
        }

        public byte[] GetConectTargetByte(String hsot, int port)
        {
            byte[] data = new byte[10];
            data[0] = 5;
            data[1] = 1;
            data[2] = 0;
            data[3] = 1;

            IPAddress ipAdd = Dns.GetHostAddresses(hsot)[0];
            string strIp = ipAdd.ToString();
            string[] strAryTemp = strIp.Split(new char[] { '.' });
            data[4] = Convert.ToByte(strAryTemp[0]);
            data[5] = Convert.ToByte(strAryTemp[1]);
            data[6] = Convert.ToByte(strAryTemp[2]);
            data[7] = Convert.ToByte(strAryTemp[3]);

            data[8] = (byte)(port / 256);
            data[9] = (byte)(port % 256);
            return data;
        }

        /// <summary>
        /// 测试连接代理服务器
        /// </summary>
        /// <param name="strRemoteHost"></param>
        /// <param name="iRemotePort"></param>
        /// <param name="sProxyServer"></param>
        /// <returns></returns>
        public bool ConnectProxyServer(string host, int port, TcpClient sProxyServer,String username,String password,int timeout)
        {
            try
            {
                sw.Start();
                //构造Socks5代理服务器第一连接头(无用户名密码)
                byte[] bySock5Receive = new byte[10];
                int readCount = 0;
                sProxyServer.ReceiveTimeout = (timeout * 1000) - CreatProxyUseTime;
                if (String.IsNullOrEmpty(username) && String.IsNullOrEmpty(password))
                {
                    sProxyServer.Client.Send(s5_hello, s5_hello.Length, SocketFlags.None);
                }
                else
                {
                    byte[] login = GetLoginByte(username, password);
                    sProxyServer.Client.Send(login, login.Length, SocketFlags.None);
                }
                readCount = sProxyServer.Client.Receive(bySock5Receive, bySock5Receive.Length, SocketFlags.None);
                if (readCount < 2)
                {
                    sProxyServer.Close();
                    throw new Exception("不能获得代理服务器正确响应。");
                }

                else if (bySock5Receive[0] != 5 || (bySock5Receive[1] != 0 && bySock5Receive[1] != 2))
                {
                    sProxyServer.Close();
                    throw new Exception("代理服务其返回的响应错误。");
                }
                else
                {
                    //用户验证  
                    if (bySock5Receive[1] == 2)
                    {
                        if (String.IsNullOrEmpty(username) && String.IsNullOrEmpty(password))
                        {
                            throw new Exception("代理服务器需要进行身份确认,您未设置代理账号和密码。");
                        }
                    }

                    if (bySock5Receive[1] == 0)
                    {
                        byte[] data = GetConectTargetByte(host, port);
                        sProxyServer.Client.Send(data, data.Length, SocketFlags.None);
                        byte[] readData = new byte[100];
                        readCount = sProxyServer.Client.Receive(readData, readData.Length, SocketFlags.None);

                        if (readCount >= 2 && bySock5Receive[0] == 5 && bySock5Receive[1] == 0)
                        {
                            return true;
                        }
                        else
                        {
                            sProxyServer.Close();
                            //利用Socks5代理连接目标出错。
                            return false;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                sProxyServer.Close();
                Tools.SysLog("Socks5代理发生异常！" + e.Message);
            }
            finally {
                sw.Stop();
                ConectProxyUseTime = (int)sw.ElapsedMilliseconds;
                sw.Reset();
            }
            return false;
        }
    }
}
