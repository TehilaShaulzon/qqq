using homeworkCore.Interfaces;
using homeworkCore.Models;
using System.Text.Json;
namespace homeworkCore.Services;
public class UsersService:IUserServices
{
    private List<User> User;
    private string fileName="User.json" ;
    public UsersService()
    {
        this.fileName = Path.Combine( "data", "users.json");
        
        using (var jsonFile = File.OpenText(fileName))
        {
            User = JsonSerializer.Deserialize<List<User>>(jsonFile.ReadToEnd(),
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        
        }
    }

    private void saveToFile()
    {
        File.WriteAllText(fileName, JsonSerializer.Serialize(User));
    }
    public List<User> GetAll() => User;

    public User GetById(int id) 
    {
        return User.First(p => p.Id == id);
    }

    public int Add(User newUser)
    {
        if (User.Count == 0)

            {
                newUser.Id = 1;
            }
            else
            {
        newUser.Id =  User.Max(p => p.Id) + 1;

            }

        User.Add(newUser);
        saveToFile();
        return newUser.Id;
    }
  
    public bool Update(int id, User newUser)
    {
        if (id != newUser.Id)
            return false;

        var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = User.IndexOf(existingUser);
        if (index == -1 )
            return false;

        User[index] = newUser;
        saveToFile();

        return true;
    }  

      
    public bool Delete(int id)
    {
        var existingUser = GetById(id);
        if (existingUser == null )
            return false;

        var index = User.IndexOf(existingUser);
        if (index == -1 )
            return false;

        User.RemoveAt(index);
        saveToFile();
        return true;
    }  
}
public static class UserUtils
{
    public static void AddUser(this IServiceCollection services)
    {
        services.AddSingleton<IUserServices, UsersService>();
    }
}