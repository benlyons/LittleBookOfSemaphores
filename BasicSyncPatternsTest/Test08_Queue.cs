using BasicSyncPatterns;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DancerType = BasicSyncPatterns.Sec08_Queue.DancerType;

namespace BasicSyncPatternsTest
{
    public class Test08_Queue
    {
        public enum Add
        {
            Leader,
            Follower
        }

        private Sec08_Queue classUnderTest;

        private IList<Thread> threads;

        [SetUp]
        public void SetUp()
        {
            this.classUnderTest = new Sec08_Queue();
            this.threads = new List<Thread>();
        }

        private static IEnumerable<TestCaseData> Tests
        {
            get
            {
                yield return new TestCaseData(new[] { Add.Follower }, 0);
                yield return new TestCaseData(new[] { Add.Follower }, 0);
            }
        }

        [TestCaseSource("Tests")]
        public void WhenLeadersAndFollowerAdded_WhoShouldDance(IEnumerable<Add> dancersToAdd, int howManyPairs)
        {
            foreach (var dancerToAdd in dancersToAdd)
            {
                if (dancerToAdd == Add.Leader)
                {
                    this.CreateAndStartLeaderThread();
                }
                else
                {
                    this.CreateAndStartFollowerThread();
                }

                Assert.That(this.classUnderTest.Dancers.Count, Is.EqualTo(howManyPairs * 2));

                for (int i = 0; i < howManyPairs; i++)
                {
                    var pair = this.classUnderTest.Dancers.Skip(2 * i).Take(2);

                    Assert.That(pair, Is.EquivalentTo(new[] { DancerType.Leader, DancerType.Follower }));
                }
            }
        }

        [Test]
        public void When1Leader0Followers_NoOneShouldDance()
        {
            this.CreateAndStartLeaderThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.Empty);
        }

        [Test]
        public void When1Leader1Follower_BothShouldDance()
        {
            this.CreateAndStartLeaderThread();
            this.CreateAndStartFollowerThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.EquivalentTo(new[] { DancerType.Follower, DancerType.Leader }));
        }

        [Test]
        public void When0Leaders1Followers_NoOneShouldDance()
        {
            this.CreateAndStartFollowerThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.Empty);
        }

        [Test]
        public void When1FollowerThen1Leader_BothShouldDance()
        {
            this.CreateAndStartFollowerThread();
            this.CreateAndStartLeaderThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.EquivalentTo(new[] { DancerType.Follower, DancerType.Leader }));
        }

        [Test]
        public void When2Leaders1Follower_Only1Leader1FollowerShouldDance()
        {
            this.CreateAndStartLeaderThread();
            this.CreateAndStartLeaderThread();
            this.CreateAndStartFollowerThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.EquivalentTo(new[] { DancerType.Follower, DancerType.Leader }));
        }

        [Test]
        public void When1FollowerThen2Leaders_Only1Leader1FollowerShouldDance()
        {
            this.CreateAndStartFollowerThread();

            this.CreateAndStartLeaderThread();
            this.CreateAndStartLeaderThread();

            JoinThreads();

            Assert.That(classUnderTest.Dancers, Is.EquivalentTo(new[] { DancerType.Follower, DancerType.Leader }));
        }

        private void CreateAndStartLeaderThread()
        {
            var leaderThread = new Thread(() => this.classUnderTest.AddLeader());
            leaderThread.Start();

            this.threads.Add(leaderThread);
        }

        private void CreateAndStartFollowerThread()
        {
            var followerThread = new Thread(() => this.classUnderTest.AddFollower());
            followerThread.Start();

            this.threads.Add(followerThread);
        }

        private void JoinThreads()
        {
            foreach (var thread in this.threads)
            {
                thread.Join(1000);
            }
        }
    }
}