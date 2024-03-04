using MediatR;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.PersonService.Commands.UpdatePerson;

public sealed record UpdatePersonCommand(
    int Id,
    string FirstName,
    string LastName,
    string PersonalId,
    DateTime BirthDate,
    int CityId,
    Gender Gender,
    IReadOnlyCollection<PhoneNumberModel> PhoneNumbers) : IRequest<PersonModel>, IRequest<Unit>;