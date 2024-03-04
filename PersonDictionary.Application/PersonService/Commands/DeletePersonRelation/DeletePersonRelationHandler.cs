using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Interface;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.DeletePersonRelation;

public class DeletePersonRelationHandler : IRequestHandler<DeletePersonRelationCommand, DeleteResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public DeletePersonRelationHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<DeleteResponseModel> Handle(DeletePersonRelationCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _repository.GetPersonByIdDetailedAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new(message);
        }

        var relatedPerson = person.RelatedPersons.FirstOrDefault(x => x.RelatedPersonId == request.RelatedPersonId);
        if (relatedPerson is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
            throw new(string.Format(message, request.RelatedPersonId));
        }

        person.RelatedPersons.Remove(relatedPerson);
        _repository.Delete(relatedPerson);

        await _unitOfWork.CommitAsync(cancellationToken);
        return new DeleteResponseModel("Person Relation deleted successfully.", 200);
    }
}