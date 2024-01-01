using MediatR;
using Microsoft.AspNetCore.Mvc;
using user.User;

namespace user.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    private readonly IMediator mediator;

    public UserController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<List<User.User>> Get()
    {
        return await mediator.Send(new RetrieveUsers());
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User.User>> Get(string id)
    {
        var user = await mediator.Send(new RetrieveUser(id));

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(User.User user)
    {
        await mediator.Send(new CreateUser(user));

        return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User.User updatedUser)
    {
        var user = await mediator.Send(new RetrieveUser(id));

        if (user is null)
        {
            return NotFound();
        }

        updatedUser.Id = user.Id;

        await mediator.Send(new UpdateUser(id, updatedUser));

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await mediator.Send(new RetrieveUser(id));

        if (book is null)
        {
            return NotFound();
        }

        await mediator.Send(new DeleteUser(id));

        return NoContent();
    }
}