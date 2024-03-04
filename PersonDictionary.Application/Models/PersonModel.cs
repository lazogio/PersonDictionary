﻿using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
        public Gender Gender { get; set; }
        public IEnumerable<PhoneNumberModel> PhoneNumbers { get; set; }
    }
}
