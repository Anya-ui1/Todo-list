// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace TodoList
{
    public class TaskItem : INotifyPropertyChanged
    {
        private string title;
        private string description;
        private DateTime deadline;
        private ObservableCollection<string> tags;

        public TaskItem(Task task)
        {
            title = task.title;
            description = task.description;
            deadline = task.deadline;
            tags = [];
            foreach (var tag in task.tags)
            {
                tags.Add(tag);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged("Description");
            }
        }

        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        public DateTime Deadline
        {
            get { return deadline; }
            set
            {
                deadline = value;
                OnPropertyChanged("Deadline");
            }
        }

        public ReadOnlyObservableCollection<string> Tags => new ReadOnlyObservableCollection<string>(tags);
    }
}

