using MediatR;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.DeletePersonRelation;

public sealed record DeletePersonRelationCommand(
    int PersonId,
    int RelatedPersonId
) : IRequest<DeleteResponseModel>;
