using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        // Метод для сохранения в JSON файл
        public void Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var data = new TodoListData
            {
                Items = _items.ToList()
            };

            string json = JsonSerializer.Serialize(data, options);
            File.WriteAllText(path, json);
        }

        // Метод для загрузки из JSON файла
        public void Load(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException("Path cannot be null or empty", nameof(path));

            if (!File.Exists(path))
                throw new FileNotFoundException($"File not found: {path}");

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            string json = File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<TodoListData>(json, options);

            if (data?.Items != null)
            {
                _items.Clear();
                _items.AddRange(data.Items);
            }
        }

        // Вспомогательный класс для сериализации
        private class TodoListData
        {
            public List<TodoItem> Items { get; set; } = new();
        }
    }
}