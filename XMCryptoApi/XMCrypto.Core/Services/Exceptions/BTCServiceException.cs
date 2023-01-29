
namespace XMCrypto.Core.Services.Exceptions
{
    public class BTCServiceException : Exception
    {
        public const int PROVIDER_NOT_FOUND = 198601;

        public int ExceptionCode { get; set; }
        public BTCServiceException():base()
        {

        }

        public BTCServiceException(string message, int code): base(message)
        {
            ExceptionCode = code;
        }
    }
}
