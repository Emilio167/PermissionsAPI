namespace Infrastructure.Data.Interfaces
{
    public interface IUnitOfWork
    {
        Task CompleteAsync();
    }
}