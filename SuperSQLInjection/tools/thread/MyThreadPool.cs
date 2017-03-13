using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using tools;
using System.Collections;

namespace SuperSQLInjection.tools
{
    class MyThreadPool
    {
        public static int maxThread = 1;
        public static String tname = "ThreadPool-";
        public static ArrayList threads = new ArrayList();
        public static Thread cth = null;
        public static AutoResetEvent _autoResetEvent = new AutoResetEvent(true);
        public static void setMaxThread(int maxTh)
        {
            maxThread = maxTh;
            cth = new Thread(clearThread);
            cth.Start();

        }
        public static int getCurrentThreadCount()
        {
            return threads.Count;

        }

        public static void clearThread()
        {
            while (true)
            {
                lock (threads.SyncRoot)
                {
                    for (int i = 0; i < threads.Count; i++)
                    {

                        if (threads.Count <= 0)
                        {

                            break;
                        }
                        Thread cth = (Thread)threads[i];
                        if (cth.IsAlive == false)
                        {
                            threads.Remove(cth);
                            _autoResetEvent.Set();
                        }
                    }
                }
                Thread.Sleep(10);
            }
        }

        public static void killAllThread()
        {

            lock (threads.SyncRoot)
            {
                for (int i = 0; i < threads.Count; i++)
                {

                    if (threads.Count <= 0)
                    {

                        break;
                    }

                    Thread cth = (Thread)threads[i];
                    cth.Abort();
                }
            }
        }

        public static void initThread(ParameterizedThreadStart ps, Object obj, String name)
        {
            while (Main.status == 1)
            {
                if (threads.Count < maxThread && Main.status == 1)
                {
                    Thread th = new Thread(ps);
                    th.Name = tname ;
                    th.IsBackground = true;
                    lock (threads.SyncRoot)
                    {
                        threads.Add(th);
                    }

                    th.Start(obj);
                    break;
                }
                _autoResetEvent.WaitOne();
            }
        }

        public static void initThread(ParameterizedThreadStart ps, Object obj)
        {
            while (Main.status == 1)
            {
                if (threads.Count < maxThread && Main.status == 1)
                {
                    Thread th = new Thread(ps);
                    th.IsBackground = true;
                    th.Name = tname;
                    lock (threads.SyncRoot)
                    {
                        threads.Add(th);
                    }
                    th.Start(obj);
                    break;
                }
                Thread.Sleep(10);
            }
        }

        public static int GetAliveThreadsCount()
        {

            /*
                foreach (Thread th in threads)
                {
                    if (th.IsAlive)
                    {
                        count++;

                    }
                }*/
            return threads.Count;
        }

    }

}