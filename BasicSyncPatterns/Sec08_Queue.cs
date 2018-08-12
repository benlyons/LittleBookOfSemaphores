using System;
using System.Collections.Generic;
using System.Threading;

namespace BasicSyncPatterns
{
    public class Sec08_Queue
    {
        public enum DancerType
        {
            Leader,
            Follower
        }

        private int leaders = 0;
        private int followers = 0;
        private readonly Semaphore barrier;

        public Sec08_Queue()
        {
            this.barrier = new Semaphore(0, 1);
        }

        public IList<DancerType> Dancers { get; } = new List<DancerType>();

        public void AddLeader()
        {
            this.leaders = 1;

            if (this.followers == 1)
            {
                this.barrier.Release();
            }
            else
            {
                this.barrier.WaitOne();
            }

            this.Dancers.Add(DancerType.Leader);
        }

        public void AddFollower()
        {
            this.followers = 1;

            if (this.leaders == 1)
            {
                this.barrier.Release();
            }
            else
            {
                this.barrier.WaitOne();
            }

            this.followers = 0;

            this.Dancers.Add(DancerType.Follower);
        }
    }
}
