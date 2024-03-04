using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Domain.Entities
{
    public class PhoneNumber : Entity
    {
        public PhoneNumber()
        {
        }

        public string Number { get; set; }
        public PhoneNumberType NumberType { get; set; }
        public int PersonId { get; set; }
        public Person Person { get; set; }
    }
}