
namespace XMCrypto.Core.Services.Providers.Exceptions
{
    public class BTCProviderException : Exception
    {
        public const int CLIENT_API_NOT_CONFIGURED_CODE = 201401;
        public const int API_SERVICE_NOT_AVAILABLE = 201402;
        public int ExceptionCode { get; set; }

        public BTCProviderException():base()
        {

        }

        public BTCProviderException(string message, int code) : base(message)
        {
            ExceptionCode = code;
        }
    }
}
