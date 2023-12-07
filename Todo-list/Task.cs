namespace lab2;

partial class Program
{
    struct Task
    {
        public string title;
        public string description;
        public DateTime deadline;
        public HashSet<string> tags;
    }

    class TaskByDeadline : IComparer<Task>
    {
        public int Compare(Task x, Task y)
        {
            int val = x.deadline.CompareTo(y.deadline);
            if (val != 0)
            {
                return val;
            }
            return x.title.CompareTo(y.title);
        }
    }

}