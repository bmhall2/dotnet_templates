using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<List<User>> Get()
    {
        return await mediator.Send(new RetrieveUsersRequest());
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var user = await mediator.Send(new RetrieveUser(id));

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpPost]
    public async Task<IActionResult> Post(CreateUserRequest request)
    {
        var id = await mediator.Send(request);

        return CreatedAtAction(nameof(Get), new { id = id }, user);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UpdateUserRequest request)
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