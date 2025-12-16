using Xunit;
using Todo.Core;
using System;
using System.IO;
using System.Linq;

namespace Todo.Core.Tests
{
    public class TodoListTests
    {
        [Fact]
        public void Add_IncreasesCount()
        {
            var list = new TodoList();
            list.Add(" task ");
            Assert.Equal(1, list.Count);
            Assert.Equal("task", list.Items.First().Title);
        }

        [Fact]
        public void Remove_ById_Works()
        {
            var list = new TodoList();
            var item = list.Add("a");
            Assert.True(list.Remove(item.Id));
            Assert.Equal(0, list.Count);
        }

        [Fact]
        public void Find_ReturnsMatches()
        {
            var list = new TodoList();
            list.Add("Buy milk");
            list.Add("Read book");
            var found = list.Find("buy").ToList();
            Assert.Single(found);
            Assert.Equal("Buy milk", found[0].Title);
        }

        [Fact]
        public void Save_And_Load_Work_Correctly()
        {
            
            var originalList = new TodoList();
            originalList.Add("Task 1");
            originalList.Add("Task 2");
            originalList.Items.First().MarkDone();

            string tempFile = Path.GetTempFileName();

            try
            {
                
                originalList.Save(tempFile);

                
                Assert.True(File.Exists(tempFile));

                
                var fileContent = File.ReadAllText(tempFile);
                Assert.Contains("Task 1", fileContent);
                Assert.Contains("Task 2", fileContent);
                Assert.Contains("true", fileContent); 

                
                var loadedList = new TodoList();
                loadedList.Load(tempFile);

                
                Assert.Equal(2, loadedList.Count);

                var task1 = loadedList.Items.First(i => i.Title == "Task 1");
                var task2 = loadedList.Items.First(i => i.Title == "Task 2");

                Assert.True(task1.IsDone); 
                Assert.False(task2.IsDone); 
                Assert.NotEqual(Guid.Empty, task1.Id);
                Assert.NotEqual(Guid.Empty, task2.Id);
            }
            finally
            {
                
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

        [Fact]
        public void Save_WithEmptyList_CreatesValidJson()
        {
            
            var list = new TodoList();
            string tempFile = Path.GetTempFileName();

            try
            {
                
                list.Save(tempFile);

                
                var json = File.ReadAllText(tempFile);
                var loadedList = new TodoList();
                loadedList.Load(tempFile);

                Assert.Empty(loadedList.Items);
            }
            finally
            {
                if (File.Exists(tempFile))
                    File.Delete(tempFile);
            }
        }

    }
}
