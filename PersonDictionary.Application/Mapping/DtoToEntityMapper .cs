using PersonDictionary.Application.Interface;
using PersonDictionary.Application.PersonService.Commands.CreatePerson;
using PersonDictionary.Domain.Entities;

namespace PersonDictionary.Application.Mapping
{
    public class DtoToEntityMapper : IDtoToEntityMapper
    {
        public Person MapToEntity(CreatePersonCommand command)
        {
            var person = new Person
            {
                CreatedDate = DateTime.Now,
                FirstName = command.FirstName,
                LastName = command.LastName,
                PersonalId = command.PersonalId,
                BirthDate = command.BirthDate,
                CityId = command.CityId,
                Gender = command.Gender,
                PhoneNumbers = command.PhoneNumbers.Select(phone => new PhoneNumber
                {
                    Number = phone.Number,
                    NumberType = phone.NumberType,
                    PersonId = command.Id
                }).ToList()
            };

            return person;
        }
    }
}
