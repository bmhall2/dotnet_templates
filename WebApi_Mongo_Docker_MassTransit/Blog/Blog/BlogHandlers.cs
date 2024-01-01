using MediatR;
using MongoDB.Driver;

namespace blog.Blog;

public class CreateBlogHandler : IRequestHandler<CreateBlog, string>
{
    private readonly IMongoCollection<Blog> blogCollection;

    public CreateBlogHandler(
        IMongoCollection<Blog> blogCollection
    )
    {
        this.blogCollection = blogCollection;
    }

    public async Task<string> Handle(CreateBlog request, CancellationToken cancellationToken)
    {
        if (request == null || request.Blog == null)
            throw new Exception("Invalid blog object");

        var blog = request.Blog;
        await blogCollection.InsertOneAsync(blog);

        return blog.Id ?? "error";
    }
}

public class RetrieveBlogsHandler : IRequestHandler<RetrieveBlogs, List<Blog>>
{
    private readonly IMongoCollection<Blog> blogCollection;

    public RetrieveBlogsHandler(
        IMongoCollection<Blog> blogCollection
    )
    {
        this.blogCollection = blogCollection;
    }

    public async Task<List<Blog>> Handle(RetrieveBlogs request, CancellationToken cancellationToken)
    {
        return await blogCollection.Find(_ => true).ToListAsync();
    }
}

public class RetrieveBlogHandler : IRequestHandler<RetrieveBlog, Blog>
{
    private readonly IMongoCollection<Blog> blogCollection;

    public RetrieveBlogHandler(
        IMongoCollection<Blog> blogCollection
    )
    {
        this.blogCollection = blogCollection;
    }

    public async Task<Blog> Handle(RetrieveBlog request, CancellationToken cancellationToken)
    {
        return await blogCollection.Find(x => x.Id == request.BlogId).FirstOrDefaultAsync();
    }
}

public class UpdateBlogHandler : IRequestHandler<UpdateBlog>
{
    private readonly IMongoCollection<Blog> blogCollection;

    public UpdateBlogHandler(
        IMongoCollection<Blog> blogCollection
    )
    {
        this.blogCollection = blogCollection;
    }

    async Task IRequestHandler<UpdateBlog>.Handle(UpdateBlog request, CancellationToken cancellationToken)
    {
        if (request == null || request.Blog == null)
            throw new Exception("Invalid blog object");

        var blog = request.Blog;
        await blogCollection.ReplaceOneAsync(x => x.Id == request.BlogId, blog);
    }
}

public class DeleteBlogHandler : IRequestHandler<DeleteBlog>
{
    private readonly IMongoCollection<Blog> blogCollection;

    public DeleteBlogHandler(
        IMongoCollection<Blog> blogCollection
    )
    {
        this.blogCollection = blogCollection;
    }

    public async Task Handle(DeleteBlog request, CancellationToken cancellationToken)
    {
        await blogCollection.DeleteOneAsync(x => x.Id == request.BlogId);
    }
}