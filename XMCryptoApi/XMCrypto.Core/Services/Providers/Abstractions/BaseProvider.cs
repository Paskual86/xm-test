
using XMCrypto.Core.Utils;
using XMCrypto.Domain.Enums;

namespace XMCrypto.Core.Services.Providers.Abstractions
{
    public abstract class BaseProvider
    {
        protected readonly IHttpClientFactory httpClientFactory;

        public string UrlProvider { get; private set; }

        protected string ClientApiName { get; init; }

        public BaseProvider(IHttpClientFactory httpCF, string urlProvider)
        {
            httpClientFactory = httpCF;
            UrlProvider = urlProvider;
            ClientApiName = string.Empty;
        }

        public virtual async Task<ExternalServiceStatus> GetStatusOfServiceAsync()
        {
            var client = httpClientFactory.CreateClient(ClientApiName);

            if (client == null)
            {
                // TODO: This should be replace by Internal Exception
                throw new Exception($"The Client {ClientApiName} is not configured");
            }

            string url = client.BaseAddress.AbsolutePath;

            var pingResponse = NetworkUtils.PingService(url);

            using (var response = await client.GetAsync(""))
            {
                if ((pingResponse == ExternalServiceStatus.Available) && (response.StatusCode == System.Net.HttpStatusCode.OK))
                    return ExternalServiceStatus.Available;
                else
                    return ExternalServiceStatus.NotAvailable;
            }
        }
    }
}
