﻿namespace TodoApi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public long IdTodoList { get; set; }
    }
}
