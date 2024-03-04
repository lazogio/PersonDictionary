using System.ComponentModel.DataAnnotations;

namespace PersonDictionary.Domain.Entities
{
    public abstract class Entity
    {
        [Key] 
        public int Id { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}