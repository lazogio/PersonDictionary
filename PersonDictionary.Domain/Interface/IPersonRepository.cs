using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.GenericModel;

namespace PersonDictionary.Domain.Interface
{
    public interface IPersonRepository
    {
        Task<Person?> GetPersonByIdAsync(int id, CancellationToken cancellationToken);
        Task<Person?> GetPersonByIdDetailedAsync(int id, CancellationToken cancellationToken);
        Task<Person?> GetPersonByPersonalIdAsync(string personalId, CancellationToken cancellationToken);
        Task InsertAsync(Person entity, CancellationToken cancellationToken);
        void Delete(Person entity);
        void Delete(PersonRelation entity);
        void Update(Person entity);

        Task<PagedResult<Person>> SearchPersonsAsync(string? quickSearch,
            string? firstName,
            string? lastName,
            string? personalId,
            int? page,
            int? pageSize,
            CancellationToken cancellationToken);
      Task<IQueryable<PersonRelationModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken);  
    }
    
    
}