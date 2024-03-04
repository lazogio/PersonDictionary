using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Entities;

namespace PersonDictionary.Application.Mapping;

public class PersonRelationMapper : BaseMapper<PersonRelation, RelatedPersonsModel>
{
    public override RelatedPersonsModel MapToModel(PersonRelation entity)
    {
        return new RelatedPersonsModel
        {
            RelatedPersonId = entity.RelatedPersonId,
            RelationType = entity.RelationType
        };
    }

    public override PersonRelation MapToEntity(RelatedPersonsModel model)
    {
        return new PersonRelation
        {
            PersonId = model.RelatedPersonId,
            RelatedPersonId = model.RelatedPersonId,
            RelationType = model.RelationType
        };
    }

    public override List<RelatedPersonsModel> MapToModelList(IEnumerable<PersonRelation> entities)
    {
        return entities.Select(MapToModel).ToList();
    }

    public override List<PersonRelation> MapToEntityList(IEnumerable<RelatedPersonsModel> models)
    {
        return models.Select(MapToEntity).ToList();
    }
}