using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Domain.Entities
{
    public class Person : Entity
    {
        public Person()
        {
            RelatedPersons = new List<PersonRelation>();
            RelatedToPersons = new List<PersonRelation>();
            PhoneNumbers = new List<PhoneNumber>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
        public Gender Gender { get; set; }
        public ICollection<PersonRelation> RelatedPersons { get; set; }
        public ICollection<PersonRelation> RelatedToPersons { get; set; }
        public ICollection<PhoneNumber> PhoneNumbers { get; set; }
     
    }
}