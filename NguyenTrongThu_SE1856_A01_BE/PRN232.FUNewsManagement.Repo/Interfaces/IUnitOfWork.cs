namespace PRN232.FUNewsManagement.Repo.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ISystemAccountRepository SystemAccounts { get; }
        ICategoryRepository Categories { get; }
        INewsArticleRepository NewsArticles { get; }
        ITagRepository Tags { get; }
        
        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
