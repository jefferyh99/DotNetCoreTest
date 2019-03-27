using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Produces("application/json")]//响应
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates a TodoItem.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "id": 1,
        ///        "name": "Item1",
        ///        "isComplete": true
        ///     }
        ///
        /// </remarks>
        /// <param name="item"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>       
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // PATCH: api/Todo/5 
        /// <summary>
        /// 部分更新
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchTodoItem(long id, string name)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Name = name;

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Todo/5
        [HttpGet("{id:long}")]
        //[HttpGet("{name:alpha}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
       
        public async Task<ActionResult<TodoItem>> GetTodoItem([FromRoute]long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        #region 不管参数，会路由到同一个方法，可以合并到一起，查询与获取



        // GET: api/Todo
        //[HttpGet("/Product")]//上升到根目錄
        //[HttpGet("/Product/{id}",Name = "Products_List")]
        //注意，很少会使用获取全部，通常都有参数，所以就拿最多参数的就可以
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            return await _context.TodoItems.ToListAsync();
        }

        //数组用逗号隔开
        //Query1,
        [HttpGet("NoNeed")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemsByQuery1([FromQuery]TodoItem item)
        {
            return await _context.TodoItems.Where(p => p.Name.Equals(item.Name)).ToListAsync();
        }

        //Query2
        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemsByQuery2([FromRoute]string name)
        {
            return await _context.TodoItems.Where(p => p.Name.Equals(name)).ToListAsync();
        }

        //Query3//最不好，避免使用，通常是TodoItem条件类型较多较复杂才使用
        [HttpPost("Search")]

        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemsByQuery3([FromBody]TodoItem item)
        {
            return await _context.TodoItems.Where(p => p.Name.Equals(item.Name)).ToListAsync();
        }

        #endregion
        // GET: api/Todo/Find/5
        [HttpGet("Find/{id}")]
        [ProducesResponseType(typeof(TodoItem), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetById(int id)
        {

            var todoItem = _context.TodoItems.Find(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(todoItem);
            }
        }
    }
}