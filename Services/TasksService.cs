using Tasks.Interfaces;
using Tasks.Models;
using System.Text.Json;
namespace Tasks.Services;
public class TasksService : ITaskServices
{
    private List<Todo> todo;
    private string fileName = "Task.json";
    public TasksService()
    {
        this.fileName = Path.Combine("data", "tasks.json");

        using (var jsonFile = File.OpenText(fileName))
        {
            todo = JsonSerializer.Deserialize<List<Todo>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(todo));
    }
    public List<Todo> GetAll(int userId)
    {
       return todo.FindAll(t=>t.UserId==userId);
    }

    public Todo GetById(int id)
    {
        return todo.First(p => p.Id == id);
    }

    public int Add(Todo newTodo,int userId)
    {
        newTodo.UserId=userId;
        if (todo.Count == 0)
            newTodo.Id = 1;
        else
            newTodo.Id = todo.Max(p => p.Id) + 1;

        todo.Add(newTodo);
        saveToFile();
        return newTodo.Id;
    }

    public bool Update(int id, Todo newTodo,int userId)
    {
        if (id != newTodo.Id)
            return false;

        var existingTask = GetById(id);
        if (existingTask == null)
            return false;
            
        newTodo.UserId=userId;

        var index = todo.IndexOf(existingTask);
        if (index == -1)
            return false;

        todo[index] = newTodo;
        saveToFile();

        return true;
    }


    public bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null)
            return false;

        var index = todo.IndexOf(existingTask);
        if (index == -1)
            return false;

        todo.RemoveAt(index);
        saveToFile();
        return true;
    }
}
public static class TaskUtils
{
    public static void AddTask(this IServiceCollection services)
    {
        services.AddSingleton<ITaskServices, TasksService>();
    }
}