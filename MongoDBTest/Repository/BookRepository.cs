
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDBTest.Model;
using MongoDBTest.Repository.Base;
using MongoDBTest.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBTest.Repository
{
    /// <summary>
    /// https://blog.oz-code.com/how-to-mongodb-in-c-part-1/
    /// </summary>
    public class BookRepository : IBookRepository
    {
        MongoClient _client;
        IMongoDatabase _db;
        public MongoDBSettings MongoDBSetting { get; }

        //.net core 与 .net framework API 有点区别
        public BookRepository(IOptions<MongoDBSettings> mongoDBSetting)
        {
            MongoDBSetting = mongoDBSetting.Value;
            _client = new MongoClient(MongoDBSetting.ConnectionString);
            _db = _client.GetDatabase(MongoDBSetting.Database);
        }

        public IEnumerable<Book> GetBooks()
        {
            //AsQueryable() 支持linq
            return _db.GetCollection<Book>("Books").AsQueryable().ToList();
        }
        public Book GetBook(string id)
        {
            var builder = Builders<Book>.Filter;
            var filter = builder.Eq(c => c.Id, id);
            var result = _db.GetCollection<Book>("Books").Find(filter).SortBy(p => p.Id).FirstOrDefault();

            var result2 = _db.GetCollection<Book>("Books").AsQueryable().Where(p => p.Id.Equals(id)).FirstOrDefault();
            return result2;
        }
        public Book Create(Book p)
        {
            _db.GetCollection<Book>("Books").InsertOne(p);
            return p;
        }
        public void Update(string id, Book p)
        {
            var builder = Builders<Book>.Filter;
            var res = builder.And(builder.Eq<string>(item => item.Id, id), builder.Eq<string>(item => item.Id, id));
            _db.GetCollection<Book>("Books").FindOneAndReplace(res, p);
        }
        public void Remove(string id)
        {
            var builder = Builders<Book>.Filter;
            var filter = builder.Eq(c => c.Id, id);
            _db.GetCollection<Book>("Books").DeleteOne(filter);
        }
    }
}
