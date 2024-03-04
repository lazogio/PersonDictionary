using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Interface;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.DeletePerson;

public class DeletePersonHandler : IRequestHandler<DeletePersonCommand, DeleteResponseModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;

    public DeletePersonHandler(IUnitOfWork unitOfWork, IPersonRepository repository,
        IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }


    public async Task<DeleteResponseModel> Handle(DeletePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetPersonByIdAsync(request.Id, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new Exception(message + $"{request.Id}");
        }

        _repository.Delete(person);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new DeleteResponseModel("Person deleted successfully.", 200);
    }
}