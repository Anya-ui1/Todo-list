namespace TodoList
{

    public struct Task
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
            int titleVal = x.title.CompareTo(y.title);
            if (titleVal == 0)
            {
                return 0; // нужны только уникальные записи
            }
            int deadlineVal = x.deadline.CompareTo(y.deadline);
            if (deadlineVal != 0)
            {
                return deadlineVal;
            }
            return x.description.CompareTo(y.description);
        }
    }
}