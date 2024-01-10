namespace TodoList;

public partial class App
{
    static IStorage storage;

    static void PrintStorageVariants()
    {
        Console.WriteLine("Choose data storage:");
        Console.WriteLine("1. JSON");
        Console.WriteLine("2. XML");
        Console.WriteLine("3. SQLite");
        Console.Write("> ");
    }

    static void ChooseStorage()
    {
        var line = Console.ReadLine();
        if (Int32.TryParse(line, out int action))
        {
            switch (action)
            {
                case (int)IStorage.StorageType.jsonStorage:
                    storage = new JsonStorage();
                    break;
                case (int)IStorage.StorageType.xmlStorage:
                    storage = new XmlStorage();
                    break;
                case (int)IStorage.StorageType.sqliteStorage:
                    storage = new SqliteStorage();
                    break;
                default:
                    storage = new InMemoryStorage();
                    Console.WriteLine("Wrong number of action, storage will be just in memory");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Wrong number of action, storage will be just in memory");
        }
    }
}

interface IStorage
{
    enum StorageType
    {
        jsonStorage = 1,
        xmlStorage,
        sqliteStorage
    }

    public abstract bool CreateTask(Task task);
    public abstract bool UpdateTask(Task task);
    public abstract bool DeleteTask(string title);
    public abstract Task FindTaskByTitle(string title);
    public abstract SortedSet<Task> FindTasksByTags(string[] tags);
    public abstract SortedSet<Task> FindLastTasks(int number);
    public abstract SortedSet<Task> GetAllTasks();
}