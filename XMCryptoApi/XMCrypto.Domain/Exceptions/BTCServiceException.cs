

namespace XMCrypto.Domain.Exceptions
{
    /// <summary>
    /// Manage Exceptions for the BTC Service
    /// </summary>
    public class BTCServiceException : Exception
    {
        public const int PROVIDER_NOT_FOUND = 198601;
        public const int INTERNAL_ERROR = 198602;
        public const int ERROR_SAVING_INFO_STORE = 198603;
        public const int NO_PROVIDERS_IMPLEMENTATION = 198604;

        public int ExceptionCode { get; set; }
        public BTCServiceException() : base()
        {

        }

        public BTCServiceException(string message, int code) : base(message)
        {
            ExceptionCode = code;
        }
    }
}
