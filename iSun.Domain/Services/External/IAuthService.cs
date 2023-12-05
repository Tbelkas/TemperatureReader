using iSun.Domain.Models;
using iSun.Domain.Models.Responses;

namespace iSun.Domain.Services.External;

public interface IAuthService
{
    Task<Response<AuthorizeResponseModel>> GetToken(string username, string password);
}