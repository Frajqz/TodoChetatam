using System;
using System.Text.Json.Serialization;

namespace Todo.Core
{
    public class TodoItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; private set; } = Guid.NewGuid();

        [JsonPropertyName("title")]
        public string Title { get; private set; }

        [JsonPropertyName("isDone")]
        public bool IsDone { get; private set; }

        // Конструктор для сериализации
        [JsonConstructor]
        public TodoItem(Guid id, string title, bool isDone = false)
        {
            Id = id;
            Title = title?.Trim() ?? throw new ArgumentNullException(nameof(title));
            IsDone = isDone;
        }

        // Основной конструктор (для обратной совместимости)
        public TodoItem(string title) : this(Guid.NewGuid(), title, false)
        {
        }

        public void MarkDone() => IsDone = true;
        public void MarkUndone() => IsDone = false;

        public void Rename(string newTitle)
        {
            if (string.IsNullOrWhiteSpace(newTitle))
                throw new ArgumentException("Title required", nameof(newTitle));

            Title = newTitle.Trim();
        }
    }
}