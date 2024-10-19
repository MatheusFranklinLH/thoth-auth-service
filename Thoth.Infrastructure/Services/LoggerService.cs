using Thoth.Domain.Interfaces;

namespace Thoth.Infrastructure.Services {
	public class LoggerService : ILoggerService {
		public void Insert(string message) {
			Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
		}
	}
}
