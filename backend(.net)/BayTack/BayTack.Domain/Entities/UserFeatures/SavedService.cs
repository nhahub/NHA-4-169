using BayTack.Domain.Common.BaseEntity;

namespace BayTack.Domain.Entities.UserFeatures
{
    /// <summary>
    /// A customer bookmarking a Service (not a provider - that's what Favorite is for).
    /// Backs Front_end/customer/app/saved (bt_c_saved), which stores an array of service ids.
    /// This entity didn't exist in the original model (only Favorite -> ProviderId did), so it's
    /// new; flagging that rather than silently repurposing Favorite for two different meanings.
    /// </summary>
    public class SavedService : BaseEntity<string>
    {
        public string CustomerId { get; private set; }
        public string ServiceId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        private SavedService() { }

        public static SavedService Create(string customerId, string serviceId) => new()
        {
            Id = Guid.NewGuid().ToString(),
            CustomerId = customerId,
            ServiceId = serviceId
        };
    }
}