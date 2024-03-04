using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Commands.UpdatePerson;

public class UpdatePersonHandler : IRequestHandler<UpdatePersonCommand, PersonModel>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPersonRepository _repository;
    private readonly IMapper<Person, PersonModel> _personMapper;
    private readonly IResourceManagerService _resourceManagerService;


    public UpdatePersonHandler(IUnitOfWork unitOfWork, IPersonRepository repository, IMapper<Person, PersonModel> personMapper,
        IResourceManagerService resourceManagerService)
    {
        _unitOfWork = unitOfWork;
        _repository = repository;
        _personMapper = personMapper;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<PersonModel> Handle(UpdatePersonCommand request, CancellationToken cancellationToken)
    {
        var person = await _repository.GetPersonByIdDetailedAsync(request.Id, cancellationToken);

        if (person is null)
        {
            var message = _resourceManagerService.GetString(ValidationMessages.PersonNotFoundById);
            throw new Exception(message + $"{request.Id}");
        }

        person.FirstName = request.FirstName;
        person.LastName = request.LastName;
        person.PersonalId = request.PersonalId;
        person.Gender = request.Gender;
        person.CityId = request.CityId;
        person.BirthDate = request.BirthDate;
        person.UpdatedDate = DateTime.Now;

        foreach (var phoneNumberModel in request.PhoneNumbers.OfType<UpdatePhoneNumberModel>())
        {
            var phoneNumber = phoneNumberModel;
            var dbPhoneNumber = person.PhoneNumbers.FirstOrDefault(x => x.Id == phoneNumber.Id);

            if (dbPhoneNumber != null)
            {
                dbPhoneNumber.Number = phoneNumber.Number;
                dbPhoneNumber.NumberType = phoneNumber.NumberType;
                dbPhoneNumber.UpdatedDate = DateTime.Now;
            }
        }

        _repository.Update(person);
        await _unitOfWork.CommitAsync(cancellationToken);

        return _personMapper.MapToModel(person);
    }
}