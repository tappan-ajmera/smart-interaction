using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xmlgrammar;
using Console = System.Console;
using MongoDB.Bson;
using MongoDB.Driver;


namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            CoreNLP cr = new CoreNLP();
            cr.Init();
            
            //Console.WriteLine("Done");
            var client = new MongoClient();
            var db = client.GetDatabase("grammar");
            var coll = db.GetCollection<Questions>("questions");
            //var id = new ObjectId("56ae186574b37d96dcad948f");
            var quests = coll.Find(new BsonDocument()).ToListAsync().Result;
            Console.WriteLine("Questions");
            foreach (var quest in quests)
            {
                cr.POSTagger(quest.q.ToString());
                //Console.WriteLine("*" + quest.q);
            }
            Console.WriteLine("Done executing");
        }

        public class Questions
        {
            public ObjectId Id{ get; set; }
            public int qno { get; set; }
            public string q { get; set; }
        }
    }
}
