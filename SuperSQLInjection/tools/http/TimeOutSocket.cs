using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace SuperSQLInjection.tools.http
{
    class TimeOutSocket
    {
        private  bool IsConnectionSuccessful = false;
        private  Exception socketexception =null;
        private  ManualResetEvent TimeoutObject = new ManualResetEvent(false);
        public int useTime = 0;
        public  TcpClient Connect(String host,int port,int timeoutMSec)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            TimeoutObject.Reset();
            socketexception = null;

            TcpClient tcpclient = new TcpClient();

            tcpclient.BeginConnect(host, port,new AsyncCallback(CallBackMethod), tcpclient);


            if (TimeoutObject.WaitOne(timeoutMSec, false))
            {
                if (IsConnectionSuccessful)
                {
                    sw.Stop();
                    useTime = (int)sw.ElapsedMilliseconds;
                    return tcpclient;
                }
                else
                {
                    throw socketexception;
                }
            }
            else
            {
                tcpclient.Close();
                throw new TimeoutException("TimeOut Exception");
            }
        }
        private void CallBackMethod(IAsyncResult asyncresult)
        {
            try
            {
                IsConnectionSuccessful = false;
                TcpClient tcpclient = asyncresult.AsyncState as TcpClient;

                if (tcpclient.Client != null)
                {
                    tcpclient.EndConnect(asyncresult);
                    IsConnectionSuccessful = true;
                }
            }
            catch (Exception ex)
            {
                IsConnectionSuccessful = false;
                socketexception = ex;
            }
            finally
            {
                TimeoutObject.Set();
            }
        }
    }
}
