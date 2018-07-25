using Microsoft.EntityFrameworkCore;

namespace TodoAPI.Models
{
    /// <summary>
    /// As a bridge bewteen Model and DB
    /// </summary>
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions options) : base(options)
        {
        }

        /// <summary>
        /// DB Object
        /// </summary>
        public DbSet<TodoItem> Items { get; private set; }

    }
}
