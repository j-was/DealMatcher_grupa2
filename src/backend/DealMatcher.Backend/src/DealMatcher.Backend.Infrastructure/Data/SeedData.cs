

using DealMatcher.Backend.Core.Aggregates.Payment;

namespace DealMatcher.Backend.Infrastructure.Data;

public static class SeedData
{
    private const string DefaultImageUrl = "https://dealmatcherstorage.blob.core.windows.net/pictures/ab67616d0000b2735575dc147c4b0fb4ae041c90.jpg";

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

        if (!dbContext.Set<PaymentMethod>().Any())
        {
            await SeedPaymentMethods(dbContext);
        }

        if (!dbContext.Set<DeliveryMethod>().Any())
        {
            await SeedDeliveryMethods(dbContext);
        }
    }

    public static async Task SeedPaymentMethods(AppDbContext dbContext)
    {
        var paymentMethods = new List<PaymentMethod>
    {
        new(
            "blik",
            "BLIK",
            "Polski Standard Płatności",
            DefaultImageUrl
        ),
        new(
            "card",
            "Karta płatnicza",
            "Visa / Mastercard / American Express",
            DefaultImageUrl
        ),
        new(
            "transfer",
            "Przelew bankowy",
            "Szybki przelew online",
            DefaultImageUrl
        ),
    };

        await dbContext.Set<PaymentMethod>().AddRangeAsync(paymentMethods);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedDeliveryMethods(AppDbContext dbContext)
    {
        var deliveryMethods = new List<DeliveryMethod>
    {
        new(
            "parcel_locker",
            "Paczkomat",
            "Dostawa do paczkomatu InPost",
            12.99m,
            2
        ),
        new(
            "courier",
            "Kurier",
            "Dostawa kurierem pod wskazany adres",
            19.99m,
            1
        ),
    };

        await dbContext.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedCategoriesAndCategoryProperties(AppDbContext dbContext)
    {
        var categories = new List<Category>
        {
            new(
                "Inna",
                "Inne produkty",
                []
            ),

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
                    new("Rozmiar ramy", CategoryPropertyType.Text, null),
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
                    new("Kolor", CategoryPropertyType.Select,
                    [
                        "Biały",
                        "Czarny",
                        "Brązowy",
                        "Szary",
                        "Inny"
                    ]),
                    new("Wymiary", CategoryPropertyType.Text, null),
                    new("Stan", CategoryPropertyType.Select,
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
                    new("Rozmiar", CategoryPropertyType.Text, null),
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

            new(
                "Książki",
                "Książki, podręczniki i komiksy",
                [
                    new("Autor", CategoryPropertyType.Text, null),
                    new("Wydawnictwo", CategoryPropertyType.Text, null),
                    new("Rok wydania", CategoryPropertyType.Number, null),
                    new("Stan", CategoryPropertyType.Select,
                    [
                        "Nowa",
                        "Używana",
                        "Kolekcjonerska"
                    ]),
                ]
            ),

            new(
                "Motoryzacja",
                "Samochody, motocykle i części",
                [
                    new("Marka", CategoryPropertyType.Text, null),
                    new("Model", CategoryPropertyType.Text, null),
                    new("Rok produkcji", CategoryPropertyType.Number, null),
                    new("Przebieg", CategoryPropertyType.Number, null),
                    new("Stan", CategoryPropertyType.Select,
                    [
                        "Nowy",
                        "Używany",
                        "Uszkodzony"
                    ]),
                ]
            ),

            new(
                "Nieruchomości",
                "Mieszkania, domy, działki",
                [
                    new("Powierzchnia", CategoryPropertyType.Number, null),
                    new("Liczba pokoi", CategoryPropertyType.Number, null),
                    new("Piętro", CategoryPropertyType.Number, null),
                    new("Typ", CategoryPropertyType.Select,
                    [
                        "Sprzedaż",
                        "Wynajem"
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
            new("jan.kowalski@email.com", "Jan", "Kowalski"),
            new("anna.nowak@email.com", "Anna", "Nowak"),
            new("piotr.wisniewski@email.com", "Piotr", "Wiśniewski"),
            new("katarzyna.dabrowska@email.com", "Katarzyna", "Dąbrowska"),
            new("michal.lewandowski@email.com", "Michał", "Lewandowski"),
            new("magdalena.wojcik@email.com", "Magdalena", "Wójcik"),
            new("tomasz.kaminski@email.com", "Tomasz", "Kamiński"),
            new("aleksandra.kowalczyk@email.com", "Aleksandra", "Kowalczyk"),
            new("krzysztof.zielinski@email.com", "Krzysztof", "Zieliński"),
            new("joanna.szymanska@email.com", "Joanna", "Szymańska"),
        };

        await dbContext.Set<User>().AddRangeAsync(users);
        await dbContext.SaveChangesAsync();
    }

    public static async Task SeedOffers(AppDbContext dbContext)
    {
        var sellerIds = dbContext.Set<User>()
            .Select(u => u.Id)
            .ToList();

        // Create a dictionary for category lookup by name
        var categories = await dbContext.Set<Category>()
            .ToDictionaryAsync(c => c.Name, c => c.Id);

        var offers = new List<Offer>();

        // Helper function to safely get category ID
        int GetCategoryId(string categoryName) =>
            categories.TryGetValue(categoryName, out var id) ? id : categories["Inna"];

        // Elektronika offers
        offers.AddRange(
        [
            new Offer(
                title: "iPhone 13 Pro 256GB",
                description: "Sprzedam iPhone 13 Pro w kolorze grafitowym. Bez śladów użytkowania. Komplet z pudełkiem i ładowarką.",
                price: 2999.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[0],
                tags: ["elektronika", "telefon", "apple", "smartfon"],
                categoryId: GetCategoryId("Elektronika"),
                properties:
                [
                    new OfferProperty("Stan", "Bardzo dobry"),
                    new OfferProperty("Pamięć", "256"),
                    new OfferProperty("Kolor", "Grafitowy"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Laptop Dell XPS 15",
                description: "Sprzedam laptopa Dell XPS 15 z procesorem Intel i7, 16GB RAM, dysk SSD 512GB. Używany do pracy biurowej, stan bardzo dobry.",
                price: 4500.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[2],
                tags: ["elektronika", "laptop", "dell", "komputer"],
                categoryId: GetCategoryId("Elektronika"),
                properties:
                [
                    new OfferProperty("Stan", "Bardzo dobry"),
                    new OfferProperty("Pamięć", "512"),
                    new OfferProperty("Kolor", "Srebrny"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Samsung Galaxy S22 Ultra",
                description: "Sprzedam Samsung Galaxy S22 Ultra 512GB. Telefon w idealnym stanie, używany 3 miesiące. Gwarancja do końca roku.",
                price: 3499.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[3],
                tags: ["elektronika", "telefon", "samsung", "smartfon"],
                categoryId: GetCategoryId("Elektronika"),
                properties:
                [
                    new OfferProperty("Stan", "Nowy"),
                    new OfferProperty("Pamięć", "512"),
                    new OfferProperty("Kolor", "Czarny"),
                ],
                availability: 2
            ),
            new Offer(
                title: "Słuchawki Sony WH-1000XM4",
                description: "Słuchawki Sony z redukcją szumów. Używane przez rok, w bardzo dobrym stanie. W komplecie etui i kabel.",
                price: 699.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[5],
                tags: ["elektronika", "słuchawki", "sony", "audio"],
                categoryId: GetCategoryId("Elektronika"),
                properties:
                [
                    new OfferProperty("Stan", "Bardzo dobry"),
                    new OfferProperty("Kolor", "Czarny"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Tablet iPad Air 5 generacji",
                description: "iPad Air 5 z procesorem M1, 64GB WiFi. Stan idealny, używany okazjonalnie. Szkło hartowane od nowości.",
                price: 2199.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[7],
                tags: ["elektronika", "tablet", "apple", "ipad"],
                categoryId: GetCategoryId("Elektronika"),
                properties:
                [
                    new OfferProperty("Stan", "Bardzo dobry"),
                    new OfferProperty("Pamięć", "64"),
                    new OfferProperty("Kolor", "Szary"),
                ],
                availability: 1
            ),
        ]);

        // Sport offers
        offers.AddRange(
        [
            new Offer(
                title: "Rower górski Trek Marlin 5",
                description: "Rower górski Trek Marlin 5, rocznik 2022. Przejechane około 500km, regularnie serwisowany.",
                price: 1800.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[0],
                tags: ["sport", "rower", "górski", "trek"],
                categoryId: GetCategoryId("Sport"),
                properties:
                [
                    new OfferProperty("Rozmiar ramy", "M"),
                    new OfferProperty("Kolor", "Czarny"),
                    new OfferProperty("Rok produkcji", "2022"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Hantle regulowane 2x15kg",
                description: "Komplet hantli regulowanych 2x15kg z uchwytami. Stan bardzo dobry, używane w domu.",
                price: 249.99m,
                imageUrls: [DefaultImageUrl],
                sellerId: sellerIds[1],
                tags: ["sport", "siłownia", "hantle", "fitness"],
                categoryId: GetCategoryId("Sport"),
                properties:
                [
                    new OfferProperty("Kolor", "Czarny"),
                    new OfferProperty("Rok produkcji", "2023"),
                ],
                availability: 3
            ),
            new Offer(
                title: "Narty Atomic Redster X5",
                description: "Narty Atomic Redster X5 z wiązaniami. Długość 170cm. Używane 2 sezony, regularnie serwisowane.",
                price: 899.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[4],
                tags: ["sport", "narty", "zima", "atomic"],
                categoryId: GetCategoryId("Sport"),
                properties:
                [
                    new OfferProperty("Kolor", "Czerwony"),
                    new OfferProperty("Rok produkcji", "2021"),
                ],
                availability: 1
            ),
        ]);

        // Meble offers
        offers.AddRange(
        [
            new Offer(
                title: "Sofa narożna szara",
                description: "Sofa narożna w kolorze szarym, wymiary 250x180cm. Zakupiona rok temu, używana sporadycznie.",
                price: 1200.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[1],
                tags: ["meble", "sofa", "dom", "salon"],
                categoryId: GetCategoryId("Meble"),
                properties:
                [
                    new OfferProperty("Kolor", "Szary"),
                    new OfferProperty("Wymiary", "250x180x90"),
                    new OfferProperty("Stan", "Dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Biurko regulowane elektrycznie",
                description: "Biurko z regulacją wysokości, blat 160x80cm, kolor biały. Silnik elektryczny, pamięć 3 pozycji.",
                price: 899.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[6],
                tags: ["meble", "biurko", "home office", "regulowane"],
                categoryId: GetCategoryId("Meble"),
                properties:
                [
                    new OfferProperty("Kolor", "Biały"),
                    new OfferProperty("Wymiary", "160x80"),
                    new OfferProperty("Stan", "Nowy"),
                ],
                availability: 2
            ),
            new Offer(
                title: "Szafa przesuwna 3-drzwiowa",
                description: "Szafa przesuwna z lustrem, kolor biały. Wymiary 200x220x60cm. Do demontażu i odbioru własnego.",
                price: 750.00m,
                imageUrls: [DefaultImageUrl],
                sellerId: sellerIds[2],
                tags: ["meble", "szafa", "sypialnia"],
                categoryId: GetCategoryId("Meble"),
                properties:
                [
                    new OfferProperty("Kolor", "Biały"),
                    new OfferProperty("Wymiary", "200x220x60"),
                    new OfferProperty("Stan", "Używany"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Stół rozkładany dębowy",
                description: "Stół z litego drewna dębowego, rozkładany do 240cm. Stan bardzo dobry, delikatne ślady użytkowania.",
                price: 1100.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[8],
                tags: ["meble", "stół", "jadalnia", "drewno"],
                categoryId: GetCategoryId("Meble"),
                properties:
                [
                    new OfferProperty("Kolor", "Brązowy"),
                    new OfferProperty("Wymiary", "160-240x90"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
        ]);

        // Odzież offers
        offers.AddRange(
        [
            new Offer(
                title: "Kurtka zimowa Nike rozmiar L",
                description: "Sprzedam kurtkę zimową Nike w rozmiarze L. Kolor czarny, noszona jeden sezon. Ciepła i lekka, idealna na zimę.",
                price: 249.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[1],
                tags: ["odzież", "kurtka", "zima", "nike"],
                categoryId: GetCategoryId("Odzież"),
                properties:
                [
                    new OfferProperty("Rozmiar", "L"),
                    new OfferProperty("Kolor", "Czarny"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Sukienka wieczorowa rozmiar M",
                description: "Sukienka wieczorowa, kolor czerwony. Noszona raz na wesele. Marka: Reserved.",
                price: 89.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[3],
                tags: ["odzież", "sukienka", "wieczorowa", "damskie"],
                categoryId: GetCategoryId("Odzież"),
                properties:
                [
                    new OfferProperty("Rozmiar", "M"),
                    new OfferProperty("Kolor", "Czerwony"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Garnitur męski slim fit rozmiar 52",
                description: "Garnitur męski, granatowy, slim fit. Marka: Bytom. Noszony kilka razy, stan idealny.",
                price: 399.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[5],
                tags: ["odzież", "garnitur", "meskie", "eleganckie"],
                categoryId: GetCategoryId("Odzież"),
                properties:
                [
                    new OfferProperty("Rozmiar", "52"),
                    new OfferProperty("Kolor", "Niebieski"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Buty sportowe Adidas Ultraboost rozmiar 42",
                description: "Buty Adidas Ultraboost, kolor czarny. Używane przez 2 miesiące, w bardzo dobrym stanie.",
                price: 199.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[9],
                tags: ["odzież", "buty", "adidas", "sportowe"],
                categoryId: GetCategoryId("Odzież"),
                properties:
                [
                    new OfferProperty("Rozmiar", "42"),
                    new OfferProperty("Kolor", "Czarny"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
        ]);

        // Książki offers
        offers.AddRange(
        [
            new Offer(
                title: "Czysty kod - Robert C. Martin",
                description: "Książka w stanie idealnym, czytana raz. Wydanie polskie.",
                price: 49.99m,
                imageUrls: [DefaultImageUrl],
                sellerId: sellerIds[2],
                tags: ["książki", "programowanie", "informatyka"],
                categoryId: GetCategoryId("Książki"),
                properties:
                [
                    new OfferProperty("Autor", "Robert C. Martin"),
                    new OfferProperty("Wydawnictwo", "Helion"),
                    new OfferProperty("Rok wydania", "2021"),
                    new OfferProperty("Stan", "Bardzo dobra"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Wiedźmin - zestaw 8 książek",
                description: "Kompletna seria Wiedźmin Andrzeja Sapkowskiego. Stan bardzo dobry, niektóre tomy nieczytane.",
                price: 199.99m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[4],
                tags: ["książki", "fantasy", "wiedźmin", "sapkowski"],
                categoryId: GetCategoryId("Książki"),
                properties:
                [
                    new OfferProperty("Autor", "Andrzej Sapkowski"),
                    new OfferProperty("Wydawnictwo", "superNOWA"),
                    new OfferProperty("Stan", "Bardzo dobra"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Harry Potter - kolekcja ilustrowana",
                description: "Pierwsze 4 tomy Harry'ego Pottera w wydaniu ilustrowanym. Stan idealny, nieczytane.",
                price: 299.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[7],
                tags: ["książki", "harry potter", "fantasy", "kolekcja"],
                categoryId: GetCategoryId("Książki"),
                properties:
                [
                    new OfferProperty("Autor", "J.K. Rowling"),
                    new OfferProperty("Wydawnictwo", "Media Rodzina"),
                    new OfferProperty("Stan", "Nowa"),
                ],
                availability: 1
            ),
        ]);

        // Motoryzacja offers
        offers.AddRange(
        [
            new Offer(
                title: "Opony zimowe 205/55 R16 komplet",
                description: "Komplet 4 opon zimowych Michelin. Bieżnik 6mm, używane jeden sezon.",
                price: 599.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[0],
                tags: ["motoryzacja", "opony", "zimowe", "michelin"],
                categoryId: GetCategoryId("Motoryzacja"),
                properties:
                [
                    new OfferProperty("Marka", "Michelin"),
                    new OfferProperty("Model", "Alpin 6"),
                    new OfferProperty("Rok produkcji", "2023"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Fotelik samochodowy dla dziecka 9-36kg",
                description: "Fotelik samochodowy marki Britax Römer. Używany 2 lata, stan bardzo dobry. Posiada certyfikat bezpieczeństwa.",
                price: 249.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[3],
                tags: ["motoryzacja", "fotelik", "dziecko"],
                categoryId: GetCategoryId("Motoryzacja"),
                properties:
                [
                    new OfferProperty("Marka", "Britax Römer"),
                    new OfferProperty("Model", "Kidfix XP"),
                    new OfferProperty("Rok produkcji", "2022"),
                    new OfferProperty("Stan", "Bardzo dobry"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Uchwyt na rowery na hak holowniczy",
                description: "Uchwyt na 3 rowery, marka Thule. Używany 3 razy, stan idealny. W komplecie klucze i instrukcja.",
                price: 349.99m,
                imageUrls: [DefaultImageUrl],
                sellerId: sellerIds[6],
                tags: ["motoryzacja", "rower", "bagażnik", "thule"],
                categoryId: GetCategoryId("Motoryzacja"),
                properties:
                [
                    new OfferProperty("Marka", "Thule"),
                    new OfferProperty("Model", "EasyFold XT 3"),
                    new OfferProperty("Rok produkcji", "2023"),
                    new OfferProperty("Stan", "Nowy"),
                ],
                availability: 1
            ),
        ]);

        // Nieruchomości offers
        offers.AddRange(
        [
            new Offer(
                title: "Mieszkanie 3-pokojowe Kraków",
                description: "Przestronne mieszkanie 65m2 w Krakowie. 3 pokoje, kuchnia, łazienka. Balkon, piwnica, miejsce parkingowe.",
                price: 589000.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[1],
                tags: ["nieruchomości", "mieszkanie", "kraków", "sprzedaż"],
                categoryId: GetCategoryId("Nieruchomości"),
                properties:
                [
                    new OfferProperty("Powierzchnia", "65"),
                    new OfferProperty("Liczba pokoi", "3"),
                    new OfferProperty("Piętro", "2"),
                    new OfferProperty("Typ", "Sprzedaż"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Mieszkanie 2-pokojowe do wynajęcia Warszawa",
                description: "Mieszkanie 45m2 w centrum Warszawy. 2 pokoje, kuchnia, łazienka. Umeblowane, gotowe do zamieszkania.",
                price: 3200.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[5],
                tags: ["nieruchomości", "mieszkanie", "warszawa", "wynajem"],
                categoryId: GetCategoryId("Nieruchomości"),
                properties:
                [
                    new OfferProperty("Powierzchnia", "45"),
                    new OfferProperty("Liczba pokoi", "2"),
                    new OfferProperty("Piętro", "4"),
                    new OfferProperty("Typ", "Wynajem"),
                ],
                availability: 1
            ),
            new Offer(
                title: "Działka budowlana 800m2 pod Warszawą",
                description: "Działka budowlana 800m2 w miejscowości 20km od Warszawy. Media na granicy działki, piękna okolica.",
                price: 199000.00m,
                imageUrls: [DefaultImageUrl, DefaultImageUrl],
                sellerId: sellerIds[8],
                tags: ["nieruchomości", "działka", "budowlana", "mazowieckie"],
                categoryId: GetCategoryId("Nieruchomości"),
                properties:
                [
                    new OfferProperty("Powierzchnia", "800"),
                    new OfferProperty("Typ", "Sprzedaż"),
                ],
                availability: 1
            ),
        ]);

        await dbContext.Set<Offer>().AddRangeAsync(offers);
        await dbContext.SaveChangesAsync();
    }
}
