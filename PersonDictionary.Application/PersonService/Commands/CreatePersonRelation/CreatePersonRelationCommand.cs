using MediatR;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.PersonService.Commands.CreatePersonRelation;

public sealed record CreatePersonRelationCommand(
    int PersonId,
    int RelatedPersonId,
    RelationType RelationType
) : IRequest<RelatedPersonsModel>;