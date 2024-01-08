using Newtonsoft.Json;

namespace lab2;

partial class Program
{
    class JsonStorage : IStorage
    {
        string dbFilePath;
        static IStorage inMemoryStorage;

        public JsonStorage()
        {
            dbFilePath = Path.GetTempPath() + "tasks_storage.json";
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

            bool result = inMemoryStorage.CreateTask(task);
            if (result)
            {
                SortedSet<Task> tasks = inMemoryStorage.GetAllTasks();
                return SaveTasks(tasks);
            }

            return false;
        }

        public bool UpdateTask(Task task)
        {
            Task taskFound = inMemoryStorage.FindTaskByTitle(task.title);
            if (taskFound.Equals(default(Task)))
            {
                return false;
            }

            bool result = inMemoryStorage.UpdateTask(task);
            if (result)
            {
                SortedSet<Task> tasks = inMemoryStorage.GetAllTasks();
                return SaveTasks(tasks);
            }

            return false;
        }

        public bool DeleteTask(string title)
        {
            Task taskFound = inMemoryStorage.FindTaskByTitle(title);
            if (taskFound.Equals(default(Task)))
            {
                return false;
            }

            bool result = inMemoryStorage.DeleteTask(title);
            if (result)
            {
                SortedSet<Task> tasks = inMemoryStorage.GetAllTasks();
                return SaveTasks(tasks);
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

        bool SaveTasks(SortedSet<Task> tasks)
        {
            string serializedTasks = JsonConvert.SerializeObject(tasks);
            File.WriteAllText(dbFilePath, serializedTasks);
            return true;
        }
    }

}