namespace EnsekTechTest.Abstractions.Errors;

public static class HttpCodeErrors
{
    public static readonly IsError BadRequest =
        new IsError("Http Bad Request - The Request returned a 400 Bad Request");
    public static readonly IsError UnauthorisedLogin =
        new IsError("Invalid Login - Invalid credentials entered, please check and retry");
    public static readonly IsError BadRequestLogin =
        new IsError("Bad Request Login - Invalid credentials entered, please check and retry");
}
