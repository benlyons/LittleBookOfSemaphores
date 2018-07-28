using BasicSyncPatterns;
using NUnit.Framework;
using System.Threading;

namespace BasicSyncPatternsTest
{
    public class Test04_Mutex
    {
        [Test]
        public void Test_TwoThreadsIncrementCount_CountIsIncrementedCorrectly()
        {
            var test = new Sec04_Mutex();

            Thread threadA = new Thread(() => test.ThreadA());
            Thread threadB = new Thread(() => test.ThreadB());

            threadA.Start();
            threadB.Start();

            threadA.Join();
            threadB.Join();

            Assert.That(test.Count, Is.EqualTo(2));
        }
    }
}