using MediatR;
using PersonDictionary.Domain.GenericModel;
using PersonDictionary.Domain.Interface;

namespace PersonDictionary.Application.PersonService.Queries.GetPersonRelation;

public class GetPersonsRelationsHandler : IRequestHandler<GetPersonsRelationsQuery, IQueryable<PersonRelationModel>>
{
    private readonly IPersonRepository _repository;

    public GetPersonsRelationsHandler(IPersonRepository repository)
    {
        _repository = repository;
    }

    public async Task<IQueryable<PersonRelationModel>> Handle(GetPersonsRelationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _repository.GetPersonsRelationsAsync(cancellationToken);
        return result;
    }
}