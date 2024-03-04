using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Commands.CreatePersonRelation;

public class CreatePersonRelationHandler : IRequestHandler<CreatePersonRelationCommand, RelatedPersonsModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IMapper<PersonRelation, RelatedPersonsModel> _relatedPersonsMapper;
    private readonly IResourceManagerService _resourceManagerService;


    public CreatePersonRelationHandler(IPersonRepository repository,
        IMapper<PersonRelation, RelatedPersonsModel> personMapper,
        IResourceManagerService resourceManagerService, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _relatedPersonsMapper = personMapper;
        _resourceManagerService = resourceManagerService;
        _unitOfWork = unitOfWork;
    }

    public async Task<RelatedPersonsModel> Handle(CreatePersonRelationCommand request,
        CancellationToken cancellationToken)
    {
        var person = await _repository.GetPersonByIdDetailedAsync(request.PersonId, cancellationToken);
        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new Exception(message + $"{request.PersonId}");
        }

        var personRelated = await _repository.GetPersonByIdDetailedAsync(request.RelatedPersonId, cancellationToken);
        if (personRelated is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.RelatedPersonNotFoundById);
            throw new Exception(message + $"{request.PersonId}");
        }

        var personRelation = new PersonRelation(person, personRelated, request.RelationType);

        person.RelatedPersons.Add(personRelation);
        personRelated.RelatedToPersons.Add(personRelation);

        _repository.Update(personRelated);

        await _unitOfWork.CommitAsync(cancellationToken);

        return _relatedPersonsMapper.MapToModel(personRelation);
    }
}