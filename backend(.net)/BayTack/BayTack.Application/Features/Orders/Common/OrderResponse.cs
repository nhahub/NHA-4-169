using System;

namespace BayTack.Application.Features.Orders.Common
{
	/// <summary>Shape expected by Front_end/customer/app/orders list (bt_c_orders).</summary>
	public sealed record OrderResponse(
		string Id,
		string ServiceId,
		string Title,
		string Provider,
		string? Avatar,
		decimal Price,
		string Status,
		int Progress,
		DateTime CreatedAt)
	{
		/// <summary>"active" | "completed" | "cancelled" bucket used by the ?status= filter.</summary>
		public static string StatusGroupOf(string status) => status switch
		{
			"Completed" => "completed",
			"Cancelled" => "cancelled",
			_ => "active" // Pending, Confirmed, InProgress, Disputed all read as "active" to the customer
		};

		public static int ProgressFor(string status) => status switch
		{
			"Pending" => 0,
			"Confirmed" => 25,
			"InProgress" => 60,
			"Disputed" => 50,
			"Completed" => 100,
			"Cancelled" => 0,
			_ => 0
		};
	}
}
