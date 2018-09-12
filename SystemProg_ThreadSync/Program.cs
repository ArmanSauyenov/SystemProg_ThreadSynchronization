using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSynchronization
{
    // Thread Synchronization
    class DatabaseConnection
    {
        private static object _locker = new object();
        private static Semaphore _semaphore = new Semaphore(2, 2);
        public void WriteTransactionToDb()
        {
            //lock (_locker)
            //{
            _semaphore.WaitOne();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                Console.WriteLine("Opened SQL connection in thread # " +
                    Thread.CurrentThread.ManagedThreadId);

                Thread.Sleep(1000);


            }
            _semaphore.Release();
            Console.WriteLine("Closed SQL connection in thread # " +
                    Thread.CurrentThread.ManagedThreadId);
            //}

        }
    }
    class Data
    {
        public string[] Credentials;
        public Data()
        {
            Credentials = new string[1000];
            for (int i = 0; i < 1000; i++)
            {
                Credentials[i] = Guid
                    .NewGuid()
                    .ToString();
            }
        }
    }
    class Program
    {
        public static void OperationOne(Data data)
        {
            for (int i = 0; i < 1000; i++)
            {
                data.Credentials[i] =
                    data.Credentials[i].ToLower();
            }
        }
        public static void OperationTwo(Data data)
        {
            for (int i = 0; i < 1000; i++)
            {
                data.Credentials[i] =
                    new string(data.Credentials[i].Take(10).ToArray());
            }
        }

        public static void Print(Data data)
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(data.Credentials[i]);
            }
        }
        static void Main(string[] args)
        {
            /*
            Data data = new Data();

            Thread threadOne = new Thread(() => OperationOne(data));
            Thread threadTwo = new Thread(() => OperationTwo(data));
            Thread threadThree = new Thread(() => Print(data));

            threadOne.Start();
            threadOne.Join();

            threadTwo.Start();
            threadTwo.Join();

            threadThree.Start();
            threadThree.Join(); */

            /*
            DatabaseConnection connection = new DatabaseConnection();

            Thread[] threads = new Thread[28];
            for(int i =0; i < 28; i++)
            {
                threads[i] = new Thread(() => connection.WriteTransactionToDb());
            }

            foreach (var item in threads)
            {
                item.Start();
            }
            */


            bool existed = false;
            string[] appKey = new string[] { "ONE", "TWO", "THREE" };
            Mutex mutexObj = null;

            for (int i = 0; i < appKey.Length; i++)
            {
                mutexObj = new Mutex(true, appKey[i], out existed);
                if (existed)
                {
                    Console.WriteLine("App opened!");
                    break;
                }
            }

            if (existed == false)
            {
                Console.WriteLine("Already exists 3-more copies in the same time");
            }

            Console.ReadLine();
            if (mutexObj != null)
            {
                mutexObj.ReleaseMutex();
            }

        }
    }
}
