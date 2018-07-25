using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TodoAPI.Models;

namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    public class TodoController : Controller
    {
        private TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;
        }

        // GET api/get
        [HttpGet]
        [HttpGet("getAll")]
        public IEnumerable<TodoItem> Get()
        {
            return _context.Items.ToArray();
        }

        // GET api/todo/5
        [HttpGet("{id}", Name ="GetTodo")]
        public IActionResult GetById(int id)
        {
            var target = _context.Items.FirstOrDefault(item => item.ID == id);
            if(null == target)
            {
                // return coustmized message
                var result = new ContentResult()
                {
                    Content = $"No Data for '{id}'",
                    StatusCode = 200
                };
            }

            //there are 3 ways and they are the same
            //1. static method is my favorite
            return Ok(target);
            
            //2. create ObjectResult with 200 status code
            //return new OkObjectResult(target);

            //3. create ObjectResult but need set the status code and content
            //return new ObjectResult(myResult); 
        }

        // POST api/todo
        [HttpPost]
        public IActionResult Create([FromBody]TodoItem newItem)
        {
            if(null == newItem || newItem.Content.Length == 0)
            {
                return Ok("no data to be inserted");
            }

            _context.Items.Add(newItem); //add item to list
            _context.SaveChanges();      //confirm the change

            //there are 2 ways to produces a Status201Created response
            //1. call the ROUTE by name
            return CreatedAtRoute("GetTodo", new { newItem.ID }, newItem);

            //2. call the METHOD by name
            //return CreatedAtAction(nameof(GetById), new { newItem.ID }, newItem);            
        }

        // PUT api/todo/5
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]TodoItem value)
        {
            var target = _context.Items.FirstOrDefault(item => item.ID == id);
            if(null == target)
            {
                return new ObjectResult($"id:{id} is not exist");
            }

            target.Content = value.Content;
            target.IsCompleted = value.IsCompleted;
            _context.SaveChanges();

            //For HTTP PUT, standard response is HTTP 204(NoContent)
            return NoContent();
        }

        // DELETE api/todo/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var target = _context.Items.FirstOrDefault(opt => opt.ID == id);
            if(null == target)
            {
                return new ObjectResult($"id:{id} is not exist");
            }

            _context.Items.Remove(target);
            _context.SaveChanges();

            //For HTTP PUT, standard response is HTTP 204(NoContent)
            return NoContent();
        }
    }
}
