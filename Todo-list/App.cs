using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace TodoList
{
    public partial class App : Application
    {
        // static void Main(string[] args)
        // {
        //     Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");

        //     PrintStorageVariants();
        //     ChooseStorage();

        //     var api = new Api(storage);
        //     Parallel.Invoke(
        //      () => api.Run(),
        //      () => RunConsoleCommands());

        // }

        public ObservableCollection<TaskItem> TaskItems { get; set; } = new ObservableCollection<TaskItem>();

        private void AppStartup(object sender, StartupEventArgs args)
        {
            // var api = new Api(storage);
            // Parallel.Invoke(() => api.Run());

            Task task;
            task.title = "Test task";
            task.description = "Some description";
            task.deadline = DateTime.ParseExact("10-01-2024", "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
            task.tags = [];

            TaskItems.Add(new TaskItem(task));
        }

        static void RunConsoleCommands()
        {
            for (; ; )
            {
                PrintMenu();
                var line = Console.ReadLine();
                if (Int32.TryParse(line, out int action))
                {
                    switch (action)
                    {
                        case 1:
                            AddTask();
                            break;
                        case 2:
                            SearchTask();
                            break;
                        case 3:
                            LastTasks();
                            break;
                        case 4:
                            return;
                        default:
                            Console.WriteLine("Wrong number of action");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong number of action");
                }
            }
        }

        static void PrintMenu()
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Add task");
            Console.WriteLine("2. Search task");
            Console.WriteLine("3. Last tasks");
            Console.WriteLine("4. Exit");
            Console.Write("> ");
        }

        static void PrintTask(Task task)
        {
            Console.WriteLine($"Title: {task.title}");
            Console.WriteLine($"Description: {task.description}");
            Console.WriteLine("Deadline: {0:dd-MM-yyyy}", task.deadline);
            Console.Write("Tags: ");
            string tags = string.Join(", ", task.tags);
            Console.WriteLine(tags);
        }

        static void AddTask()
        {
            Task task;
            Console.WriteLine("New task");
            Console.Write("Title: ");
            var title = Console.ReadLine();
            if (title == null || title == "")
            {
                Console.WriteLine("Task title can't be empty");
                return;
            }
            Console.Write("Description: ");
            var description = Console.ReadLine();
            if (description == null || description == "")
            {
                Console.WriteLine("Task description can't be empty");
                return;
            }
            Console.Write("Deadline: ");
            var deadline = Console.ReadLine();
            if (deadline == null || deadline == "")
            {
                Console.WriteLine("Task deadline can't be empty");
                return;
            }

            task.title = title;
            task.description = description;
            task.deadline = DateTime.ParseExact(deadline, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);

            Console.WriteLine("Tags (finish on empty line)");
            task.tags = [];
            for (int i = 1; ; i++)
            {
                Console.Write($"{i}: ");
                var tag = Console.ReadLine();
                if (tag == null || tag == "")
                {
                    break;
                }
                task.tags.Add(tag);
            }

            storage.CreateTask(task);
        }

        static void SearchTask()
        {
            Console.WriteLine("Search tasks by tags: ");
            var tagLine = Console.ReadLine();
            if (tagLine == null || tagLine == "")
            {
                Console.WriteLine("No such tasks");
                return;
            }
            string[] tags = tagLine.Split(' ');
            SortedSet<Task> tasks = storage.FindTasksByTags(tags);
            if (tasks == null || tasks.Count == 0)
            {
                Console.WriteLine("No tasks found by tags");
                return;
            }

            Console.WriteLine("Tasks found by tags:");
            var en = tasks.GetEnumerator();
            while (en.MoveNext())
            {
                PrintTask(en.Current);
                break;
            }
        }

        static void LastTasks()
        {
            Console.Write("Number of last tasks to show: ");
            var line = Console.ReadLine();
            if (Int32.TryParse(line, out int number))
            {
                SortedSet<Task> tasks = storage.FindLastTasks(number);
                if (tasks == null || tasks.Count == 0)
                {
                    Console.WriteLine("No tasks in the storage");
                    return;
                }

                Console.WriteLine("Actual tasks:");
                var en = tasks.GetEnumerator();
                while (en.MoveNext())
                {
                    PrintTask(en.Current);
                }
            }
            else
            {
                Console.WriteLine("Wrong number of last tasks to show");
            }

        }
    }
}