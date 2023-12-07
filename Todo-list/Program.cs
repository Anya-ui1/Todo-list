using System.ComponentModel;

namespace lab2;

partial class Program
{
    static SortedSet<Task> tasks;

    static Program()
    {
        tasks = new SortedSet<Task>(new TaskByDeadline());
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Enter the number of action and press [Enter]. Then follow instructions.");

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
        Console.WriteLine("Deadline: {0:dd.MM.yyyy}", task.deadline);
        Console.Write("Tags: ");
        var tagsEn = task.tags.GetEnumerator();
        while (tagsEn.MoveNext())
        {
            Console.Write($"{tagsEn.Current}, ");
        }
        Console.WriteLine();
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
        task.deadline = DateTime.ParseExact(deadline, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);

        Console.WriteLine("Tags (finish on empty line)");
        task.tags = new HashSet<string>();
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

        tasks.Add(task);
    }

    static void SearchTask()
    {
        Console.WriteLine("Search tasks by tag: ");
        var tagLine = Console.ReadLine();
        if (tagLine == null || tagLine == "")
        {
            Console.WriteLine("No such tasks");
            return;
        }
        string[] tags = tagLine.Split(' ');
        var en = tasks.GetEnumerator();
        Console.WriteLine("Tasks found by tag:");
        while (en.MoveNext())
        {
            for (int i = 0; i < tags.Length; ++i)
            {
                if (en.Current.tags.Contains(tags[i]))
                {
                    PrintTask(en.Current);
                    break;
                }
            }

        }
    }

    static void LastTasks()
    {
        Console.Write("Number of last tasks to show: ");
        var line = Console.ReadLine();
        if (Int32.TryParse(line, out int number))
        {
            Console.WriteLine("Actual tasks:");
            var en = tasks.GetEnumerator();
            int i = 0;
            while (en.MoveNext() && i < number)
            {
                PrintTask(en.Current);
                ++i;
            }
        }
        else
        {
            Console.WriteLine("Wrong number of last tasks to show");
        }

    }
}