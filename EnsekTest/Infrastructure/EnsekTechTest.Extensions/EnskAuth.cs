using EnsekTechTest.Abstractions;
using EnsekTechTest.Abstractions.Errors;
using EnsekTechTest.TestData.POCOS;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace EnsekTechTest.Extensions;

public static class EnskAuth
{
    public static async Task<(OutcomeResult Result, HttpResponseMessage HttpResponse)> ValidateLogin(this Login login, string tokenEndpoint)
    {
        var loginPayload = new
        {
            username = login.Username,
            password = login.Password
        };

        var json = JsonSerializer.Serialize(loginPayload);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var client = new HttpClient();
        var response = await client.PostAsync(tokenEndpoint, content);

        return response.StatusCode switch
        {
            System.Net.HttpStatusCode.OK => (OutcomeResult.Success(), response),
            System.Net.HttpStatusCode.BadRequest => ((OutcomeResult Result, HttpResponseMessage HttpResponse))(HttpCodeErrors.BadRequest, response),
            System.Net.HttpStatusCode.Unauthorized => ((OutcomeResult Result, HttpResponseMessage HttpResponse))(HttpCodeErrors.UnauthorisedLogin, response),
            _ => throw new NotImplementedException($"{response.StatusCode} - is not a valid Response Code"),
        };
    }
    public static async Task<string> GetToken(this Login login, string tokenEndpoint)
    {
        Login validLogin = new()
        {
            Username = login.Username,
            Password = login.Password
        };
        (OutcomeResult Outcome, HttpResponseMessage httpResponse) response = await validLogin.ValidateLogin(tokenEndpoint);

        var responseBody = await response.httpResponse.Content.ReadAsStringAsync();
        var json = JObject.Parse(responseBody);
        return json["access_token"]?.ToString() ?? string.Empty;
    }
}
