//using System.Collections.Generic;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using homeworkCore.Interfaces;
using homeworkCore.Models;
using homeworkCore.Services;
//using Users.Interfaces;

namespace homeworkCore.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    IUserServices UsersService;

    public UsersController(IUserServices UserServices)
    {
        this.UsersService = UserServices;
    }

    [HttpGet]
    public ActionResult<List<User>> Get()
    {
        return UsersService.GetAll();
    }

    [HttpGet("{id}")]
    public ActionResult<User> Get(int id)
    {
        var User = UsersService.GetById(id);
        if (User == null)
            return NotFound();
        return User;
    }

    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(User newUser)
    {
        var newId = UsersService.Add(newUser);

        return CreatedAtAction("Post",
            new { id = newId }, UsersService.GetById(newId));
    }

    public ActionResult<String> Login([FromBody] User User)
    {
        var dt = DateTime.Now;

        if (User.Name != "Wray"
        || User.Password != "123456")
        {
            // $"W{dt.Year}#{dt.Day}!"
            return Unauthorized();
        }

        var claims = new List<Claim>
            {
                new Claim("type", "Admin"),
            };

        var token = TasksTokenService.GetToken(claims);

        return new OkObjectResult(TasksTokenService.WriteToken(token));
    }

    [HttpPut("{id}")]
    [Authorize(Policy = "User")]
    public ActionResult Put(int id, User newUser)
    {
        var result = UsersService.Update(id, newUser);
        if (!result)
        {
            return BadRequest();
        }
        return NoContent();
    }

    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        var result = UsersService.Delete(id);
    }
}

internal interface IUserServices
{
}