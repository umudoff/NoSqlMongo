using MongoDB.Bson;
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
            BookRepository repo = new BookRepository("BookLibrary", "Books");
            UIService service = new UIService(repo);
            service.InitialiseLibrary();
            List<Book> books;
            books = service.GetBooksGtOne();
            PrintBook(books);
            Console.WriteLine(service.CountBooksGtOne(books));
            var maxBook = service.GetBookMaxCount();
            PrintBook(maxBook);
            var minBook = service.GetBookMinCount();
            PrintBook(minBook);
            service.GetDistinctAuthors();
            service.GetBookEmptyAuthor();
            service.IncrementBookCount();
            service.AddFavoriteGenre();
            var result = service.DeleteLtCount();
            PrintBook(result);

            Console.ReadLine();
        }

        public static void PrintBook(Book book)
        {
            Console.Write(book.Author + " " + book.Count + " " + book.Name + " ");
            foreach (var t in book.Genre)
            {
                Console.Write(t + " ");
            }
            Console.WriteLine();
        }

        public static void PrintBook(List<Book> books)
        {
            foreach (var r in books)
            {
                Console.Write(r.Name + " " + r.Count + " " + r.Author + " ");
                foreach (var t in r.Genre)
                {
                    Console.Write(t + " ");
                }
                Console.WriteLine();
            }
        }
    }
}
