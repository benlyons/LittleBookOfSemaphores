using BasicSyncPatterns;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BasicSyncPatterns.Sec07_ReusableBarrier;

namespace BasicSyncPatternsTest
{
    public class Test07_ReusableBarrier
    {
        [Test]
        public void TestSingleThreaded()
        {
            var test = new Sec07_ReusableBarrier();

            int loopTimes = 5;

            test.RunCode(loopTimes);

            for (int i = 0; i < loopTimes; i++)
            {
                Assert.That(Sec07_ReusableBarrier.StatementsExecuted[2 * i], Is.EqualTo(StatementExecuted.Rendezvous));
                Assert.That(Sec07_ReusableBarrier.StatementsExecuted[2 * i + 1], Is.EqualTo(StatementExecuted.CriticalPoint));
            }
        }

        [Test]
        public void TestMultiThreaded()
        {
            var test = new Sec07_ReusableBarrier();

            int loopTimes = 2;
            int threads = 2;

            for (int i = 0; i < threads; i++)
            {
                var thread = new Thread(() => test.RunCode(loopTimes));
                thread.Start();
            }

            IEnumerable<StatementExecuted> expectedStatementsExecuted = new StatementExecuted[]
            {
                StatementExecuted.Rendezvous,
                StatementExecuted.Rendezvous,
                StatementExecuted.CriticalPoint,
                StatementExecuted.CriticalPoint,
                StatementExecuted.Rendezvous,
                StatementExecuted.Rendezvous,
                StatementExecuted.CriticalPoint,
                StatementExecuted.CriticalPoint,
            };

            CollectionAssert.AreEqual(expectedStatementsExecuted, Sec07_ReusableBarrier.StatementsExecuted);
        }
    }
}