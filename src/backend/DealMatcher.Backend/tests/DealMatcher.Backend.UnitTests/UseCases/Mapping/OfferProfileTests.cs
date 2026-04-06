namespace DealMatcher.Backend.UnitTests.UseCases.Mapping;

public class OfferProfileTests
{
    private readonly IMapper _mapper;

    public OfferProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<OfferProfile>();
            },
            new SerilogLoggerFactory());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        var configuration = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<OfferProfile>();
            },
            new SerilogLoggerFactory());

        configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_MapsBasicPropertiesCorrectly()
    {
        var offerEntity = CreateOfferEntity();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Id.ShouldBe(offerEntity.Id);
        dto.Title.ShouldBe(offerEntity.Title as string);
        dto.Description.ShouldBe(offerEntity.Description);
        dto.Price.ShouldBe((double)offerEntity.Price);
        dto.Availability.ShouldBe(offerEntity.Availability);
        dto.UpdatedAt.ShouldBe(offerEntity.UpdatedAt);
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_MapsImagesCorrectly()
    {
        var offerEntity = CreateOfferEntity();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Images.ShouldNotBeNull();
        dto.Images.ShouldBe(offerEntity.ImageUrls);
        dto.Images.Count.ShouldBe(1);
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_MapsStatusToUpperString()
    {
        var offerEntity = CreateOfferEntity();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Status.ShouldBe(OfferStatus.Draft.Value.ToUpper());
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_MapsAllStatusValuesCorrectly()
    {
        // Arrange
        var statuses = new[] { OfferStatus.Active, OfferStatus.Sold, OfferStatus.Deleted, OfferStatus.Promoted };
        var expectedStatuses = new[] { "ACTIVE", "SOLD", "DELETED", "PROMOTED" };

        for (var i = 0; i < statuses.Length; i++)
        {
            var offerEntity = CreateOfferEntity();
            offerEntity.ChangeStatus(statuses[i]);

            var dto = _mapper.Map<OfferDTO>(offerEntity);

            dto.Status.ShouldBe(expectedStatuses[i]);
        }
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_CreatesSellerDTOWithDefaultValues()
    {
        var offerEntity = CreateOfferEntity();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Seller.ShouldNotBeNull();
        dto.Seller.Id.ShouldBe(offerEntity.SellerId);
        dto.Seller.Name.ShouldBe(string.Empty);
        dto.Seller.Rating.ShouldBe(0f);
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_CreatesCategoryDTOWithDefaultValues()
    {
        var offerEntity = CreateOfferEntity();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Category.ShouldNotBeNull();
        dto.Category.Id.ShouldBe(offerEntity.CategoryId);
        dto.Category.Name.ShouldBe(string.Empty);
        dto.Category.Description.ShouldBe(string.Empty);
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_MapsPropertiesToDictionary()
    {
        var offerEntity = CreateOfferEntity();

        var expectedDictionary = new Dictionary<string, string>
        {
            { "Color", "Red" }
        };

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Properties.ShouldNotBeNull();
        dto.Properties.ShouldBe(expectedDictionary);
        dto.Properties.Count.ShouldBe(1);
    }

    [Fact]
    public void Map_OfferEntityToOfferDTO_WithEmptyProperties_MapsToEmptyDictionary()
    {
        var offerEntity = CreateOfferEntity();
        offerEntity.Properties.Clear();

        var dto = _mapper.Map<OfferDTO>(offerEntity);

        dto.Properties.ShouldNotBeNull();
        dto.Properties.ShouldBeEmpty();
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsBasicPropertiesCorrectly()
    {
        var offerEntity = CreateOfferEntity();
        var seller = CreateSellerEntity();
        var category = CreateCategoryEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, seller, category);

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Id.ShouldBe(offerEntity.Id);
        dto.Title.ShouldBe(offerEntity.Title);
        dto.Description.ShouldBe(offerEntity.Description);
        dto.Price.ShouldBe((double)offerEntity.Price);
        dto.Availability.ShouldBe(offerEntity.Availability);
        dto.UpdatedAt.ShouldBe(offerEntity.UpdatedAt);
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsImagesCorrectly()
    {
        var offerEntity = CreateOfferEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), CreateCategoryEntity());

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Images.ShouldNotBeNull();
        dto.Images.ShouldBe(offerEntity.ImageUrls);
        dto.Images.Count.ShouldBe(1);
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsStatusToUpperString()
    {
        var offerEntity = CreateOfferEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), CreateCategoryEntity());

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Status.ShouldBe("DRAFT");
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsSellerWithCompleteInformation()
    {
        var offerEntity = CreateOfferEntity();
        var seller = CreateSellerEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, seller, CreateCategoryEntity());

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Seller.ShouldNotBeNull();
        dto.Seller.Id.ShouldBe(seller.Id);
        dto.Seller.Name.ShouldBe(seller.Name);
        dto.Seller.Rating.ShouldBe(seller.Rating);
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsCategoryWithCompleteInformation()
    {
        var offerEntity = CreateOfferEntity();
        var category = CreateCategoryEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), category);

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Category.ShouldNotBeNull();
        dto.Category.Id.ShouldBe(category.Id);
        dto.Category.Name.ShouldBe(category.Name);
        dto.Category.Description.ShouldBe(category.Description);
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_MapsPropertiesToDictionary()
    {
        var offerEntity = CreateOfferEntity();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), CreateCategoryEntity());

        var expectedDictionary = new Dictionary<string, string>
        {
            { "Color", "Red" }
        };

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Properties.ShouldNotBeNull();
        dto.Properties.ShouldBe(expectedDictionary);
        dto.Properties.Count.ShouldBe(1);
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_WithEmptyProperties_MapsToEmptyDictionary()
    {
        var offerEntity = CreateOfferEntity();
        offerEntity.Properties.Clear();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), CreateCategoryEntity());

        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Properties.ShouldNotBeNull();
        dto.Properties.ShouldBeEmpty();
    }

    [Fact]
    public void Map_OfferInfoToOfferDTO_WithNullProperties_MapsToEmptyDictionary()
    {
        var offerEntity = CreateOfferEntity();
        offerEntity.Properties.Clear();
        var offerInfo = new OfferProfile.OfferInfo(offerEntity, CreateSellerEntity(), CreateCategoryEntity());


        var dto = _mapper.Map<OfferDTO>(offerInfo);

        dto.Properties.ShouldNotBeNull();
        dto.Properties.ShouldBeEmpty();
    }


    private static Offer CreateOfferEntity()
    {
        return new Offer(
            "Test Offer",
            "Test Description",
            99.99m,
            ["https://example.com/image.jpg"],
            1,
            ["tag1", "tag2"],
            1,
            [new(3, "Color", "Red")],
            10
        );
    }

    private static User CreateSellerEntity()
    {
        return new User
        ("John Doe"
        );
    }

    private static Category CreateCategoryEntity()
    {
        return new Category
        (
            "Electronics",
            "Electronic devices and accessories"
        );
    }
}
