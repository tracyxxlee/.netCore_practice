namespace TodoAPI
{
    public class TodoItem
    {
        public int ID { get; set; }
        public string Content { get; set; } = "";
        public bool IsCompleted { get; set; } = false;
    }
}
