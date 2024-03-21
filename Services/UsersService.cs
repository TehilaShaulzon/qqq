using Tasks.Interfaces;
using Tasks.Models;
using System.Text.Json;
namespace Tasks.Services;
public class UsersService : IUserServices
{
    private List<User> User;
    private string fileName = "User.json";
    public UsersService()
    {
        this.fileName = Path.Combine("data", "users.json");
        using (var jsonFile = File.OpenText(fileName))
        {
            User = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(User));
    }
    public List<User> GetAll() => User;

    public User GetById(int id)
    {
        return User.FirstOrDefault(p => p.Id == id);
    }

    public int Add(User newUser)
    {
        if (User.Count == 0)

        {
            newUser.Id = 1;
        }
        else
        {
            newUser.Id = User.Max(p => p.Id) + 1;

        }

        User.Add(newUser);
        saveToFile();
        return newUser.Id;
    }

    public bool Update(int id, User newUser)
    {
        Console.WriteLine("1");
        if (id != newUser.Id)
            return false;
        Console.WriteLine("2");

        var existingUser = GetById(id);
        if (existingUser == null)
            return false;
        Console.WriteLine("3");

        var index = User.IndexOf(existingUser);
        if (index == -1)
            return false;
        Console.WriteLine("4");

        User[index] = newUser;
        saveToFile();

        return true;
    }


    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null)
            return false;

        var index = User.IndexOf(existingUser);
        if (index == -1)
            return false;

        User.RemoveAt(index);
        saveToFile();
        return true;
    }

    public int ExistUser(string name, string password)
    {
        User existUser = User.FirstOrDefault(u => u.Name.Equals(name) && u.Password.Equals(password));
        if (existUser != null)
            return existUser.Id;
        return -1;
    }
}

public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserServices, UsersService>();
    }
}
//  public bool Update(string name,string password, User newUser)
//     {


//         Console.WriteLine("1");
//         int existingUser=ExistUser(name,password);
//         if (existingUser != newUser.Id)
//             return false;
//         Console.WriteLine("2");

//         var index = User.IndexOf(GetById(existingUser));
//         if (index == -1)
//             return false;
//         Console.WriteLine("4");

//         User[index] = newUser;
//         saveToFile();

//         return true;
//     }
