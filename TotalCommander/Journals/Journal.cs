using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TotalCommander.Journals
{
    class Journal
    {
        private String id;
        private Dictionary<String, String> attributes;
        private DateTime creationDate;

        public static readonly String extension = ".jou";

        public Journal(String id, Dictionary<String, String> attributes)
        {
            this.id = id;
            this.attributes = attributes;
            this.creationDate = DateTime.Now;
        }

        public void Serialize(String path)
        {
            if (Directory.Exists(path))
            {
                path = Path.Combine(path, id + extension);
            }
            FileStream fs = File.OpenWrite(path);
            StreamWriter writer = new StreamWriter(fs);
            writer.WriteLine("Journal id " + id);
            writer.WriteLine(creationDate);
            writer.WriteLine();
            foreach (String key in attributes.Keys)
            {
                writer.WriteLine(key + ": " + attributes[key]);
            }
        }

        public String Id
        {
            get
            {
                return id;
            }
        }

        public Dictionary<String, String> Attributes
        {
            get
            {
                Dictionary<String, String> attrbs = new Dictionary<string,string>(attributes);
                return attrbs;
            }
        }

        public DateTime CreationDate
        {
            get
            {
                return creationDate;
            }
        }
    }
}
