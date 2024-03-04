using MediatR;
using Microsoft.AspNetCore.Http;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.UploadPersonImage;

public class UploadPersonImageCommand : IRequest<UploadResultResponseModel>
{
    public int Id { get; set; }
    public IFormFile? File { get; set; }
}