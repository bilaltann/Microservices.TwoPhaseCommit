namespace Coordinator.Services.Abstractions
{
    public interface ITransactionService
    {
        Task<Guid> CreateTransactionAsync();

        Task PrepareServiceAsync(Guid transactionId);

        Task<bool> CheckReadyServiceAsync(Guid transactionId);

        Task CommitAsync(Guid transactionId);

        Task<bool> CheckTransactionStateServiceAsync(Guid transactionId);

        Task RollbackAsync(Guid transactionId);
    }
}
