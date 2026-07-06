using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Provider
{
	// Consumed by: Front_end/provider/app/portfolio/scripts/core/api.js + services/portfolioService.js
	[ApiController]
	[Route("portfolio/items")]
	public class PortfolioController : ControllerBase
	{
		/// <summary>GET /portfolio/items -> PortfolioItem[] { id, title, description, price, images[] }</summary>
		[HttpGet]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll portfolio items" });

		/// <summary>GET /portfolio/items/{id} -> PortfolioItem</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(string id)
			=> StatusCode(501, new { message = "Not implemented: GetById portfolio item" });

		/// <summary>POST /portfolio/items  Body: { title, description, price } -> PortfolioItem</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create portfolio item" });

		/// <summary>PUT /portfolio/items/{id}  Body: { title, description, price } -> PortfolioItem</summary>
		[HttpPut("{id}")]
		public IActionResult Update(string id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update portfolio item" });

		/// <summary>DELETE /portfolio/items/{id} -> { success: true }</summary>
		[HttpDelete("{id}")]
		public IActionResult Delete(string id)
			=> StatusCode(501, new { message = "Not implemented: Delete portfolio item" });

		/// <summary>
		/// POST /portfolio/items/{id}/images  (multipart/form-data, field name "images", max per APP_CONFIG.portfolio.maxImages)
		/// Response: { urls: string[] }
		/// </summary>
		[HttpPost("{id}/images")]
		public IActionResult UploadImages(string id, [FromForm] IFormFileCollection images)
			=> StatusCode(501, new { message = "Not implemented: Upload portfolio images" });
	}
}
