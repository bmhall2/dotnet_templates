namespace User.Contract;

public class UserCreated
{
    public UserCreated(User user)
    {
        this.User = user;
    }
    
    public User User { get; set;}
}

public class User
{
    public User(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public string Id { get; }
    public string Name { get; }
}