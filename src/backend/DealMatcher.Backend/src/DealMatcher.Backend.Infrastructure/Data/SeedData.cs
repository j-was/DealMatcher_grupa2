namespace DealMatcher.Backend.Infrastructure.Data;

public static class SeedData
{

    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        if (!dbContext.Set<User>().Any())
        {
            await SeedUsers(dbContext);
        }

        if (!dbContext.Set<Offer>().Any())
        {
            await SeedOffers(dbContext);
        }
    }

    // version with less precise User, only to be added to the database
    public static async Task SeedUsers(AppDbContext dbContext)
    {
        var users = new List<User>
    {
        new("jan.kow@mm.com","Jan","Kowalski"),
        new("ann.n@h.pl","Anna","Nowak"),
        new("placeholder@xxnx.com","Piotr","Wiśniewski"),
    };

        await dbContext.Set<User>().AddRangeAsync(users);
        await dbContext.SaveChangesAsync();
    }
    public static async Task SeedOffers(AppDbContext dbContext)
    {

        var sellerIds = dbContext.Set<User>()
            .Select(u => u.Id)
            .ToList();

        var offers = new List<Offer>
        {
            new(
                title: "iPhone 13 Pro 256GB",
                description: "Sprzedam iPhone 13 Pro w kolorze grafitowym. Bez śladów użytkowania. Komplet z pudełkiem i ładowarką.",
                price: 2999.99m,
                imageUrls: ["https://placeholder.com/iphone.jpg"],
                sellerId: sellerIds[0],
                tags: ["elektronika", "telefon", "apple", "smartfon"],
                categoryId: 1,
                properties:
                [
                    new OfferProperty(1, "Stan", "Bardzo dobry"),
                    new OfferProperty(2, "Pamięć", "256GB"),
                    new OfferProperty(3, "Kolor", "Grafitowy"),
                ],
                availability: 1
            ),
            new(
                title: "Rower górski Trek Marlin 5",
                description: "Rower górski Trek Marlin 5, rocznik 2022. Przejechane około 500km, regularnie serwisowany.",
                price: 1800.00m,
                imageUrls: ["https://placeholder.com/rower.jpg"],
                sellerId: sellerIds[0],
                tags: ["sport", "rower", "górski"],
                categoryId: 2,
                properties:
                [
                    new OfferProperty(4, "Rozmiar ramy", "M"),
                    new OfferProperty(5, "Kolor", "Czarny"),
                    new OfferProperty(6, "Rok produkcji", "2022"),
                ],
                availability: 1
            ),
            new(
                title: "Sofa narożna szara",
                description: "Sofa narożna w kolorze szarym, wymiary 250x180cm. Zakupiona rok temu, używana sporadycznie.",
                price: 1200.00m,
                imageUrls: ["https://placeholder.com/sofa.jpg"],
                sellerId: sellerIds[1],
                tags: ["meble", "sofa", "dom"],
                categoryId: 3,
                properties:
                [
                    new OfferProperty(7, "Kolor", "Szary"),
                    new OfferProperty(8, "Wymiary", "250x180cm"),
                    new OfferProperty(9,"Stan", "Dobry"),
                ],
                availability: 1
            ),
            new(
                title: "Kurtka zimowa Nike rozmiar L",
                description: "Sprzedam kurtkę zimową Nike w rozmiarze L. Kolor czarny, noszona jeden sezon. Ciepła i lekka, idealna na zimę.",
                price: 249.99m,
                imageUrls: ["https://placeholder.com/kurtka.jpg"],
                sellerId: sellerIds[1],
                tags: ["odzież", "kurtka", "zima", "nike"],
                categoryId: 4,
                properties:
                [
                    new OfferProperty(10,"Rozmiar", "L"),
                    new OfferProperty(11,"Kolor", "Czarny"),
                    new OfferProperty(12,"Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new(
                title: "Laptop Dell XPS 15",
                description: "Sprzedam laptopa Dell XPS 15 z procesorem Intel i7, 16GB RAM, dysk SSD 512GB. Używany do pracy biurowej, stan bardzo dobry.",
                price: 4500.00m,
                imageUrls: ["https://placeholder.com/laptop.jpg"],
                sellerId: sellerIds[2],
                tags: ["elektronika", "laptop", "dell", "komputer"],
                categoryId: 1,
                properties:
                [
                    new OfferProperty(13,"Procesor", "Intel i7"),
                    new OfferProperty(14,"RAM", "16GB"),
                    new OfferProperty(15,"Dysk", "512GB SSD"),
                    new OfferProperty(16,"Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
        };

        await dbContext.Set<Offer>().AddRangeAsync(offers);
        _ = await dbContext.SaveChangesAsync();
    }
}
