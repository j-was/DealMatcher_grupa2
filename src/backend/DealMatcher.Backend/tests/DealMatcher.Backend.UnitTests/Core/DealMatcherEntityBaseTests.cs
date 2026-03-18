namespace DealMatcher.Backend.UnitTests.Core;

public class DealMatcherEntityBaseTests
{
    [Fact]
    public void DealMatcherEntityBase_WhenCreated_ShouldHaveCorrectCreatedAtDeletedAtAndIsDeletedSet()
    {

        var beforeCreationTime = DateTime.UtcNow;
        var entity = new DealMatcherEntityBase();
        var afterCreationTime = DateTime.UtcNow;

        entity.CreatedAt
          .ShouldBeInRange(beforeCreationTime, afterCreationTime);

        entity.DeletedAt
          .ShouldBeNull();

        entity.IsDeleted
          .ShouldBeFalse();
    }

    [Fact]
    public void DealMatcherEntityBase_WhenDeleted_ShouldHaveCorrectDeletedAtIsDeletedSet()
    {
        var entity = new DealMatcherEntityBase();

        var beforeDeletionTime = DateTime.UtcNow;

        entity.Delete();

        var afterDeletionTime = DateTime.UtcNow;

        entity.DeletedAt
          .ShouldNotBeNull()
          .ShouldBeInRange(beforeDeletionTime, afterDeletionTime);

        entity.IsDeleted
          .ShouldBeTrue();
    }
}
