using Microsoft.Data.Sqlite;

namespace lab2;

partial class Program
{
    class SqliteStorageStorage : IStorage
    {
        string dbFilePath;

        /*
        ExecuteNonQuery():
        выполняет sql-выражение и возвращает количество измененных записей.
        Подходит для sql-выражений INSERT, UPDATE, DELETE, CREATE.

        ExecuteReader(): выполняет sql-выражение и возвращает считанные из таблицы строки.
        Подходит для sql-выражения SELECT.

        ExecuteScalar(): выполняет sql-выражение и возвращает одно скалярное значение, например, число.
        Подходит для sql-выражения SELECT в паре с одной из встроенных функций SQL, как например, Min, Max, Sum, Count.
        */

        public SqliteStorageStorage()
        {
            dbFilePath = Path.GetTempPath() + "tasks_storage.db";
            Console.WriteLine($"Data base file path: {dbFilePath}");

            CreateTableTasks();
        }

        public bool CreateTask(Task task)
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            string deadline = string.Format("{0:dd-MM-yyyy}", task.deadline);
            string tags = string.Join(',', task.tags);
            string sqlExpression = $"INSERT INTO Tasks (title, description, deadline, tags) VALUES ('{task.title}', '{task.description}', '{deadline}', '{tags}')";
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                Console.WriteLine("Ошибка при добавлении записи в таблицу");
                return false;
            }
            return true;
        }

        public bool UpdateTask(Task task)
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            string deadline = string.Format("{0:dd-MM-yyyy}", task.deadline);
            string tags = string.Join(',', task.tags);
            string sqlExpression = $"UPDATE Tasks SET description='{task.description}', deadline='{deadline}', tags='{tags}' WHERE title='{task.title}'";
            Console.WriteLine("sql expr = " + sqlExpression);
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                Console.WriteLine("Ошибка при добавлении записи в таблицу");
                return false;
            }
            return true;
        }

        public bool DeleteTask(string title)
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            string sqlExpression = $"DELETE FROM Tasks WHERE title='{title}'";
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                Console.WriteLine("Ошибка при удалении записи из таблицы");
                return false;
            }
            return true;
        }

        public Task FindTaskByTitle(string title)
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            string sqlExpression = $"SELECT * FROM Tasks WHERE title='{title}' LIMIT 1";
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string description = reader["description"].ToString();
                string deadline = reader["deadline"].ToString();
                string tags = reader["tags"].ToString();

                Task task;
                task.title = title;
                task.description = description;
                task.deadline = DateTime.ParseExact(deadline, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                task.tags = [];
                string[] words = tags.Split(',');
                foreach (var word in words)
                {
                    task.tags.Add(word);
                }
                return task;
            }
            return new Task { };
        }

        public SortedSet<Task> FindTasksByTags(string[] tags)
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            SortedSet<Task> tasks = GetAllTasks();
            SortedSet<Task> foundTasks = new SortedSet<Task>(new TaskByDeadline());
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
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            SortedSet<Task> tasks = GetAllTasks();
            SortedSet<Task> foundTasks = new SortedSet<Task>(new TaskByDeadline());
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
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            SortedSet<Task> tasks = new SortedSet<Task>(new TaskByDeadline());
            string sqlExpression = "SELECT * FROM Tasks";
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using SqliteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string title = reader["title"].ToString();
                string description = reader["description"].ToString();
                string deadline = reader["deadline"].ToString();
                string tags = reader["tags"].ToString();

                Task task;
                task.title = title;
                task.description = description;
                task.deadline = DateTime.ParseExact(deadline, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                task.tags = [];
                string[] words = tags.Split(',');
                foreach (var word in words)
                {
                    task.tags.Add(word);
                }
                tasks.Add(task);
            }

            return tasks;
        }

        void CreateTableTasks()
        {
            using var connection = new SqliteConnection($"Data Source={dbFilePath}");
            connection.Open();

            string sqlExpression = "CREATE TABLE Tasks(title TEXT NOT NULL PRIMARY KEY UNIQUE, description TEXT NOT NULL, deadline TEXT NOT NULL, tags TEXT NOT NULL)";
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            try
            {
                command.ExecuteNonQuery();
            }
            catch (SqliteException)
            {
                return;
            }

            Console.WriteLine("Таблица Tasks создана");
        }
    }

}