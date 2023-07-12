using WebApi_Mongo_Docker.Domain;
using WebApi_Mongo_Docker.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_Mongo_Docker.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService) =>
        _userService = userService;

    [HttpGet]
    public async Task<List<User>> Get() =>
        await _userService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User user)
    {
        await _userService.CreateAsync(user);

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _userService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return NoContent();
    }
}