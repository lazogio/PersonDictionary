using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.Models
{
    public class PhoneNumberModel
    {
        public string Number { get; set; }
        public PhoneNumberType NumberType { get; set; }
    }
}
