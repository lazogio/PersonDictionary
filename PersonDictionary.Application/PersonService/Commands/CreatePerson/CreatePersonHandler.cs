using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Commands.CreatePerson
{
    public class CreatePersonHandler : IRequestHandler<CreatePersonCommand, PersonModel>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPersonRepository _repository;
        private readonly IDtoToEntityMapper _dtoToEntityMapper;
        private readonly IMapper<Person, PersonModel> _personMapper;
        private readonly IResourceManagerService _resourceManagerService;

        public CreatePersonHandler(IPersonRepository repository, IUnitOfWork unitOfWork,
            IDtoToEntityMapper dtoToEntityMapper, IMapper<Person, PersonModel> personMapper,
            IResourceManagerService resourceManagerService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _dtoToEntityMapper = dtoToEntityMapper;
            _personMapper = personMapper;
            _resourceManagerService = resourceManagerService;
        }

        public async Task<PersonModel> Handle(CreatePersonCommand request, CancellationToken cancellationToken)
        {
            var existingPerson = await _repository.GetPersonByPersonalIdAsync(request.PersonalId, cancellationToken);
            var message = _resourceManagerService.GetString(ValidationMessages.PersonWithPersonalIdAlreadyExists);

            if (existingPerson is not null)
            {
                throw new Exception(message + $"{request.PersonalId}");
            }

            var person = _dtoToEntityMapper.MapToEntity(request);

            await _repository.InsertAsync(person, cancellationToken);
            await _unitOfWork.CommitAsync(cancellationToken);

            return _personMapper.MapToModel(person);
        }
    }
}