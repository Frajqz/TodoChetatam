using System.IO;
using Xunit;
using Todo.Core;

namespace Todo.Core.Tests
{
    public class TodoListJsonTests
    {
        [Fact]
        public void Save_And_Load_Work()
        {
            var list = new TodoList();
            list.Add("Task 1");
            list.Add("Task 2");
            list.Items.First().MarkDone();

            string path = "test.json";
            list.Save(path);

            var loadedList = TodoList.Load(path);
            Assert.Equal(2, loadedList.Count);
            Assert.Equal("Task 1", loadedList.Items.First().Title);
            Assert.True(loadedList.Items.First().IsDone);

            File.Delete(path); // очистка
        }
    }
}
