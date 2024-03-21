using Tasks.Models;
using System.Collections.Generic;

namespace Tasks.Interfaces;

public interface ITaskServices
{
    List<Todo> GetAll(int userId);

    Todo GetById(int id);
    
    int Add(Todo newTodo,int userId);
 
    bool Update(int id, Todo newTodo,int userId);
    
    bool Delete(int id);
}