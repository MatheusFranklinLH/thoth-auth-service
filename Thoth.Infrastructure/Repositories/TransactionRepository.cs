using Microsoft.EntityFrameworkCore.Storage;
using Thoth.Domain.Repositories;
using Thoth.Infrastructure.Context;

namespace Thoth.Infrastructure.Repositories {
	public class TransactionRepository : ITransactionRepository, IDisposable {
		private readonly ThothDbContext _context;
		private IDbContextTransaction _transaction;

		public TransactionRepository(ThothDbContext context) {
			_context = context;
		}

		public async Task BeginTransactionAsync() {
			_transaction = await _context.Database.BeginTransactionAsync();
		}

		public async Task CommitAsync() {
			if (_transaction is null)
				return;
			await _transaction.CommitAsync();
		}

		public async Task RollbackAsync() {
			if (_transaction is null)
				return;
			await _transaction.RollbackAsync();
		}
                public void Dispose() {
                        if (_transaction is null)
                                return;

                        // Use the synchronous Dispose method to avoid fire-and-forget async calls
                        // within the Dispose pattern.
                        _transaction.Dispose();
                }
        }
}
