
namespace XMCrypto.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
