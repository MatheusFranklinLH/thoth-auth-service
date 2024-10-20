
namespace Thoth.Domain.Repositories {
	public interface ITransactionRepository : IDisposable {
		public Task BeginTransactionAsync();
		public Task CommitAsync();
		public Task RollbackAsync();
	}
}
