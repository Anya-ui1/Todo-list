// using System.Data.Common;
using Xunit;

namespace TodoList;

public class Testclass
{
    [Fact]
    public void TestTask1()
    {
        var storage = new InMemoryStorage();

        Task task;
        task.title = "TestTask";
        task.description = "TestTask description";
        task.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        task.tags = [];
        task.tags.Add("test");

        Assert.True(storage.CreateTask(task));

        // Wrong task title, task with this title has already existed
        Assert.False(storage.CreateTask(task));

        task.description = "New TestTask description";
        Assert.True(storage.UpdateTask(task));

        Task notExistedTask;
        notExistedTask.title = "NotExistedTestTask";
        notExistedTask.description = "TestTask description";
        notExistedTask.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        notExistedTask.tags = [];
        notExistedTask.tags.Add("no");

        Assert.False(storage.UpdateTask(notExistedTask));

        Assert.False(storage.DeleteTask(notExistedTask.title));

        Assert.Equal(task, storage.FindTaskByTitle(task.title));

        Assert.True(storage.DeleteTask(task.title));
    }

    [Fact]
    public void TestTask2()
    {
        var storage = new JsonStorage();

        Task task;
        task.title = "TestTask2";
        task.description = "TestTask description";
        task.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        task.tags = [];
        task.tags.Add("test");

        Assert.True(storage.CreateTask(task));

        // Wrong task title, task with this title has already existed
        Assert.False(storage.CreateTask(task));

        task.description = "New TestTask description";
        Assert.True(storage.UpdateTask(task));

        Task notExistedTask;
        notExistedTask.title = "NotExistedTestTask2";
        notExistedTask.description = "TestTask description";
        notExistedTask.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        notExistedTask.tags = [];
        notExistedTask.tags.Add("no");

        Assert.False(storage.UpdateTask(notExistedTask));

        Assert.False(storage.DeleteTask(notExistedTask.title));

        Assert.Equal(task, storage.FindTaskByTitle(task.title));

        Assert.True(storage.DeleteTask(task.title));
    }

    [Fact]
    public void TestTask3()
    {
        var storage = new XmlStorage();

        Task task;
        task.title = "TestTask3";
        task.description = "TestTask description";
        task.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        task.tags = [];
        task.tags.Add("test");

        Assert.True(storage.CreateTask(task));

        // Wrong task title, task with this title has already existed
        Assert.False(storage.CreateTask(task));

        task.description = "New TestTask description";
        Assert.True(storage.UpdateTask(task));

        Task notExistedTask;
        notExistedTask.title = "NotExistedTestTask3";
        notExistedTask.description = "TestTask description";
        notExistedTask.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        notExistedTask.tags = [];
        notExistedTask.tags.Add("no");

        Assert.False(storage.UpdateTask(notExistedTask));

        Assert.False(storage.DeleteTask(notExistedTask.title));

        Assert.Equal(task, storage.FindTaskByTitle(task.title));

        Assert.True(storage.DeleteTask(task.title));
    }

    [Fact]
    public void TestTask4()
    {
        var storage = new SqliteStorage();

        Task task;
        task.title = "TestTask4";
        task.description = "TestTask description";
        task.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        task.tags = [];
        task.tags.Add("test");

        Assert.True(storage.CreateTask(task));

        // Wrong task title, task with this title has already existed
        Assert.False(storage.CreateTask(task));

        task.description = "New TestTask description";
        Assert.True(storage.UpdateTask(task));

        Task notExistedTask;
        notExistedTask.title = "NotExistedTestTask4";
        notExistedTask.description = "TestTask description";
        notExistedTask.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        notExistedTask.tags = [];
        notExistedTask.tags.Add("no");

        Assert.False(storage.UpdateTask(notExistedTask));

        Assert.False(storage.DeleteTask(notExistedTask.title));

        Assert.Equal(task, storage.FindTaskByTitle(task.title));

        Assert.True(storage.DeleteTask(task.title));
    }
}