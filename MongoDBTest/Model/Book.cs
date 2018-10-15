using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MongoDBTest.Model
{
    public class Book
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("BookId")]
        public int BookId { get; set; }
        [BsonElement("BookName")]
        public string BookName { get; set; }
        [BsonElement("Price")]
        public int Price { get; set; }
        [BsonElement("Category")]
        public string Category { get; set; }
        [BsonElement("Author")]
        public string Author { get; set; }
    }
}
