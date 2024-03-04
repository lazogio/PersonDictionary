using MediatR;
using PersonDictionary.Domain.GenericModel;

namespace PersonDictionary.Application.PersonService.Queries.GetPersonRelation;

public sealed record GetPersonsRelationsQuery() : IRequest<IQueryable<PersonRelationModel>>;