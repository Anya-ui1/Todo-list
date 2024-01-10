using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Http;

namespace TodoList;

class Api
{
    static IStorage storage;

    public Api(IStorage st)
    {
        storage = st;
    }

    public void Run()
    {
        var builder = WebApplication.CreateBuilder();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.MapGet("/tasks", () =>
        {
            SortedSet<Task> tasks = storage.GetAllTasks();
            return JsonConvert.SerializeObject(tasks); ;
        });

        app.MapGet("/tasks/title={title}", (string title) =>
        {
            Task task = storage.FindTaskByTitle(title);
            return JsonConvert.SerializeObject(task); ;
        });

        app.MapGet("/tasks/tag={tag}", (string tag) =>
        {
            string[] tags = [];
            tags[0] = tag;
            SortedSet<Task> tasks = storage.FindTasksByTags(tags);
            return JsonConvert.SerializeObject(tasks);
        });

        app.MapGet("/last-tasks/number={number}", (int number) =>
        {
            string[] tags = [];
            SortedSet<Task> tasks = storage.FindLastTasks(number);
            return JsonConvert.SerializeObject(tasks);
        });

        app.MapPost("/tasks", async (HttpRequest req) =>
        {
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                Task? task = ParseTaskFromJson(await reader.ReadToEndAsync());
                if (task == null)
                {
                    return JsonConvert.SerializeObject("не удалось создать задачу");
                }
                bool result = storage.CreateTask((Task)task);
                if (result) return JsonConvert.SerializeObject(task);
                return JsonConvert.SerializeObject("не удалось создать задачу");
            }


        });

        app.MapPut("/tasks", async (HttpRequest req) =>
        {
            using (var reader = new StreamReader(req.Body, Encoding.UTF8))
            {
                Task? task = ParseTaskFromJson(await reader.ReadToEndAsync());
                if (task == null)
                {
                    return JsonConvert.SerializeObject("не удалось изменить задачу");
                }

                bool result = storage.UpdateTask((Task)task);
                if (result) return JsonConvert.SerializeObject(task);
                return JsonConvert.SerializeObject("не удалось изменить задачу");
            }
        });

        app.MapDelete("/tasks/title={title}", (string title) =>
        {
            bool result = storage.DeleteTask(title);
            if (result) return JsonConvert.SerializeObject("задача удалена");
            return JsonConvert.SerializeObject("не удалось удалить задачу");
        });

        app.Run("http://localhost:8080");
    }

    private Task? ParseTaskFromJson(string value)
    {
        dynamic? parsed = JsonConvert.DeserializeObject<dynamic>(value);
        if (parsed == null)
        {
            return null;
        }

        Task task;
        task.title = parsed.title;
        task.description = parsed.description;
        task.deadline = DateTime.ParseExact((string)parsed.deadline, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
        task.tags = [];
        foreach (string tag in parsed.tags)
        {
            task.tags.Add(tag);
        }

        return task;
    }
}