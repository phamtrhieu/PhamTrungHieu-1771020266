using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Todo
{
    internal class TodoRepository
    {
        private readonly List<Todo> _todos = new();
        private int _nextId;
        private readonly string _filePath = "todos.txt";
        public TodoRepository()
        {
            LoadFromFile();
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_filePath)) return;
            foreach (var line in File.ReadAllLines(_filePath))
            {
                var item = Todo.FromFileString(line);
                _todos.Add(item);
                if (item.Id >= _nextId)
                    _nextId = item.Id + 1;
            }
        }
        public void SaveChanges()
        {
            File.WriteAllLines(_filePath, _todos.Select(x => x.ToFileString()));
        }
        public List<Todo> GetAll() => _todos;
        public Todo Add(string title)
        {
            var todo = new Todo
            {
                Id = _nextId++,
                Title = title,
                IsSuccess = false,
            };
            _todos.Add(todo);
            SaveChanges();
            return todo;
        }
        public bool Delete(int id)
        {
            var todo = _todos.FirstOrDefault(x => x.Id == id);
            if (todo != null)
            {
                _todos.Remove(todo);
                SaveChanges();
                return true;
            }
            return false;
        }
        public bool Update(int id, string title)
        {
            var todo = _todos.FirstOrDefault(x => x.Id == id);
            if (todo != null)
            {
                todo.Title = title;
                SaveChanges();
                return true;
            }
            return false;
        }
        public bool Toggle(int id)
        {
            var todo = _todos.FirstOrDefault(x => x.Id == id);
            if (todo != null)
            {
                todo.IsSuccess = !todo.IsSuccess;
                SaveChanges();
                return true;
            }
            return false;
        }
    }
}