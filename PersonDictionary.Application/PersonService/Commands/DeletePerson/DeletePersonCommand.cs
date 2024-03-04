using MediatR;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.DeletePerson;

public sealed record DeletePersonCommand(
    int Id
) : IRequest<DeleteResponseModel>;