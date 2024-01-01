using MediatR;

namespace blog.Blog;

public class CreateBlog : IRequest<string>
{
    public CreateBlog(Blog blog)
    {
        this.Blog = blog;
    }

    public Blog Blog { get; set; }
}

public class RetrieveBlogs : IRequest<List<Blog>>
{

}

public class RetrieveBlog : IRequest<Blog>
{
    public RetrieveBlog(string blogId)
    {
        BlogId = blogId;
    }

    public string BlogId { get; set; }
}

public class UpdateBlog : IRequest
{
    public UpdateBlog(string blogId, Blog blog)
    {
        BlogId = blogId;
        Blog = blog;
    }

    public string BlogId { get; set; }
    public Blog Blog { get; set; }
}

public class DeleteBlog : IRequest
{
    public DeleteBlog(string blogId)
    {
        BlogId = blogId;
    }

    public string BlogId { get; set; }
}