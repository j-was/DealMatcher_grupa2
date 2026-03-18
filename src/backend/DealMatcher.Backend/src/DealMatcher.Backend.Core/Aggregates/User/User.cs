namespace DealMatcher.Backend.Core.Aggregates.User;

public sealed class User :
  DealMatcherEntityBase,
  IAggregateRoot
{
    public string Name { get; private set; }
    public float Rating { get; private set; }

    public User(string name)
    {
        Name = name;
        Rating = 0;
    }

#pragma warning disable CS8618
    private User()
    {
        /* EF */
    }
#pragma warning restore CS8618
}
