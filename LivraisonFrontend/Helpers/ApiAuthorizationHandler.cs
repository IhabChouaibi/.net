using System.Net.Http.Headers;

namespace LivraisonFrontend.Helpers;

public class ApiAuthorizationHandler : DelegatingHandler
{
    private readonly SessionManager _sessionManager;

    public ApiAuthorizationHandler(SessionManager sessionManager)
    {
        _sessionManager = sessionManager;
    }

    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(_sessionManager.Token))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _sessionManager.Token);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
