using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander.Journals
{
    interface IJournalManager
    {
        public List<Journal> GetJournals();
        public void RemoveJournal(int journalNumber);
    }

    public class JournalManagerImpl : IJournalManager
    {
        private List<Journal> journals;

        public JournalManagerImpl()
        {
            journals = new List<Journal>();
            JournalsLimit = 10;
            GenerationInterval = 3600;
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
    }
}
