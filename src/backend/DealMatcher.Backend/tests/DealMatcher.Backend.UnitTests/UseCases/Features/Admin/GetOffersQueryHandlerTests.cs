namespace DealMatcher.Backend.UnitTests.UseCases.Features.Admin;

public class GetOffersQueryHandlerTests
{
    private readonly IReadRepository<OfferEntity> _offersRepository;
    private readonly IReadRepository<CategoryEntity> _categoriesRepository;
    private readonly IReadRepository<UserEntity> _usersRepository;
    private readonly IMapper _mapper;
    private readonly GetOffersQueryHandler _handler;

    public GetOffersQueryHandlerTests()
    {
        _offersRepository = Substitute.For<IReadRepository<OfferEntity>>();
        _categoriesRepository = Substitute.For<IReadRepository<CategoryEntity>>();
        _usersRepository = Substitute.For<IReadRepository<UserEntity>>();
        _mapper = Substitute.For<IMapper>();
        _handler = new GetOffersQueryHandler(
            _offersRepository, _categoriesRepository, _usersRepository, _mapper);
    }

    // [Fact]
    // public async Task Handle_ShouldReturnSuccess_WithPagedOffers_WhenUserIsAdmin()
    // {
    //     var query = new GetOffersQuery(1, 10, null, 1);
    //     var adminUser = CreateAdminUser(1);
    //     var offers = new List<OfferEntity>
    //     {
    //         CreateOfferEntity(1, "Offer 1", 1),
    //         CreateOfferEntity(2, "Offer 2", 1)
    //     };
    //     var seller = new UserEntity("seller@example.com", "Seller", "User");
    //     typeof(UserEntity).GetProperty("Id")!.SetValue(seller, 1);
    //     var category = new Category("Electronics", "Electronic devices");
    //     typeof(CategoryEntity).GetProperty("Id")!.SetValue(category, 1);
    //
    //     _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
    //         .Returns(adminUser);
    //     _offersRepository.ListAsync(Arg.Any<CancellationToken>())
    //         .Returns(offers);
    //     _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
    //         .Returns(seller);
    //     _categoriesRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
    //         .Returns(category);
    //     _mapper.Map<OfferDTO>(Arg.Any<OfferProfile.OfferInfo>())
    //         .Returns(new OfferDTO(1, "Offer 1", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
    //             new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow),
    //         new OfferDTO(2, "Offer 2", "Desc", 200, [], new SellerDTO(1, "Seller"), [],
    //             new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow));
    //
    //     var result = await _handler.Handle(query, CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.ShouldNotBeNull();
    //     result.Value.Items.Count.ShouldBe(2);
    //     result.Value.Total.ShouldBe(2);
    //     result.Value.Page.ShouldBe(1);
    // }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserIsNotAdmin()
    {
        var query = new GetOffersQuery(1, 10, null, 1);
        var regularUser = new UserEntity("user@example.com", "John", "Doe");

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(regularUser);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }

    [Fact]
    public async Task Handle_ShouldReturnForbidden_WhenUserDoesNotExist()
    {
        var query = new GetOffersQuery(1, 10, null, 1);

        _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
            .Returns((UserEntity?)null);

        var result = await _handler.Handle(query, CancellationToken.None);

        result.IsSuccess.ShouldBeFalse();
        result.Status.ShouldBe(ResultStatus.Forbidden);
    }
    //
    // [Fact]
    // public async Task Handle_ShouldFilterByStatus()
    // {
    //     var query = new GetOffersQuery(1, 10, "ACTIVE", 1);
    //     var adminUser = CreateAdminUser(1);
    //     var draftOffer = CreateOfferEntity(1, "Draft Offer", 1);
    //     var activeOffer = CreateOfferEntity(2, "Active Offer", 1);
    //     typeof(OfferEntity).GetProperty("Status")!.SetValue(activeOffer, OfferStatus.Active);
    //     var offers = new List<OfferEntity> { draftOffer, activeOffer };
    //     var seller = new UserEntity("seller@example.com", "Seller", "User");
    //     var category = new Category("Electronics", "Electronic devices");
    //
    //     _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
    //         .Returns(adminUser);
    //     _offersRepository.ListAsync(Arg.Any<CancellationToken>())
    //         .Returns(offers);
    //     _usersRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
    //         .Returns(seller);
    //     _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
    //         .Returns(category);
    //     _mapper.Map<OfferDTO>(Arg.Any<OfferProfile.OfferInfo>())
    //         .Returns(new OfferDTO(2, "Active Offer", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
    //             new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow));
    //
    //     var result = await _handler.Handle(query, CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.Items.Count.ShouldBe(1);
    // }
    //
    // [Fact]
    // public async Task Handle_ShouldRespectPagination()
    // {
    //     var query = new GetOffersQuery(2, 5, null, 1);
    //     var adminUser = CreateAdminUser(1);
    //     var offers = Enumerable.Range(1, 12)
    //         .Select(i => CreateOfferEntity(i, $"Offer {i}", 1))
    //         .ToList();
    //     var seller = new UserEntity("seller@example.com", "Seller", "User");
    //     var category = new Category("Electronics", "Electronic devices");
    //
    //     _usersRepository.GetByIdAsync(1, Arg.Any<CancellationToken>())
    //         .Returns(adminUser);
    //     _offersRepository.ListAsync(Arg.Any<CancellationToken>())
    //         .Returns(offers);
    //     _usersRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
    //         .Returns(seller);
    //     _categoriesRepository.GetByIdAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
    //         .Returns(category);
    //     _mapper.Map<OfferDTO>(Arg.Any<OfferProfile.OfferInfo>())
    //         .Returns(new OfferDTO(1, "Offer", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
    //             new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow));
    //
    //     var result = await _handler.Handle(query, CancellationToken.None);
    //
    //     result.IsSuccess.ShouldBeTrue();
    //     result.Value.Page.ShouldBe(2);
    //     result.Value.Items.Count.ShouldBe(5);
    //     result.Value.Total.ShouldBe(12);
    // }

    // [Fact]
    // public async Task Handle_ShouldPassCancellationToken()
    // {
    //     var query = new GetOffersQuery(1, 10, null, 1);
    //     var cts = new CancellationTokenSource();
    //     var adminUser = CreateAdminUser(1);
    //     var offers = new List<OfferEntity>
    //     {
    //         CreateOfferEntity(1, "Offer 1", 1)
    //     };
    //     var seller = new UserEntity("seller@example.com", "Seller", "User");
    //     var category = new Category("Electronics", "Electronic devices");
    //
    //     _usersRepository.GetByIdAsync(1, cts.Token)
    //         .Returns(adminUser);
    //     _offersRepository.ListAsync(cts.Token)
    //         .Returns(offers);
    //     _usersRepository.GetByIdAsync(1, cts.Token)
    //         .Returns(seller);
    //     _categoriesRepository.GetByIdAsync(1, cts.Token)
    //         .Returns(category);
    //     _mapper.Map<OfferDTO>(Arg.Any<OfferProfile.OfferInfo>())
    //         .Returns(new OfferDTO(1, "Offer 1", "Desc", 100, [], new SellerDTO(1, "Seller"), [],
    //             new CategoryDTO(1, "Electronics", ""), [], 10, "DRAFT", DateTime.UtcNow, DateTime.UtcNow));
    //
    //     await _handler.Handle(query, cts.Token);
    //
    //     await _usersRepository.Received(1).GetByIdAsync(1, cts.Token);
    //     await _offersRepository.Received(1).ListAsync(cts.Token);
    // }

    private static UserEntity CreateAdminUser(int id)
    {
        var user = new UserEntity("admin@example.com", "Admin", "User");
        typeof(UserEntity).GetProperty("Status")!.SetValue(user, UserStatus.Admin);
        typeof(UserEntity).GetProperty("Id")!.SetValue(user, id);
        return user;
    }

    private static OfferEntity CreateOfferEntity(int id, string title, int sellerId)
    {
        var offer = new OfferEntity(title, "Description", 100m, ["image.jpg"], sellerId,
            [], 1, [new OfferProperty("Color", "Red")], 10);
        typeof(OfferEntity).GetProperty("Id")!.SetValue(offer, id);
        return offer;
    }
}
