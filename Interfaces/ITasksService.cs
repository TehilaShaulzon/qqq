using Tasks.Models;
using System.Collections.Generic;

namespace Tasks.Interfaces;

public interface ITaskServices
{
    List<Todo> GetAll();

    Todo GetById(int id);
    
    int Add(Todo newTodo);
 
    bool Update(int id, Todo newTodo);
    
    bool Delete(int id);
}