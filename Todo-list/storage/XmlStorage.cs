using System.IO;
using System.Xml.Linq;

namespace TodoList;

class XmlStorage : IStorage
{
    string dbFilePath;
    static IStorage inMemoryStorage;

    public XmlStorage()
    {
        dbFilePath = Path.GetTempPath() + "tasks_storage.xml";
        Console.WriteLine($"Data base file path: {dbFilePath}");
        // чтобы каждый раз не считывать файл, будем хранить всё в памяти
        inMemoryStorage = new InMemoryStorage();
    }

    public bool CreateTask(Task task)
    {
        Task taskFound = inMemoryStorage.FindTaskByTitle(task.title);
        if (!taskFound.Equals(default(Task)))
        {
            return false;
        }

        try
        {
            XDocument xdoc = XDocument.Load(dbFilePath);
            XElement? root = xdoc.Element("tasks");
            if (root != null)
            {
                root.Add(new XElement("task",
                            new XAttribute("title", task.title),
                            new XElement("description", task.description),
                            new XElement("deadline", task.deadline),
                            new XElement("tags", task.tags)));
                xdoc.Save(dbFilePath);
            }
        }
        catch (FileNotFoundException)
        {
            XDocument xdoc = new XDocument(new XElement("tasks",
                            new XElement("task",
                            new XAttribute("title", task.title),
                            new XElement("description", task.description),
                            new XElement("deadline", task.deadline),
                            new XElement("tags", task.tags))));
            xdoc.Save(dbFilePath);
        }

        return inMemoryStorage.CreateTask(task);
    }

    public bool UpdateTask(Task task)
    {
        Task taskFound = inMemoryStorage.FindTaskByTitle(task.title);
        if (taskFound.Equals(default(Task)))
        {
            return false;
        }

        XDocument xdoc = XDocument.Load(dbFilePath);

        var taskOld = xdoc.Element("tasks")?
            .Elements("task")
            .FirstOrDefault(p => p.Attribute("title")?.Value == task.title);

        if (taskOld != null)
        {
            var description = taskOld.Element("description");
            if (description != null) description.Value = task.description;

            var deadline = taskOld.Element("deadline");
            if (deadline != null) deadline.Value = task.deadline.ToString();

            var tags = taskOld.Element("deadline");
            if (tags != null) tags.Value = task.tags.ToString();

            xdoc.Save(dbFilePath);
            return inMemoryStorage.UpdateTask(task);
        }

        return false;
    }

    public bool DeleteTask(string title)
    {
        XDocument xdoc = XDocument.Load(dbFilePath);
        XElement? root = xdoc.Element("tasks");

        if (root != null)
        {
            var elem = root.Elements("task")
                .FirstOrDefault(p => p.Attribute("title")?.Value == title);
            if (elem != null)
            {
                elem.Remove();
                xdoc.Save(dbFilePath);
                return inMemoryStorage.DeleteTask(title);
            }
        }

        return false;
    }

    public Task FindTaskByTitle(string title)
    {
        return inMemoryStorage.FindTaskByTitle(title);
    }

    public SortedSet<Task> FindTasksByTags(string[] tags)
    {
        return inMemoryStorage.FindTasksByTags(tags);
    }

    public SortedSet<Task> FindLastTasks(int number)
    {
        return inMemoryStorage.FindLastTasks(number);
    }

    public SortedSet<Task> GetAllTasks()
    {
        return inMemoryStorage.GetAllTasks();
    }
}