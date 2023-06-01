using NVOS.Core.Database;
using System;
using System.Collections.Generic;

namespace Test
{
    [Serializable]
    public class Test
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public Test(string name, int age)
        {
            Name = name;
            Age = age;
        }

        public void Sex()
        {
            Console.WriteLine("he did the se");
        }
    }
    public class Program
    {
        static void Main(string[] args)
        {
            LiteDbService dbService = new LiteDbService();
            string collectionId1 = "collection1";
            string collectionId2 = "collection2";

            Console.WriteLine("Connecting to database");
            dbService.Open("among.db");

            Console.WriteLine("Writing to database (string objects)");
            dbService.Write(collectionId1, "key1", "key1-val");
            dbService.Write(collectionId1, "key2", 25234542);

            Test test1 = new Test("mark", 21);
            Test test2 = new Test("cark", 12);
            Test test3 = new Test("bark", 69);

            Console.WriteLine("Writing to database (class objects)");
            dbService.Write(collectionId2, "key1", test1);
            dbService.Write(collectionId2, "key2", test2);
            dbService.Write(collectionId2, "key3", test3);

            Console.WriteLine("Reading from database (class objects)");
            Console.WriteLine(dbService.Read(collectionId2, "key1"));
            Console.WriteLine(dbService.Read(collectionId2, "key2"));
            Console.WriteLine(dbService.Read(collectionId2, "key3"));

            Console.WriteLine("Collection count: " + dbService.CountCollections());
            //Console.WriteLine("Collection1 count: " + dbService.CountRecords(collectionId1));
            Console.WriteLine("Collection2 count: " + dbService.CountRecords(collectionId2));

            IEnumerable<string> collectionList = dbService.ListCollections();
            Console.WriteLine("Listing collections:");
            foreach (string collection in collectionList)
            {
                Console.WriteLine(collection);
            }

            Console.WriteLine("Listing collection1 records:");
            IEnumerable<KeyValuePair<string, object>> records1 = dbService.ListRecords(collectionId1);
            foreach (KeyValuePair<string, object> record in records1)
            {
                Console.WriteLine(record.Value);
            }

            Console.WriteLine("Listing collection2 records:");
            IEnumerable<KeyValuePair<string, object>> records2 = dbService.ListRecords(collectionId2);
            foreach (KeyValuePair<string, object> record in records2)
            {
                Console.WriteLine(record.Value);
            }

            Console.WriteLine("Connection1 exists?: " + dbService.CollectionExists("collection1"));
            Console.WriteLine("Connection2 exists?: " + dbService.CollectionExists("collection2"));
            Console.WriteLine("Nonexistent collection exists?: " + dbService.CollectionExists("what"));

            Console.WriteLine("Getting collection1");
            Console.WriteLine(dbService.GetCollection("collection1"));
            Console.WriteLine(dbService.GetCollection("collection2"));

            Console.WriteLine("Getting test object");
            ((Test)dbService.Read(collectionId2, "key1")).Sex();

            Console.Read();

            Console.WriteLine("Modifying collection1 key1 value");
            dbService.Write(collectionId1, "key1", "booba");


        }
    }
}
