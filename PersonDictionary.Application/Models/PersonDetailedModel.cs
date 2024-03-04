﻿using PersonDictionary.Domain.Enums;

namespace PersonDictionary.Application.Models
{
    public class PersonDetailedModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalId { get; set; }
        public DateTime BirthDate { get; set; }
        public int CityId { get; set; }
        public Gender Gender { get; set; }
        public virtual ICollection<RelatedPersonsModel> RelatedPersons { get; set; }
        public virtual ICollection<RelatedPersonsModel> RelatedToPersons { get; set; }
        public virtual ICollection<PhoneNumberModel> PhoneNumbers { get; set; }

    }
}
