namespace PersonDictionary.Domain.ResponseModel;

public class DeleteResponseModel
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public DeleteResponseModel(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}