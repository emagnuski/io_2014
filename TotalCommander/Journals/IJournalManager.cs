using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TotalCommander.Journals
{
    public interface IJournalManager
    {
        List<Journal> GetJournals();
        void RemoveJournal(int journalNumber);
    }

    public abstract class AbstractJournalManager : IJournalManager
    {
        protected List<Journal> journals;

        public AbstractJournalManager()
        {
            journals = new List<Journal>();
            JournalsLimit = 10;
            GenerationInterval = 3600000;
        }

        public int JournalsLimit
        {
            get;
            set;
        }

        public long GenerationInterval
        {
            get;
            set;
        }

        public List<Journal> GetJournals()
        {
            return new List<Journal>(journals);
        }

        public void RemoveJournal(int journalNumber)
        {
            journals.RemoveAt(journalNumber);
        }

        public abstract void GenerateJournal();

        public Thread Start()
        {
            Thread t = new Thread(ThreadStart);
            t.IsBackground = true;
            t.Start();
            return t;
        }

        protected void ThreadStart()
        {
            while(true)
            {
                GenerateJournal();
                if (journals.Count > JournalsLimit)
                {
                    journals.RemoveAt(0);
                }
                Thread.Sleep((int)GenerationInterval);
            }
        }
    }
}
