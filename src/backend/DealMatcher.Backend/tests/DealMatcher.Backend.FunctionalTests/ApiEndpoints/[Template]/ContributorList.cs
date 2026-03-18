// using DealMatcher.Backend.Infrastructure.Data;
// using DealMatcher.Backend.Web.Contributors;
//
// namespace DealMatcher.Backend.FunctionalTests.ApiEndpoints;
//
// [Collection("Sequential")]
// public class ContributorList(CustomWebApplicationFactory<Program> factory) : IClassFixture<CustomWebApplicationFactory<Program>>
// {
//   private readonly HttpClient _client = factory.CreateClient();
//
//   [Fact]
//   public async Task ReturnsTwoContributors()
//   {
//     var result = await _client.GetAndDeserializeAsync<ContributorListResponse>("/Contributors");
//
//     result.Contributors.Count.ShouldBe(2);
//     result.Contributors.ShouldContain(contributor => contributor.Name == SeedData.Contributor1.Name);
//     result.Contributors.ShouldContain(contributor => contributor.Name == SeedData.Contributor2.Name);
//   }
// }
