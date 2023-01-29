

namespace XMCrypto.Domain.Exceptions
{
    public class PersistanceException : Exception
    {
        public const int ARGUMENTS_NULL = 201601;
        public const int ADDING_ERROR = 201602;
        public const int COMMIT_ERROR = 201603;

        public int ExceptionCode { get; set; }

        public PersistanceException() : base()
        {

        }

        public PersistanceException(string message, int code) : base(message)
        {
            ExceptionCode = code;
        }
    }
}
