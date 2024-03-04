using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Persistence
{
    public class DbInitializer
    {
        public async Task Seed(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var _dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
            await _dbContext.Database.MigrateAsync();

            var persons = PersonsData();

            var existingIds = await _dbContext.Persons.Select(p => p.PersonalId).ToListAsync(cancellationToken);
            var personsToAdd = persons.Where(p => !existingIds.Contains(p.PersonalId)).ToList();

            if (personsToAdd.Any())
            {
                await _dbContext.Persons.AddRangeAsync(personsToAdd);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }

            var citiesList = await _dbContext.Cities.ToListAsync(cancellationToken);

            if (!citiesList.Any())
            {
                using var jsonStream = File.OpenRead("Cities.json");

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var result = await JsonSerializer.DeserializeAsync<CityListModel>(jsonStream, options);

                result.Cities.ForEach(x => x.SetCreateDate());

                await _dbContext.Cities.AddRangeAsync(result.Cities, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }

        private static IEnumerable<Person> PersonsData()
        {
            var persons = new List<Person>
            {
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Giorgi",
                    LastName = "Giorgadze",
                    PersonalId = "01987351321",
                    BirthDate = new DateTime(1985, 10, 15),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-123-4567", NumberType = PhoneNumberType.Office },
                        new PhoneNumber { Number = "555-987-6543", NumberType = PhoneNumberType.Home },
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Ana",
                    LastName = "Javakhishvili",
                    PersonalId = "01987654322",
                    BirthDate = new DateTime(1990, 5, 25),
                    CityId = 2,
                    Gender = Gender.Female,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-555-5555" }
                    }
                },
                new Person
                {
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now,
                    FirstName = "Sandro",
                    LastName = "Nadiradze",
                    PersonalId = "01987354321",
                    BirthDate = new DateTime(1982, 8, 8),
                    CityId = 1,
                    Gender = Gender.Male,
                    PhoneNumbers = new List<PhoneNumber>
                    {
                        new PhoneNumber { Number = "555-888-8888" }
                    }
                },
            };

            var relations = new List<PersonRelation>
            {
                new PersonRelation(persons[0], persons[1], RelationType.Colleague),
                new PersonRelation(persons[0], persons[1], RelationType.Familiar),
                new PersonRelation(persons[0], persons[1], RelationType.Colleague)
            };

            foreach (var relation in relations)
            {
                var person = relation.Person;
                var relatedPerson = relation.RelatedPerson;

                person.RelatedPersons.Add(relation);
                relatedPerson.RelatedToPersons.Add(relation);
            }

            return persons;
        }
    }
}