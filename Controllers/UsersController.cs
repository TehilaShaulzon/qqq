using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tasks.Interfaces;
using Tasks.Models;
using Tasks.Services;
namespace Tasks.Controllers;


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
    [Authorize(Policy = "Admin")]
    public ActionResult<List<User>> Get()
    {
        return UsersService.GetAll();
    }


    [HttpGet("{id}")]
    [Authorize(Policy = "User")]

    public ActionResult<User> Get(int id)
    {
        if ((int.Parse(User.FindFirst("id")?.Value!) != id) && User.FindFirst("type")?.Value != "Admin")
            return Unauthorized();
        var user = UsersService.GetById(id);
        if (user == null)
            return NotFound();
        return user;
    }


    [HttpPost]
    [Authorize(Policy = "Admin")]
    public ActionResult Post(User newUser)
    {
        var newId = UsersService.Add(newUser);

        return CreatedAtAction("Post",
            new { id = newId }, UsersService.GetById(newId));
    }


    [HttpPost]
    [Route("/login")]
    public ActionResult<objectToReturn> Login([FromBody] User User)
    {

        int UserExistID = UsersService.ExistUser(User.Name, User.Password);
        // var dt = DateTime.Now;
        if (UserExistID == -1)
        {
            return Unauthorized();
        }

        var claims = new List<Claim> { };
        if (User.Password == "123")
            claims.Add(new Claim("type", "Admin"));
        else
            claims.Add(new Claim("type", "User"));

        claims.Add(new Claim("id", UserExistID.ToString()));

        var token = TasksTokenService.GetToken(claims);
        return new OkObjectResult(new{Id=UserExistID,token=TasksTokenService.WriteToken(token)}) ;
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
    [Authorize(Policy = "Admin")]
    public ActionResult Delete(int id)
    {
        bool result = UsersService.Delete(id);
        if (!result)
            return NotFound();
        return NoContent();
    }
}
// public ActionResult Put(string name,string password, User newUser)
//     {
//         newUser.Id=UsersService.ExistUser(name,password);

//         var result = UsersService.Update(name,password, newUser);
//         if (!result)
//         {
//             return BadRequest();
//         }
//         return NoContent();
//     }

public class objectToReturn
{
    public int Id { get; set; }

    public string token { get; set; }
}

