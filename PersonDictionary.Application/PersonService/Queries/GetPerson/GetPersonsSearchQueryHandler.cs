using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Application.Models;
using PersonDictionary.Domain.Entities;
using PersonDictionary.Domain.GenericModel;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Queries.GetPerson;

public class GetPersonsSearchQueryHandler : IRequestHandler<GetPersonsSearchQuery, PagedResult<PersonModel>>
{
    private readonly IPersonRepository _repository;
    private readonly IMapper<Person, PersonModel> _personMapper;

    public GetPersonsSearchQueryHandler(IPersonRepository repository, IMapper<Person, PersonModel> personMapper)
    {
        _repository = repository;
        _personMapper = personMapper;
    }

    public async Task<PagedResult<PersonModel>> Handle(GetPersonsSearchQuery searchCriteria,
        CancellationToken cancellationToken)
    {
        var sourcePagedResult = await _repository.SearchPersonsAsync(
            searchCriteria.QuickSearch,
            searchCriteria.FirstName,
            searchCriteria.LastName,
            searchCriteria.PersonalId,
            searchCriteria.Page,
            searchCriteria.PageSize,
            cancellationToken);

        var pagedResultPersonModel = new PagedResult<PersonModel>
        {
            TotalCount = sourcePagedResult.PageSize,
            Page = sourcePagedResult.Page,
            PageSize = sourcePagedResult.PageSize,
            Results = _personMapper.MapToModelList(sourcePagedResult.Results),
        };

        return pagedResultPersonModel;
    }
}