using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Queries.GetPersonById;

public class GetPersonByIdHandler : IRequestHandler<GetPersonByIdQuery, PersonDetailedModel>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper<Person, PersonDetailedModel> _personMapper;
    private readonly IResourceManagerService _resourceManagerService;

    public GetPersonByIdHandler(IPersonRepository repository, IMapper<Person, PersonDetailedModel> personMapper,
        IResourceManagerService resourceManagerService)
    {
        _repository = repository;
        _personMapper = personMapper;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<PersonDetailedModel> Handle(GetPersonByIdQuery request, CancellationToken cancellationToken)
    {
        var personal = await _repository.GetPersonByIdDetailedAsync(request.PersonId, cancellationToken);
        if (personal is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new Exception(message + $"{request.PersonId}");
        }

        return _personMapper.MapToModel(personal);
    }
}