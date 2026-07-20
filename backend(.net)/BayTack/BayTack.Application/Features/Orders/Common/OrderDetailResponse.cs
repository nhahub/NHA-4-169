using System;
using System.Collections.Generic;

namespace BayTack.Application.Features.Orders.Common
{
	/// <summary>A single status-history entry, oldest first.</summary>
	public sealed record OrderHistoryEntry(string Status, DateTime ChangedAt, string ChangedBy);

	/// <summary>Shape expected by Front_end/customer/app/orders/{id} detail view (bt_c_orders).</summary>
	public sealed record OrderDetailResponse(
		string Id,
		string ServiceId,
		string Title,
		string Description,
		string Provider,
		string? Avatar,
		decimal Price,
		string Currency,
		string Status,
		int Progress,
		DateTime CreatedAt,
		DateTime StartDate,
		DateTime? EndDate,
		IReadOnlyList<OrderHistoryEntry> History);
}
