using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.Models
{
    public class RelatedPersonsModel
    {
        public int RelatedPersonId { get; set; }
        public RelationType RelationType { get; set; }
    }
}
