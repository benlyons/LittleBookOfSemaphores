using BasicSyncPatterns;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading;

namespace BasicSyncPatternsTest
{
    public class Test05_Multiplex
    {
        [Test]
        public void Test_LimitNumberOfThreadsInCriticalSection()
        {
            int maxThreadsInCriticalSection = 5;

            var test = new Sec05_Multiplex(maxThreadsInCriticalSection);

            IList<Thread> threads = new List<Thread>();

            int numberOfThreadsToRUn = 10;

            for (int i = 0; i < numberOfThreadsToRUn; i++)
            {
                var thread = new Thread(() => test.RunCode());

                threads.Add(thread);
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.That(
                test.PeakThreadsInCriticalSection, 
                Is.LessThanOrEqualTo(maxThreadsInCriticalSection),
                message: $"Peak threads in critical section was: {test.PeakThreadsInCriticalSection}");
        }
    }
}
