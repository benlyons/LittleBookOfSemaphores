using CypressTree.BasicSyncPatterns;
using NUnit.Framework;
using System;
using System.Threading;

namespace CypressTree.BasicSyncPatternsTest
{
    public class TestSignaling
    {
        [Test]
        public void Test()
        {
            var signaling = new Sec01_Signaling();

            Thread threadA = new Thread(() => signaling.ThreadA());
            Thread threadB = new Thread(() => signaling.ThreadB());

            threadA.Start();
            threadB.Start();

            threadA.Join();
            threadB.Join();

            CheckThreadAStatementRunsBeforeThreadBStatement(signaling);

            Console.WriteLine("Swapping threads...");

            threadA = new Thread(() => signaling.ThreadA());
            threadB = new Thread(() => signaling.ThreadB());

            threadB.Start();
            threadA.Start();

            threadA.Join();
            threadB.Join();
        }

        private static void CheckThreadAStatementRunsBeforeThreadBStatement(Sec01_Signaling signaling)
        {
            Assert.That(signaling.Events[0], Is.EqualTo(Sec01_Signaling.Event.ThreadA));
            Assert.That(signaling.Events[1], Is.EqualTo(Sec01_Signaling.Event.ThreadB));
        }
    }
}
