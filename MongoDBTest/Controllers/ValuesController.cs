using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDBTest.Repository;
using MongoDBTest.Repository.Base;
using MongoDBTest.Setting;
using Newtonsoft.Json;

namespace MongoDBTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        public IBookRepository Repository { get; }
       

        public ValuesController(IBookRepository repository)
        {
            Repository = repository;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            //var book = Repository.Create(new Model.Book()
            //{
            //    Author = "abc",
            //    BookName = "BookName",
            //    Category = "Category1",
            //    BookId = new Random(DateTime.Now.Millisecond).Next(9999),
            //    Id = ObjectId.GenerateNewId().ToString()
            //});

            //return new List<string>() { JsonConvert.SerializeObject(book) };

            //List
            var books = Repository.GetBooks();
            return new List<string>() { JsonConvert.SerializeObject(books) };

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(string id)
        {
            var book = Repository.GetBook(id);
            return JsonConvert.SerializeObject(book);
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
