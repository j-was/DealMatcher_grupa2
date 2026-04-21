using DealMatcher.Backend.Infrastructure.Authentication;

namespace DealMatcher.Backend.Infrastructure.Data;

public static class SeedData
{

    public static async Task InitializeAsync(AppDbContext dbContext)
    {
        if (!dbContext.Set<Category>().Any())
        {
            await SeedCategoriesAndCategoryProperties(dbContext);
        }
        if (!dbContext.Set<User>().Any())
        {
            await SeedUsers(dbContext);
        }

        if (!dbContext.Set<Offer>().Any())
        {
            await SeedOffers(dbContext);
        }
    }
    public static async Task SeedCategoriesAndCategoryProperties(AppDbContext dbContext)
    {
        var categories = new List<Category>
    {
        new(
            "Elektronika",
            "Urządzenia elektroniczne i akcesoria",
            [
                new("Stan", CategoryPropertyType.Select,
                [
                    "Nowy",
                    "Używany",
                    "Uszkodzony"
                ]),
                new("Pamięć", CategoryPropertyType.Number, null),
                new("Kolor", CategoryPropertyType.Select,
                [
                    "Czarny",
                    "Biały",
                    "Srebrny",
                    "Szary",
                    "Inny"
                ]),
            ]
        ),

        new(
            "Sport",
            "Sprzęt i akcesoria sportowe",
            [
                new( "Rozmiar ramy", CategoryPropertyType.Text, null),
                new("Kolor", CategoryPropertyType.Select,
                [
                    "Czarny",
                    "Biały",
                    "Czerwony",
                    "Niebieski",
                    "Inny"
                ]),
                new("Rok produkcji", CategoryPropertyType.Number, null),
            ]
        ),

        new(
            "Meble",
            "Meble do domu i biura",
            [
                new( "Kolor", CategoryPropertyType.Select,
                [
                    "Biały",
                    "Czarny",
                    "Brązowy",
                    "Szary",
                    "Inny"
                ]),
                new("Wymiary", CategoryPropertyType.Text, null),
                new( "Stan", CategoryPropertyType.Select,
                [
                    "Nowy",
                    "Używany",
                    "Do renowacji"
                ]),
            ]
        ),

        new(
            "Odzież",
            "Odzież damska, męska i dziecięca",
            [
                new( "Rozmiar", CategoryPropertyType.Text, null),
                new("Kolor", CategoryPropertyType.Select,
                [
                    "Czarny",
                    "Biały",
                    "Niebieski",
                    "Czerwony",
                    "Inny"
                ]),
                new("Stan", CategoryPropertyType.Select,
                [
                    "Nowy",
                    "Używany",
                    "Uszkodzony"
                ]),
            ]
        ),
    };

        await dbContext.Set<Category>().AddRangeAsync(categories);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedUsers(AppDbContext dbContext)
    {
        var users = new List<User>
    {
        new("jan.kow@mm.com","Jan","Kowalski"),
        new("ann.n@h.pl","Anna","Nowak"),
        new("placeholder@xxnx.com","Piotr","Wiśniewski")
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
                imageUrls: ["https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg"],
                sellerId: sellerIds[0],
                tags: ["elektronika", "telefon", "apple", "smartfon"],
                categoryId: 1,
                properties:
                [
                    new OfferProperty( "Stan", "Bardzo dobry"),
                    new OfferProperty( "Pamięć", "256GB"),
                    new OfferProperty( "Kolor", "Grafitowy"),
                ],
                availability: 1
            ),
            new(
                title: "Rower górski Trek Marlin 5",
                description: "Rower górski Trek Marlin 5, rocznik 2022. Przejechane około 500km, regularnie serwisowany.",
                price: 1800.00m,
                imageUrls: ["https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg"],
                sellerId: sellerIds[0],
                tags: ["sport", "rower", "górski"],
                categoryId: 2,
                properties:
                [
                    new OfferProperty("Rozmiar ramy", "M"),
                    new OfferProperty( "Kolor", "Czarny"),
                    new OfferProperty( "Rok produkcji", "2022"),
                ],
                availability: 1
            ),
            new(
                title: "Sofa narożna szara",
                description: "Sofa narożna w kolorze szarym, wymiary 250x180cm. Zakupiona rok temu, używana sporadycznie.",
                price: 1200.00m,
                imageUrls: ["https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg"],
                sellerId: sellerIds[1],
                tags: ["meble", "sofa", "dom"],
                categoryId: 3,
                properties:
                [
                    new OfferProperty( "Kolor", "Szary"),
                    new OfferProperty("Wymiary", "250x180cm"),
                    new OfferProperty("Stan", "Dobry"),
                ],
                availability: 1
            ),
            new(
                title: "Kurtka zimowa Nike rozmiar L",
                description: "Sprzedam kurtkę zimową Nike w rozmiarze L. Kolor czarny, noszona jeden sezon. Ciepła i lekka, idealna na zimę.",
                price: 249.99m,
                imageUrls: ["https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg"],
                sellerId: sellerIds[1],
                tags: ["odzież", "kurtka", "zima", "nike"],
                categoryId: 4,
                properties:
                [
                    new OfferProperty("Rozmiar", "L"),
                    new OfferProperty("Kolor", "Czarny"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new(
                title: "Laptop Dell XPS 15",
                description: "Sprzedam laptopa Dell XPS 15 z procesorem Intel i7, 16GB RAM, dysk SSD 512GB. Używany do pracy biurowej, stan bardzo dobry.",
                price: 4500.00m,
                imageUrls: ["https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg"],
                sellerId: sellerIds[2],
                tags: ["elektronika", "laptop", "dell", "komputer"],
                categoryId: 1,
                properties:
                [
                    new OfferProperty("Procesor", "Intel i7"),
                    new OfferProperty("RAM", "16GB"),
                    new OfferProperty("Dysk", "512GB SSD"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
        };

        await dbContext.Set<Offer>().AddRangeAsync(offers);
        _ = await dbContext.SaveChangesAsync();
    }
}
