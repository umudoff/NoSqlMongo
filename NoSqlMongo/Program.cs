using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlMongo
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient("mongodb://localhost:27017");
            client.DropDatabase("BookLibrary");
            var database = client.GetDatabase("BookLibrary");

            string targetCollection = "Books";


            var collection = database.GetCollection<Book>(targetCollection);

            Book[] books = new Book[]{ new Book { Name = "Hobbit", Author = "Tolkien", Count = 5, Genre = new string[] { "fantasy" }, Year = 2014 },
             new Book { Name= "Lord of the rings", Author= "Tolkien", Count=3, Genre=new string[] { "fantasy" }, Year=2015 },
             new Book { Name= "Kolobok", Author="", Count=10, Genre=new string[] { "kids" }, Year=2000},
             new Book { Name="Repka",Author="", Count=11, Genre=new string[] { "kids"}, Year=2000},
             new Book { Name= "Dyadya Stiopa", Author= "Mihalkov", Count=1, Genre=new string[] {"kids"}, Year=2001}};

            collection.InsertMany(books);

            var ft1 = Builders<Book>.Filter.Gt("Count", 1);

            var res1 = collection.Find(ft1).Sort("{Name:1}").Limit(3).ToList();
            foreach (var r in res1)
            {
                //  Console.WriteLine(r.Name+" "+ r.Count);

            }

            // Console.WriteLine(res1.Sum(b=>b.Count));



            var maxResult = collection.Find(FilterDefinition<Book>.Empty).Sort("{Count:-1}").First();
            // Console.WriteLine("Max:" + maxResult.Count);
            var minResult = collection.Find(FilterDefinition<Book>.Empty).Sort("{Count:1}").First();
            //Console.WriteLine("Min: "+ minResult.Count);



            var distinctAuthors = collection.Find(FilterDefinition<Book>.Empty).ToList().Where(b => b.Author != "").GroupBy(b => b.Author).Where(grp => grp.Count() == 1).Select(grp => grp.Key);
            foreach (var r in distinctAuthors)
            {
                // Console.WriteLine(r);
            }


            var ft2 = Builders<Book>.Filter.Eq("Author", "");
            var res2 = collection.Find(ft2).ToList();
            foreach (var r in res2)
            {
                //   Console.WriteLine(r.Name + " " + r.Author);
            }


            Console.ReadLine();
        }
    }
}
