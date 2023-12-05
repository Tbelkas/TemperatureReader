using System.Net.Http.Headers;
using iSun.Domain.Services;
using Microsoft.Extensions.Logging;

namespace iSun.Domain.Handlers;

public class AuthorizedHttpHandler(IContextService contextService, ILogger<AuthorizedHttpHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = contextService.Token;
        if (string.IsNullOrEmpty(token))
        {
            // throw ex?
            logger.LogError("Attempting to use authorized http handler without a token");
            return await base.SendAsync(request, cancellationToken);
        }
        
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}