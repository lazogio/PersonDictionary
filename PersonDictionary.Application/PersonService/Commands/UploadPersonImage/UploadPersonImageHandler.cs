using MediatR;
using PersonDictionary.Application.Interface;
using PersonDictionary.Domain.Constants;
using PersonDictionary.Domain.Interface;
using PersonDictionary.Domain.ResponseModel;

namespace PersonDictionary.Application.PersonService.Commands.UploadPersonImage;

public class UploadPersonImageHandler : IRequestHandler<UploadPersonImageCommand, UploadResultResponseModel>
{
    private readonly IPersonRepository _repository;
    private readonly IResourceManagerService _resourceManagerService;


    public UploadPersonImageHandler(IPersonRepository repository, IResourceManagerService resourceManagerService)
    {
        _repository = repository;
        _resourceManagerService = resourceManagerService;
    }

    public async Task<UploadResultResponseModel> Handle(UploadPersonImageCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.File == null || request.File.Length == 0)
            {
                var message = _resourceManagerService.GetString(ValidationMessages.NoFileIsSelected);
                throw new Exception(message);
            }

            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            var extension = Path.GetExtension(request.File.FileName);

            if (!allowedExtensions.Contains(extension.ToLower()))
            {
                throw new InvalidOperationException(
                    _resourceManagerService.GetString(ValidationMessages.InvalidFileType));
            }

            var person = await _repository.GetPersonByIdAsync(request.Id, cancellationToken);

            if (person is null)
            {
                throw new Exception($"Unable to upload image, person not found by Id: {request.Id}");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            var fileName = $"{person.FirstName}_{person.LastName}_{person.Id}.jpg";
            var path = Path.Combine(filePath, fileName);

            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            await using FileStream stream = new(path, FileMode.Create);
            await request.File.CopyToAsync(stream, cancellationToken);

            return new UploadResultResponseModel("Image uploaded successfully", 200);
        }
        catch (Exception e)
        {
            return new UploadResultResponseModel(e.Message, 500);
        }
    }
}