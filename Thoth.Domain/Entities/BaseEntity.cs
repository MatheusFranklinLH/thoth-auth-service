using Flunt.Notifications;

namespace Thoth.Domain.Entities {
	public abstract class BaseEntity : Notifiable<Notification> {
		public int Id { get; set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime ModifiedAt { get; private set; }
	}
}
