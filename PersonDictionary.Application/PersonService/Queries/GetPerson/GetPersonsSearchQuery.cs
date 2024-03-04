using MediatR;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.GenericModel;

namespace PersonDictionary.Application.PersonService.Queries.GetPerson;

public sealed record GetPersonsSearchQuery(
    string? QuickSearch,
    string? FirstName,
    string? LastName,
    string? PersonalId,
    int? Page,
    int? PageSize) : IRequest<PagedResult<PersonModel>>;