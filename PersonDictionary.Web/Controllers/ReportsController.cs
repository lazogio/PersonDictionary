using MediatR;
using Microsoft.AspNetCore.Mvc;
using PersonDictionary.Application.PersonService.Queries.GetPersonRelation;
using PersonDictionary.Domain.GenericModel;

namespace PersonDictionary.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ISender _sender;

    public ReportsController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IQueryable<PersonRelationModel>), StatusCodes.Status200OK)]
    public async Task<IActionResult> RelatedPersons([FromQuery] GetPersonsRelationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await _sender.Send(request, cancellationToken);
        return Ok(result);
    }
}