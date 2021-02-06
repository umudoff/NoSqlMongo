using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NoSqlMongo
{
    class UIService
    {
        private BookRepository _repository;
       
        public  UIService(BookRepository repository)
        {
            _repository = repository;
        }


        public void  InitialiseLibrary()
        {
             Book[] books = new Book[]{ new Book { Name = "Hobbit", Author = "Tolkien", Count = 5, Genre = new string[] { "fantasy" }, Year = 2014 },
             new Book { Name= "Lord of the rings", Author= "Tolkien", Count=3, Genre=new string[] { "fantasy" }, Year=2015 },
             new Book { Name= "Kolobok", Author="", Count=10, Genre=new string[] { "kids" }, Year=2000},
             new Book { Name="Repka",Author="", Count=11, Genre=new string[] { "kids"}, Year=2000},
             new Book { Name= "Dyadya Stiopa", Author= "Mihalkov", Count=1, Genre=new string[] {"kids"}, Year=2001}};
           
            _repository.InsertBook(books);
        }

        public List<Book> GetBooksGtOne()
        {
            var ft1 = Builders<Book>.Filter.Gt("Count", 1);
            var sortDef = Builders<Book>.Sort.Ascending(b => b.Name);
            var bookList= _repository.FindBook(ft1,sortDef,3);
        
            return bookList;
        }

        public int CountBooksGtOne(List<Book> books)
        {
            return books.Sum(b => b.Count);
        }

        public Book GetBookMaxCount()
        {
            var ft1 = FilterDefinition<Book>.Empty;
            var sortDef= Builders<Book>.Sort.Descending(b => b.Count);
            List<Book> books = _repository.FindBook(ft1, sortDef, 1);
            Book maxResult =books.First();
            return maxResult;
        }

        public Book GetBookMinCount()
        {
            var ft1 = FilterDefinition<Book>.Empty;
            var sortDef = Builders<Book>.Sort.Ascending(b => b.Count);
            List<Book> books = _repository.FindBook(ft1, sortDef, 1);
            Book minResult = books.First();
            return minResult;
        }

        public List<string> GetDistinctAuthors()
        {
            var ft1 = FilterDefinition<Book>.Empty;
            List<Book> books= _repository.FindBook(ft1);
            var distinctAuthors = books.Where(b => b.Author != "").GroupBy(b => b.Author).Where(grp => grp.Count() == 1).Select(grp => grp.Key).ToList();  
            return distinctAuthors;
        }

        public List<Book> GetBookEmptyAuthor()
        {
            var ft1 = Builders<Book>.Filter.Eq("Author", "");
            List<Book> result = _repository.FindBook(ft1);
            return result;
        }

        public List<Book> IncrementBookCount()
        {
            var ft1 = FilterDefinition<Book>.Empty;
            UpdateDefinition<Book> update = "{$inc: { Count: 1} }";
            var res= _repository.UpdateBooks(ft1, update);
            return _repository.FindBook(ft1);
        }

        public List<Book> AddFavoriteGenre()
        {
            Expression<Func<Book, bool>> ft = b => b.Genre.Contains("fantasy");
            UpdateDefinition<Book> update = "{$addToSet: {Genre: \"favority\"} }";
            var res= _repository.UpdateBooks(ft, update);
            return _repository.FindBook(FilterDefinition<Book>.Empty);
        }

        public List<Book> DeleteLtCount()
        {
            var ft = Builders<Book>.Filter.Lt("Count", 3);
            var res= _repository.DeleteBooks(ft);
            return _repository.FindBook(FilterDefinition<Book>.Empty);
        }

        public List<Book> DeleteAll()
        {
            var ft = FilterDefinition<Book>.Empty;
            var res = _repository.DeleteBooks(ft);
            return _repository.FindBook(ft);
        }

    }
}
