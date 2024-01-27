using Tasks.Interfaces;
using Tasks.Models;
using System.Text.Json;
namespace Tasks.Services;
public class TasksService:ITaskServices
{
    private List<Todo> todo;
    private string fileName = "Task.json";
    public TasksService(/*IWebHostEnvinronment webHost*/)
    {
        // todo = new List<Todo>
        // {
        //     new Todo { Id = 1, Name = "homeWork", IsDone = false},
        //     new Todo { Id = 2, Name = "Organize the house", IsDone = false},
        //     new Todo { Id = 3, Name = "Special", IsDone = true}
        // };
        this.fileName = Path.Combine(/*webHost.ContentRootPath,*/ "data", "tasks.json");
        // saveToFile();
        
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
    public List<Todo> GetAll() => todo;

    public Todo GetById(int id) 
    {
        return todo.First(p => p.Id == id);
    }

    public int Add(Todo newTodo)
    {
        if (todo.Count == 0)

            {
                newTodo.Id = 1;
            }
            else
            {
        newTodo.Id =  todo.Max(p => p.Id) + 1;

            }

        todo.Add(newTodo);
        saveToFile();
        return newTodo.Id;
    }
  
    public bool Update(int id, Todo newTodo)
    {
        if (id != newTodo.Id)
            return false;

        var existingTask = GetById(id);
        if (existingTask == null )
            return false;

        var index = todo.IndexOf(existingTask);
        if (index == -1 )
            return false;

        todo[index] = newTodo;
        saveToFile();

        return true;
    }  

      
    public bool Delete(int id)
    {
        var existingTask = GetById(id);
        if (existingTask == null )
            return false;

        var index = todo.IndexOf(existingTask);
        if (index == -1 )
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