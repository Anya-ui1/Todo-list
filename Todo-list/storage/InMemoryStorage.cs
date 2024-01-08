using System.Xml.XPath;

namespace lab2;

partial class Program
{
    class InMemoryStorage : IStorage
    {
        static SortedSet<Task> tasks = [];

        public InMemoryStorage()
        {
            tasks = new SortedSet<Task>(new TaskByDeadline());
        }

        public bool CreateTask(Task task)
        {
            Task taskFound = FindTaskByTitle(task.title);
            if (taskFound.Equals(default(Task)))
            {
                return tasks.Add(task);
            }
            Console.WriteLine("Wrong task title, task with this title has already existed");
            return false;
        }

        public bool UpdateTask(Task task)
        {
            bool result = DeleteTask(task.title);
            if (result)
            {
                return CreateTask(task);
            }
            return false;
        }

        public bool DeleteTask(string title)
        {
            Task task = FindTaskByTitle(title);
            if (task.Equals(default(Task)))
            {
                Console.WriteLine("Wrong task title, no task found with this title");
                return false;
            }
            return tasks.Remove(task);
        }

        public Task FindTaskByTitle(string title)
        {
            return tasks.FirstOrDefault(u => u.title == title);
        }

        public SortedSet<Task> FindTasksByTags(string[] tags)
        {
            SortedSet<Task> foundTasks = [];
            var en = tasks.GetEnumerator();
            while (en.MoveNext())
            {
                for (int i = 0; i < tags.Length; ++i)
                {
                    if (en.Current.tags.Contains(tags[i]))
                    {
                        foundTasks.Add(en.Current);
                        break;
                    }
                }
            }
            return foundTasks;
        }

        public SortedSet<Task> FindLastTasks(int number)
        {
            SortedSet<Task> foundTasks = [];
            var en = tasks.GetEnumerator();
            int i = 0;
            while (en.MoveNext() && i < number)
            {
                foundTasks.Add(en.Current);
                ++i;
            }
            return foundTasks;
        }

        public SortedSet<Task> GetAllTasks()
        {
            return tasks;
        }
    }

}