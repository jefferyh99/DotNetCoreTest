using MongoDBTest.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBTest.Repository.Base
{
    public interface IBookRepository
    {
         IEnumerable<Book> GetBooks();

         Book GetBook(string id);

         Book Create(Book p);

         void Update(string id, Book p);

         void Remove(string id);
    }
}
