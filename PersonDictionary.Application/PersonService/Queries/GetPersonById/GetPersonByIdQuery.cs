using MediatR;
using PersonDictionary.Application.Models;

namespace PersonDictionary.Application.PersonService.Queries.GetPersonById;

public sealed record GetPersonByIdQuery(
    int PersonId
) : IRequest<PersonDetailedModel>;