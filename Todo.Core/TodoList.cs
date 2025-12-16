using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Todo.Core
{
    public class TodoList
    {
        private readonly List<TodoItem> _items = new();
        public IReadOnlyList<TodoItem> Items => _items.AsReadOnly();
        public TodoItem Add(string title)
        {
            var item = new TodoItem(title);
            _items.Add(item);
            return item;
        }
        public bool Remove(Guid id) => _items.RemoveAll(i => i.Id == id) > 0;
        public IEnumerable<TodoItem> Find(string substring) =>
        _items.Where(i => i.Title.Contains(substring ?? string.Empty,
       StringComparison.OrdinalIgnoreCase));
        public int Count => _items.Count;



        public void Save(string path)
        {
            var json = JsonSerializer.Serialize(_items);
            File.WriteAllText(path, json);
        }

        public void AddItem(TodoItem item)
        {
            _items.Add(item);
        }

        public static TodoList Load(string path)
        {
            var json = File.ReadAllText(path);
            var items = JsonSerializer.Deserialize<List<TodoItem>>(json);
            var list = new TodoList();
            foreach (var item in items)
            {
                list.AddItem(item);
            }
            return list;
        }

    }
}
