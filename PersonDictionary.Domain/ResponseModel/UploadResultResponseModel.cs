namespace PersonDictionary.Domain.ResponseModel;

public class UploadResultResponseModel
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public UploadResultResponseModel(string message, int statusCode)
    {
        Message = message;
        StatusCode = statusCode;
    }
}