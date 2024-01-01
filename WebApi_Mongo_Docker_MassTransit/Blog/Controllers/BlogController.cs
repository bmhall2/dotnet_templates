using MediatR;
using Microsoft.AspNetCore.Mvc;
using blog.Blog;

namespace blog.Controllers;

[ApiController]
[Route("api/blog")]
public class BlogController : ControllerBase
{
    private readonly IMediator mediator;

    public BlogController(IMediator mediator)
    {
        this.mediator = mediator;
    }

    [HttpGet]
    public async Task<List<Blog.Blog>> Get()
    {
        return await mediator.Send(new RetrieveBlogs());
    }

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Blog.Blog>> Get(string id)
    {
        var blog = await mediator.Send(new RetrieveBlog(id));

        if (blog is null)
        {
            return NotFound();
        }

        return blog;
    }

    [HttpPost]
    public async Task<IActionResult> Post(Blog.Blog blog)
    {
        await mediator.Send(new CreateBlog(blog));

        return CreatedAtAction(nameof(Get), new { id = blog.Id }, blog);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, Blog.Blog updatedBlog)
    {
        var blog = await mediator.Send(new RetrieveBlog(id));

        if (blog is null)
        {
            return NotFound();
        }

        updatedBlog.Id = blog.Id;

        await mediator.Send(new UpdateBlog(id, updatedBlog));

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await mediator.Send(new RetrieveBlog(id));

        if (book is null)
        {
            return NotFound();
        }

        await mediator.Send(new DeleteBlog(id));

        return NoContent();
    }
}