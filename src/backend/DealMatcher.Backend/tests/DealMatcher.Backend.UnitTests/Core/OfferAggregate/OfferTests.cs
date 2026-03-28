namespace DealMatcher.Backend.UnitTests.Core.OfferAggregate;

public class OfferTests
{
    private static List<OfferProperty> TestProperties =>
        [new OfferProperty("Color", "Red")];

    private static List<string> TestImages =>
        ["https://test.com/a.jpg"];

    private static List<string> TestTags =>
        ["Electronics"];

    [Fact]
    public void Constructor_Should_Create_Offer_With_Valid_Data()
    {
        var offer = new Offer(
            "Test Product",
            "Some description",
            10m,
            TestImages,
            1,
            TestTags,
            1,
            TestProperties,
            5);

        offer.Title.ShouldBe("Test Product");
        offer.Description.ShouldBe("Some description");
        offer.Price.ShouldBe(10m);
        offer.ImageUrls.Count.ShouldBe(1);
        offer.Tags.Count.ShouldBe(1);
        offer.CategoryId.ShouldBe(1);
        offer.Availability.ShouldBe(5);
        offer.Status.ShouldBe(OfferStatus.Draft);
        offer.Properties.Count.ShouldBe(1);
        offer.UpdatedAt.ShouldBeLessThanOrEqualTo(DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    public void Constructor_Should_Throw_When_Title_Is_Invalid(string invalidTitle)
    {
        Should.Throw<ArgumentException>(() =>
            new Offer(
                invalidTitle,
                "desc",
                1m,
                TestImages,
                1,
                TestTags,
                1,
                TestProperties,
                1));
    }

    [Theory]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_Price_Is_Negative(decimal invalidPrice)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Offer(
                "title",
                "desc",
                invalidPrice,
                TestImages,
                1,
                TestTags,
                1,
                TestProperties,
                1));
    }

    [Theory]
    [InlineData(-1)]
    public void Constructor_Should_Throw_When_Availability_Is_Negative(int invalidaAvail)
    {
        Should.Throw<ArgumentOutOfRangeException>(() =>
            new Offer(
                "title",
                "desc",
                1m,
                TestImages,
                1,
                TestTags,
                1,
                TestProperties,
                invalidaAvail));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Image_Limit_Exceeded()
    {
        var images = Enumerable.Repeat("x", OfferConstants.MaxImagesCount + 1)
            .ToList();

        Should.Throw<ArgumentException>(() =>
            new Offer(
                "title",
                "desc",
                1m,
                images,
                1,
                TestTags,
                1,
                TestProperties,
                1));
    }

    [Fact]
    public void Constructor_Should_Throw_When_Tag_Limit_Exceeded()
    {
        var tags = Enumerable.Repeat("tag", OfferConstants.MaxTagsCount + 1)
            .ToList();

        Should.Throw<ArgumentException>(() =>
            new Offer(
                "title",
                "desc",
                1m,
                TestImages,
                1,
                tags,
                1,
                TestProperties,
                1));
    }

    [Fact]
    public void ChangeStatus_Should_Update_Status_And_Timestamp()
    {
        var offer = new Offer(
            "title",
            "desc",
            1m,
            TestImages,
            1,
            TestTags,
            1,
            TestProperties,
            1);

        var before = offer.UpdatedAt;
        System.Threading.Thread.Sleep(10);

        offer.ChangeStatus(OfferStatus.Sold);

        offer.Status.ShouldBe(OfferStatus.Sold);
        offer.UpdatedAt.ShouldBeGreaterThan(before);
    }

    [Fact]
    public void Constructor_Should_Allow_Null_Tags_And_Properties()
    {
        var offer = new Offer(
            "title",
            "desc",
            1m,
            TestImages,
            1,
            null,
            1,
            null,
            1);

        offer.Tags.ShouldNotBeNull();
        offer.Tags.Count.ShouldBe(0);

        offer.Properties.ShouldNotBeNull();
        offer.Properties.Count.ShouldBe(0);
    }
}

