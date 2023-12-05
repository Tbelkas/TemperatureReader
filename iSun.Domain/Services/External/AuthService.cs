using System.Net.Http.Json;
using System.Text.Json;
using iSun.Domain.Models;
using iSun.Domain.Models.Requests;
using iSun.Domain.Models.Responses;
using Microsoft.Extensions.Logging;

namespace iSun.Domain.Services.External;

public class AuthService(IHttpClientFactory clientFactory, ILogger<AuthService> logger) : IAuthService
{
    private readonly HttpClient _client = clientFactory.CreateClient();

    public async Task<Response<AuthorizeResponseModel>> GetToken(string username, string password)
    {
        var requestModel = new AuthorizeRequestModel(username, password);
        var json = JsonSerializer.Serialize(requestModel);
        var httpContent = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

        var response = await _client.PostAsync("authorize", httpContent);

        if (!response.IsSuccessStatusCode)
        {
            return Response<AuthorizeResponseModel>.Error("Something went wrong");
        }

        var result = await response.Content.ReadFromJsonAsync<AuthorizeResponseModel>();
        if (string.IsNullOrEmpty(result?.Token))
        {
            return Response<AuthorizeResponseModel>.Error("Token not in body");
        }
        
        return Response<AuthorizeResponseModel>.Success(result);
    }
}