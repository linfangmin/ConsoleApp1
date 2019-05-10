using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            CancellationTokenSource cts = new CancellationTokenSource();
            ThreadPool.QueueUserWorkItem(o => Count(cts, 1000));
            Console.WriteLine("Press <Enter> to cancel the operation");
            Console.ReadLine();
            cts.Cancel();
            Console.ReadLine();
            */
            
            /*
            
            CancellationTokenSource cts = new CancellationTokenSource();
            Task<int> t = new Task<int>(() => Sum(cts.Token, 10000), cts.Token);
            t.Start();

            Thread.Sleep(100);

            Console.ReadLine();
            cts.Cancel();
            
            try
            {
                Console.WriteLine("the sum is:" + t.Result);
            }
            catch (AggregateException ex)
            {
                ex.Handle(e => e is OperationCanceledException);
                Console.WriteLine(ex.Message);
                Console.WriteLine("Sum was canceled");
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2.Message);
            }

            Console.ReadLine();
            */

            /*
            Task<int[]> parent = new Task<int[]>(() => { 
                var results = new int[3];

                new Task(() => results[0] = Sum(1000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[1] = Sum(2000), TaskCreationOptions.AttachedToParent).Start();
                new Task(() => results[2] = Sum(3000), TaskCreationOptions.AttachedToParent).Start();

                return results;
            });

            var cwt = parent.ContinueWith(parentTask => { Array.ForEach(parentTask.Result, Console.WriteLine); });
            parent.Start();

            Console.ReadLine();*/

            /*
            string assemblyName = Assembly.GetEntryAssembly().FullName;
            AppDomain.MonitoringIsEnabled = true;
            
            AppDomain ad1 = AppDomain.CreateDomain("#AD1");
            AppDomain ad2 = AppDomain.CreateDomain("#AD2");

            MarshalByRefType mbr1 = (MarshalByRefType)ad1.CreateInstanceAndUnwrap(assemblyName, typeof(MarshalByRefType).FullName);
            MarshalByRefType mbr2 = (MarshalByRefType)ad2.CreateInstanceAndUnwrap(assemblyName, typeof(MarshalByRefType).FullName);

            Console.WriteLine("ad1 GetCurrentTime:" + mbr1.GetCurrentTime());
            Console.WriteLine("ad2 GetCurrentTime:" + mbr2.GetCurrentTime());



            long ad1_survivedMemorySize = ad1.MonitoringSurvivedMemorySize;
            long ad1_allocateMemorySize = ad1.MonitoringTotalAllocatedMemorySize;
            TimeSpan ad1_timespam = ad1.MonitoringTotalProcessorTime;            

            Console.WriteLine("ad1_allocateMemorySize:" + ad1_allocateMemorySize);
            Console.WriteLine("ad1_survivedMemorySize:" + ad1_survivedMemorySize);
            Console.WriteLine("ad1_timespam:" + ad1_timespam.TotalMilliseconds);


            long ad2_survivedMemorySize = ad2.MonitoringSurvivedMemorySize;
            long ad2_allocateMemorySize = ad2.MonitoringTotalAllocatedMemorySize;
            TimeSpan ad2_timespam = ad2.MonitoringTotalProcessorTime;            

            Console.WriteLine("ad2_allocateMemorySize:" + ad2_allocateMemorySize);
            Console.WriteLine("ad2_survivedMemorySize:" + ad2_survivedMemorySize);
            Console.WriteLine("ad2_timespam:" + ad2_timespam.TotalMilliseconds);


            try
            {
                AppDomain.Unload(ad1);
                AppDomain.Unload(ad2);


                
            }
            catch { throw; }
            
            Console.ReadLine();            
            */

            //RunThread();

            SingletonTest();

            

        }

        public static void SingletonTest()
        {
            TaskFactory factory = new TaskFactory();
            List<Task> tasks = new List<Task>();
            for (int i = 0; i < 5; i++)
            {
                tasks.Add(factory.StartNew(() => {
                    Singleton s1 = Singleton.CreateInstance();
                }));
            }
            
            Console.ReadLine();
        }

        public static void RunThread()
        {
            ThreadPool.QueueUserWorkItem(s => CreateAppdomain(1));

            ThreadPool.QueueUserWorkItem(s => CreateAppdomain(10));

            Console.ReadLine();
            
        }

        public static void CreateAppdomain(int appdomainIndex)
        {
            string assemblyName = Assembly.GetEntryAssembly().FullName;
            AppDomain.MonitoringIsEnabled = true;

            AppDomain ad1 = AppDomain.CreateDomain("#AD" + appdomainIndex);
            
            MarshalByRefType mbr1 = (MarshalByRefType)ad1.CreateInstanceAndUnwrap(assemblyName, typeof(MarshalByRefType).FullName);
            
            Console.WriteLine(ad1.FriendlyName + " GetCurrentTime:" + mbr1.GetCurrentTime());

            long ad1_survivedMemorySize = ad1.MonitoringSurvivedMemorySize;
            long ad1_allocateMemorySize = ad1.MonitoringTotalAllocatedMemorySize;
            TimeSpan ad1_timespam = ad1.MonitoringTotalProcessorTime;

            Console.WriteLine(ad1.FriendlyName + " allocateMemorySize:" + ad1_allocateMemorySize);
            Console.WriteLine(ad1.FriendlyName + " survivedMemorySize:" + ad1_survivedMemorySize);
            Console.WriteLine(ad1.FriendlyName + " timespam:" + ad1_timespam.TotalMilliseconds);

            AppDomain.Unload(ad1);
            
        }
                
        public class MarshalByRefType : MarshalByRefObject {
            public MarshalByRefType()
            { }

            public string GetCurrentTime()
            {
                return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }


        public static void Count(CancellationTokenSource cts,int countTo) 
        {
            for (var i = 0; i < countTo; i++)
            {
                if (cts.IsCancellationRequested)
                {
                    Console.WriteLine("Count is Canceled");
                    break;
                }

                Console.WriteLine(i);
                Thread.Sleep(200);
            }

            Console.WriteLine("Count is Done");
        }

        public static int Sum(CancellationToken ct, int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                if(ct.IsCancellationRequested)
                {
                    Console.WriteLine("Sum is Canceled");
                    break;
                }
                //ct.ThrowIfCancellationRequested();

                Thread.Sleep(100);
                Console.WriteLine(n);
                checked { sum += n; }
            }
            return sum;
        }

        public static int Sum(int n)
        {
            int sum = 0;
            for (; n > 0; n--)
            {
                Console.WriteLine(n);
                checked { sum += n; }
            }
            return sum;
        }
    }
    

}
