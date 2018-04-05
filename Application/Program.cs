using System;

namespace LogUsers
{
    using System.Threading;

    using LogTest;

    class Program
    {
        static void Main(string[] args)
        {
            ILog  logger = new AsyncLog(new FileWriter());

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i);
            }
            Thread.Sleep(5);
            logger.StopWithFlush();

            ILog logger2 = new AsyncLog(new FileWriter());

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i);
            }
            Thread.Sleep(5);
            logger2.StopWithoutFlush();

            Console.ReadLine();
        }
    }
}
