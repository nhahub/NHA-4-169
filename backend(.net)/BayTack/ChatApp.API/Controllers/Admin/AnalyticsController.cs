using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/analyticsService.js (Admin > Analytics page)
	[ApiController]
	[Route("analytics")]
	public class AnalyticsController : ControllerBase
	{
		/// <summary>GET /analytics/kpis -> { totalRevenue, totalOrders, activeUsers, activeProviders, ... }</summary>
		[HttpGet("kpis")]
		public IActionResult Kpis()
			=> StatusCode(501, new { message = "Not implemented: KPIs" });

		/// <summary>GET /analytics/revenue-trend?period= -> { labels: string[], values: number[] }</summary>
		[HttpGet("revenue-trend")]
		public IActionResult RevenueTrend([FromQuery] string? period)
			=> StatusCode(501, new { message = "Not implemented: Revenue trend" });

		/// <summary>GET /analytics/categories -> { labels: string[], values: number[] } (revenue/orders share per category)</summary>
		[HttpGet("categories")]
		public IActionResult Categories()
			=> StatusCode(501, new { message = "Not implemented: Category breakdown" });

		/// <summary>GET /analytics/transactions/top?limit= -> Transaction[] { id, customer, provider, amount, date }</summary>
		[HttpGet("transactions/top")]
		public IActionResult TopTransactions([FromQuery] int limit = 10)
			=> StatusCode(501, new { message = "Not implemented: Top transactions" });
	}
}
