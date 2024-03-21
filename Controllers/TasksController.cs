using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Tasks.Models;

namespace Tasks.Controllers;

[ApiController]
[Route("[controller]")]
public class TasksController : ControllerBase
{
    ITaskServices TasksService;

    public TasksController(ITaskServices taskServices)
    {
        this.TasksService = taskServices;
    }

    [HttpGet]
    [Authorize(Policy = "User")]

    public ActionResult<List<Todo>> Get()
    {
        return TasksService.GetAll(int.Parse(User.FindFirst("id")?.Value!));
    }

    [HttpGet("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult<Todo> Get(int id)
    {

        var todo = TasksService.GetById(id);
        if (todo == null)
            return NotFound();
        return todo;
    }

    [HttpPost]
    [Authorize(Policy = "User")]

    public ActionResult Post(Todo newTask)
    {

        var newId = TasksService.Add(newTask,int.Parse(User.FindFirst("id")?.Value!));

        return CreatedAtAction("Post",
            new { id = newId}, TasksService.GetById(newId));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult Put(int id, Todo newTask)
    {

        var result = TasksService.Update(id, newTask,int.Parse(User.FindFirst("id")?.Value!));
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult Delete(int id)
    {

        bool result = TasksService.Delete(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
