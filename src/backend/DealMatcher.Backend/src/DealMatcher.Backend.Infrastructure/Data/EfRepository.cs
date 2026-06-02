namespace DealMatcher.Backend.Infrastructure.Data;

public class EfRepository<T>(AppDbContext dbContext) :
    RepositoryBase<T>(dbContext), IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
}
