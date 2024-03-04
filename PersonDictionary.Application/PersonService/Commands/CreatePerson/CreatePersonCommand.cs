using System.Text.Json.Serialization;
using MediatR;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.PersonService.Commands.CreatePerson
{
    public sealed record CreatePersonCommand(
        [property: JsonIgnore] int Id,
        string FirstName,
        string LastName,
        string PersonalId,
        DateTime BirthDate,
        int CityId,
        Gender Gender,
        IEnumerable<PhoneNumberModel> PhoneNumbers) : IRequest<PersonModel>;
}