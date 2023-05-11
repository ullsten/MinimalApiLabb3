using static MinimalApiLabb3.Program;

namespace MinimalApiLabb3.Data
{
    public static class SeedData
    {
        public static void Initialize(Labb3MinmalContext context)
        {
            if (!context.Persons.Any())
            {
                var persons = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe", PhoneNumber = "123-456-7890", Interests = new List<Interest>() },
                new Person { FirstName = "Jane", LastName = "Smith", PhoneNumber = "555-555-5555", Interests = new List<Interest>() },
                new Person { FirstName = "Bob", LastName = "Johnson", PhoneNumber = "111-111-1111", Interests = new List<Interest>() },
                new Person { FirstName = "Sarah", LastName = "Williams", PhoneNumber = "222-222-2222", Interests = new List<Interest>() },
                new Person { FirstName = "Tom", LastName = "Brown", PhoneNumber = "333-333-3333", Interests = new List<Interest>() }
            };

                context.Persons.AddRange(persons);
                context.SaveChanges();
            }

            if (!context.Interests.Any())
            {
                var interests = new List<Interest>
            {
                new Interest { InterestTitle = "Sports", InterestDescription = "Playing and watching sports.", Created = DateTime.Now, Links = new List<Link>(), FK_PersonId = 1 },
                new Interest { InterestTitle = "Music", InterestDescription = "Listening to and playing music.", Created = DateTime.Now, Links = new List<Link>(), FK_PersonId = 2 },
                new Interest { InterestTitle = "Reading", InterestDescription = "Reading books and articles.", Created = DateTime.Now, Links = new List<Link>(), FK_PersonId = 3 },
                new Interest { InterestTitle = "Traveling", InterestDescription = "Exploring new places and cultures.", Created = DateTime.Now, Links = new List<Link>(), FK_PersonId = 4 },
                new Interest { InterestTitle = "Cooking", InterestDescription = "Cooking and trying out new recipes.", Created = DateTime.Now, Links = new List<Link>(), FK_PersonId = 5 }
            };

                context.Interests.AddRange(interests);
                context.SaveChanges();
            }

            if (!context.Links.Any())
            {
                var links = new List<Link>
            {
                new Link { LinkTitle = "ESPN", URL = "http://espn.com", FK_InterestId = 1 },
                new Link { LinkTitle = "Spotify", URL = "http://spotify.com", FK_InterestId = 2 },
                new Link { LinkTitle = "The New York Times", URL = "http://nytimes.com", FK_InterestId = 3 },
                new Link { LinkTitle = "Lonely Planet", URL = "http://lonelyplanet.com", FK_InterestId = 4 },
                new Link { LinkTitle = "Food Network", URL = "http://foodnetwork.com", FK_InterestId = 5 }
            };

                context.Links.AddRange(links);
                context.SaveChanges();
            }
        }
    }



}
