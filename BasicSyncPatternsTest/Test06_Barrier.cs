using CypressTree.BasicSyncPatterns;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static CypressTree.BasicSyncPatterns.Sec06_Barrier;

namespace CypressTree.BasicSyncPatternsTest
{
    public class Test06_Barrier
    {
        [Test]
        public void TestSingleThreaded()
        {
            var test = new Sec06_Barrier(1);

            test.RunCode();

            Assert.That(test.StatementsExecuted[0], Is.EqualTo(StatementExecuted.Rendezvous));
            Assert.That(test.StatementsExecuted[1], Is.EqualTo(StatementExecuted.CriticalPoint));
        }

        [Test]
        public void TestMultiThreaded()
        {
            int threadCount = 25;

            var test = new Sec06_Barrier(threadCount);

            var threads = new List<Thread>();

            for (int i = 0; i < threadCount; i++)
            {
                Thread thread = new Thread(() => test.RunCode());
                threads.Add(thread);
                thread.Start();
            }

            threads.ForEach(t => t.Join());

            var expectedStatementsExecuted =
                Enumerable.Repeat(StatementExecuted.Rendezvous, threadCount).Concat(
                Enumerable.Repeat(StatementExecuted.CriticalPoint, threadCount));

            CollectionAssert.AreEqual(expectedStatementsExecuted, test.StatementsExecuted);
        }
    }
}