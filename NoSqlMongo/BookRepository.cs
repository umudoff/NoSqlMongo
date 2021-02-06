using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlMongo
{
     class BookRepository
    {
        private static string connectionString;
        private IMongoDatabase database;
        private IMongoCollection<Book> collection;

       public BookRepository(string DBName, string targetCollection)
        {
            connectionString = ConfigurationManager.ConnectionStrings["MongoDb"].ConnectionString;
            var client = new MongoClient(connectionString);
            client.DropDatabase(DBName);
            database = client.GetDatabase(DBName);
            collection = database.GetCollection<Book>(targetCollection);
        }


        public void InsertBook(Book[] books)
        {
           collection.InsertMany(books);
        }
       
        public List<Book> FindBook(FilterDefinition<Book> filter)
        {
            var result = collection.Find(filter).ToList();
            return result;    
        }
  
        public List<Book> FindBook(FilterDefinition<Book> filter, SortDefinition<Book> sortCondition, int limit)
        {
            var result = collection.Find(filter).Sort(sortCondition).Limit(limit).ToList();
            return result;
        }

        public UpdateResult UpdateBooks(FilterDefinition<Book> filter, UpdateDefinition<Book> update)
        {
            var upd = collection.UpdateMany(filter, update);
            return upd;
        }

        public UpdateResult UpdateBooks(Expression<Func<Book, bool>> condition, UpdateDefinition<Book> update)
        {
            var upd = collection.UpdateMany(condition, update);
            return upd;
        }

        public DeleteResult DeleteBooks(FilterDefinition<Book> filter)
        {
            var del = collection.DeleteMany(filter);
            return del;
        }


    }
}
