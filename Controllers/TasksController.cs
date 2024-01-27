//using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Tasks.Models;
//  using Tasks.Services;
 //using Tasks.Interfaces;

namespace Tasks.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    ITaskServices TasksService;

    public TasksController(ITaskServices taskServices)
    {
        this.TasksService=taskServices;
    }

    [HttpGet]
    public ActionResult<List<Todo>> Get()
    {
        return TasksService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<Todo> Get(int id)
    {
        var todo = TasksService.GetById(id);
        if (todo == null)
            return NotFound();
        return todo;
    }

    [HttpPost]
    public ActionResult Post(Todo newTask)
    {
        var newId = TasksService.Add(newTask);

        return CreatedAtAction("Post", 
            new {id = newId}, TasksService.GetById(newId));
    }

    [HttpPut("{id}")]
    public ActionResult Put(int id,Todo newTask)
    {
        var result = TasksService.Update(id, newTask);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var result=TasksService.Delete(id);
    }
}
