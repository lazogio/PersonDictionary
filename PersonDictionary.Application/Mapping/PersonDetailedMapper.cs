using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Entities;

namespace PersonDictionary.Application.Mapping;

public class PersonDetailedMapper : BaseMapper<Person, PersonDetailedModel>
{
        public override PersonDetailedModel MapToModel(Person? entity)
        {
            if (entity is null)
            {
                return null!;
            }

            return new PersonDetailedModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                PersonalId = entity.PersonalId,
                BirthDate = entity.BirthDate,
                CityId = entity.CityId,
                Gender = entity.Gender,
                RelatedPersons = entity.RelatedPersons?.Select(MapToPersonRelationModel).ToList(),
                RelatedToPersons = entity.RelatedToPersons?.Select(MapToPersonRelationModel).ToList(),
                PhoneNumbers = entity.PhoneNumbers?.Select(MapToPhoneNumberModel).ToList(),
            };
        }

        public override Person MapToEntity(PersonDetailedModel? model)
        {
            if (model is null)
            {
                return null!;
            }

            return new Person
            {
                CreatedDate = DateTime.Now,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PersonalId = model.PersonalId,
                BirthDate = model.BirthDate,
                CityId = model.CityId,
                Gender = model.Gender
            };
        }

        private RelatedPersonsModel MapToPersonRelationModel(PersonRelation? relation)
        {
            if (relation is null)
            {
                return null!;
            }

            return new RelatedPersonsModel
            {
                RelatedPersonId = relation.RelatedPersonId,
                RelationType = relation.RelationType
            };
        }

        private PhoneNumberModel MapToPhoneNumberModel(PhoneNumber? phoneNumber)
        {
            if (phoneNumber is null)
            {
                return null!;
            }

            return new PhoneNumberModel
            {
                Number = phoneNumber.Number,
                NumberType = phoneNumber.NumberType
            };
        }

        public override List<PersonDetailedModel> MapToModelList(IEnumerable<Person> entities)
        {
            return entities.Select(MapToModel).ToList();
        }

        public override List<Person> MapToEntityList(IEnumerable<PersonDetailedModel> models)
        {
            return models.Select(MapToEntity).ToList();
        }
}