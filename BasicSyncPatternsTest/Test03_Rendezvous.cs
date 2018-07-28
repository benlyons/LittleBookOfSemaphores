using BasicSyncPatterns;
using NUnit.Framework;
using System.Threading;
using static BasicSyncPatterns.Sec03_Rendezvous;

namespace BasicSyncPatternsTest
{
    public class Test03_Rendezvous
    {
        [Test]
        public void Test_Statement_Order()
        {
            var test = new Sec03_Rendezvous();

            Thread threadA = new Thread(() => test.ThreadA());
            Thread threadB = new Thread(() => test.ThreadB());

            threadA.Start();
            threadB.Start();

            threadA.Join();
            threadB.Join();

            Assert.That(
                test.StatementsExecuted.IndexOf(StatementExecuted.A1),
                Is.LessThan(test.StatementsExecuted.IndexOf(StatementExecuted.B2)));

            Assert.That(
                test.StatementsExecuted.IndexOf(StatementExecuted.B1),
                Is.LessThan(test.StatementsExecuted.IndexOf(StatementExecuted.A2)));
        }
    }
}
