using Microsoft.EntityFrameworkCore;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.GenericModel;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Persistence
{
    public class PersonRepository : IPersonRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PersonRepository(ApplicationDbContext context)
        {
            _dbContext = context;
        }

        public async Task<Person?> GetPersonByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons.FindAsync(id, cancellationToken);
        }

        public async Task<Person?> GetPersonByIdDetailedAsync(int id, CancellationToken cancellationToken)
        {
            return await _dbContext.Persons
                .Include(p => p.RelatedToPersons)
                .ThenInclude(rp => rp.Person)
                .Include(p => p.RelatedPersons)
                .ThenInclude(rp => rp.RelatedPerson)
                .Include(p => p.PhoneNumbers)
                .SingleOrDefaultAsync(p => p.Id == id, cancellationToken);
        }

        public async Task<Person?> GetPersonByPersonalIdAsync(string personId,
            CancellationToken cancellationToken = default)
        {
            return await _dbContext.Persons
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.PersonalId == personId, cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task InsertAsync(Person entity, CancellationToken cancellationToken = default)
        {
            await _dbContext.Persons.AddAsync(entity, cancellationToken);
        }

        public void Delete(Person entity)
        {
            _dbContext.Persons.Remove(entity);
        }

        public void Delete(PersonRelation entity)
        {
            _dbContext.PersonRelations.Remove(entity);
        }

        public void Update(Person entity)
        {
            _dbContext.Persons.Update(entity);
        }

        public async Task<PagedResult<Person>> SearchPersonsAsync(string? quickSearch, string? firstName,
            string? lastName, string? personalId, int? page, int? pageSize, CancellationToken cancellationToken)
        {
            if (_dbContext is null)
            {
                throw new ArgumentNullException(nameof(_dbContext), @"DbContext cannot be null.");
            }

            IQueryable<Person> query = _dbContext.Persons
                .AsNoTracking()
                .Include(x => x.PhoneNumbers);

            query = query
                .Where(p => (string.IsNullOrEmpty(quickSearch) ||
                             EF.Functions.Like(p.FirstName, $"%{quickSearch}%") ||
                             EF.Functions.Like(p.LastName, $"%{quickSearch}%") ||
                             EF.Functions.Like(p.PersonalId, $"%{quickSearch}%"))
                )
                .Where(p => (string.IsNullOrEmpty(firstName) || p.FirstName.Contains(firstName)))
                .Where(p => (string.IsNullOrEmpty(lastName) || p.LastName.Contains(lastName)))
                .Where(p => (string.IsNullOrEmpty(personalId) || p.PersonalId.Contains(personalId)));

            var totalCount = await query.CountAsync(cancellationToken);

            if (page.HasValue && pageSize.HasValue)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            var results = await query.ToListAsync(cancellationToken);

            return new PagedResult<Person>
            {
                TotalCount = totalCount,
                Page = page ?? 1,
                PageSize = pageSize ?? results.Count,
                Results = results
            };
        }

        public Task<IQueryable<PersonRelationModel>> GetPersonsRelationsAsync(CancellationToken cancellationToken = default)
        {
            var query = _dbContext.PersonRelations
                .AsNoTracking()
                .Include(p => p.Person)
                .GroupBy(x => new
                {
                    x.PersonId,
                    x.RelationType,
                    x.Person.FirstName,
                    x.Person.LastName,
                    x.Person.Gender
                })
                .Select(c => new
                {
                    c.Key,
                    Count = c.Count()
                })
                .OrderBy(m => m.Key.PersonId)
                .ThenBy(m => m.Key.RelationType)
                .Select(s => new PersonRelationModel
                {
                    Id = s.Key.PersonId,
                    FirstName = s.Key.FirstName,
                    LastName = s.Key.LastName,
                    Gender = s.Key.Gender,
                    RelationType = s.Key.RelationType,
                    Count = s.Count
                });

            return Task.FromResult(query);
        }

    }
}