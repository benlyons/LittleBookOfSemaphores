using CypressTree.BasicSyncPatterns;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static CypressTree.BasicSyncPatterns.Sec07_ReusableBarrier;

namespace CypressTree.BasicSyncPatternsTest
{
    public class Test07_ReusableBarrier
    {
        [Test]
        public void TestSingleThreaded()
        {
            var test = new Sec07_ReusableBarrier(1);

            int loopTimes = 5;

            test.RunCode(loopTimes);

            for (int i = 0; i < loopTimes; i++)
            {
                Assert.That(test.StatementsExecuted[2 * i], Is.EqualTo(StatementExecuted.Rendezvous));
                Assert.That(test.StatementsExecuted[2 * i + 1], Is.EqualTo(StatementExecuted.CriticalPoint));
            }
        }

        [Test]
        public void TestMultiThreaded()
        {
            int loopTimes = 3;
            int threadCount = 20;

            var test = new Sec07_ReusableBarrier(threadCount);

            var threads = new List<Thread>();

            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(() => test.RunCode(loopTimes));
                threads.Add(thread);
                thread.Start();
            }

            var expectedStatementsExecuted =
                Enumerable.Repeat(StatementExecuted.Rendezvous, threadCount).Concat(
                Enumerable.Repeat(StatementExecuted.CriticalPoint, threadCount)).Concat(
                Enumerable.Repeat(StatementExecuted.Rendezvous, threadCount)).Concat(
                Enumerable.Repeat(StatementExecuted.CriticalPoint, threadCount)).Concat(
                Enumerable.Repeat(StatementExecuted.Rendezvous, threadCount)).Concat(
                Enumerable.Repeat(StatementExecuted.CriticalPoint, threadCount));

            threads.ForEach(t => t.Join());

            CollectionAssert.AreEqual(expectedStatementsExecuted, test.StatementsExecuted);
        }
    }
}