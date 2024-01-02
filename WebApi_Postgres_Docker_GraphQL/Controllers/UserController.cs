using WebApi_Postgres_Docker_GraphQL.Domain;
using WebApi_Postgres_Docker_GraphQL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_Postgres_Docker_GraphQL.Controllers;

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

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> Get(Guid id)
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

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, User updatedUser)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await _userService.UpdateAsync(id, updatedUser);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var user = await _userService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _userService.RemoveAsync(id);

        return Ok();
    }
}