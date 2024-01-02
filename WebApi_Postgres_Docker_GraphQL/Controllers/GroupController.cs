using WebApi_Postgres_Docker_GraphQL.Domain;
using WebApi_Postgres_Docker_GraphQL.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi_Postgres_Docker_GraphQL.Controllers;

[ApiController]
[Route("api/group")]
public class GroupController : ControllerBase
{
    private readonly GroupService _groupService;

    public GroupController(GroupService groupService) =>
        _groupService = groupService;

    [HttpGet]
    public async Task<List<Group>> Get() =>
        await _groupService.GetAsync();

    [HttpGet("{id}")]
    public async Task<ActionResult<Group>> Get(Guid id)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        return group;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Group group)
    {
        await _groupService.CreateAsync(group);

        return CreatedAtAction(nameof(Get), new { id = group.Id }, group);
    }

    [HttpPost("{groupId}/User/{userId}")]
    public async Task<IActionResult> PostUserToGroup(Guid groupId, Guid userId)
    {
        await _groupService.CreateGroupUserAsync(groupId, userId);

        return CreatedAtAction(nameof(Get), new { id = groupId }, null);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Group updatedGroup)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        updatedGroup.Id = group.Id;

        await _groupService.UpdateAsync(id, updatedGroup);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var group = await _groupService.GetAsync(id);

        if (group is null)
        {
            return NotFound();
        }

        await _groupService.RemoveAsync(id);

        return Ok();
    }
}