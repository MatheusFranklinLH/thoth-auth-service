using Flunt.Notifications;

namespace Thoth.Domain.Entities {
	public abstract class BaseEntity : Notifiable<Notification> {
		public int Id { get; private set; }
		public DateTime CreatedAt { get; private set; }
		public DateTime ModifiedAt { get; private set; }

		public void SetId(int id) => Id = id;
		public void SetCreatedAt(DateTime createdAt) => CreatedAt = createdAt;
		public void SetModifiedAt(DateTime modifiedAt) => ModifiedAt = modifiedAt;
	}
}
