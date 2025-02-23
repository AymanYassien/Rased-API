namespace Rased.Infrastructure.UnitsOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        // All System IServices ..
        // IAuthService RasedAuth { get; }
        // ....

        // Commit Changes
        Task<int> CommitChangesAsync();
    }
}
