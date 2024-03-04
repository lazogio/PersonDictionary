using PersonDictionary.Application.PersonService.Commands.CreatePerson;
using PersonDictionary.Domain.Entities;

namespace PersonDictionary.Application.Interface
{
    public interface IDtoToEntityMapper
    {
        Person MapToEntity(CreatePersonCommand command);
    }
}
