using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/todolists")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoListsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/todolists
        [HttpGet]
        public async Task<ActionResult<IList<TodoList>>> GetTodoLists()
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }

            return Ok(await _context.TodoList.ToListAsync());
        }

        // GET: api/todolists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoList>> GetTodoList(long id)
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }

            var todoList = await _context.TodoList.FindAsync(id);

            if (todoList == null)
            {
                return NotFound();
            }

            return Ok(todoList);
        }

        // GET: api/todolists/5/todoitems
        [HttpGet("{idTodoList}/todoitems")]
        public async Task<ActionResult<IList<TodoItem>>> GetTodoItems(long idTodolist)
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }

            var todolist = await _context.TodoList.FindAsync(idTodolist);

            if (todolist == null)
            {
                return NotFound();
            }

            return Ok(todolist.Items);
        }

        // PUT: api/todolists/5
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult> PutTodoList(long id, TodoList todoList)
        {
            if (id != todoList.Id)
            {
                return BadRequest();
            }

            _context.Entry(todoList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/todolists/5/todoitems/1
        [HttpPut("{idTodolist}/todoitems/{id}")]
        public async Task<ActionResult> PutTodoItem(long idTodolist, int id, TodoItem todoItem)
        {
            if (id != todoItem.Id)
            {
                return BadRequest();
            }

            var todolist = await _context.TodoList.FindAsync(idTodolist);

            if (todolist == null)
            {
                return NotFound();
            }

            todoItem = todolist.Items.Find(x => x.Id == id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.Entry(todoItem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/todolists
        // To protect from over-posting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoList>> PostTodoList(TodoList todoList)
        {
            if (_context.TodoList == null)
            {
                return Problem("Entity set 'TodoContext.TodoList'  is null.");
            }
            _context.TodoList.Add(todoList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, todoList);
        }

        // POST: api/todolists/5/todoitems
        [HttpPost("{idTodolist}/todoitems")]
        public async Task<ActionResult> PostTodoItem(long idTodolist, TodoItem todoItem)
        {
            if (_context.TodoList == null)
            {
                return Problem("Entity set 'TodoContext.TodoList'  is null.");
            }

            var todolist = await _context.TodoList.FindAsync(idTodolist);

            if (todolist == null)
            {
                return NotFound();
            }

            todolist.Items.Add(todoItem);
            _context.TodoList.Update(todolist);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTodoItems", new { id = todoItem.Id }, todoItem);
        }

        // DELETE: api/todolists/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTodoList(long id)
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }
            var todoList = await _context.TodoList.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/todolists/5/todoitems/1
        [HttpDelete("{idTodolist}/todoitems/{id}")]
        public async Task<ActionResult> DeleteTodoItem(long idTodolist, int id)
        {
            if (_context.TodoList == null)
            {
                return NotFound();
            }

            var todolist = await _context.TodoList.FindAsync(idTodolist);

            if (todolist == null)
            {
                return NotFound();
            }

            var todoitem = todolist.Items.FirstOrDefault(x => x.Id == id);

            if (todoitem == null)
            {
                return NotFound();
            }

            todolist.Items.Remove(todoitem);
            _context.TodoList.Update(todolist);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TodoListExists(long id)
        {
            return (_context.TodoList?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}