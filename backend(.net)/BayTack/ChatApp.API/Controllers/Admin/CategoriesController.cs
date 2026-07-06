using Microsoft.AspNetCore.Mvc;

namespace ChatApp.API.Controllers.Admin
{
	// Consumed by: Front_end/scripts/services/categoriesService.js (Admin > Categories page)
	// Also read publicly by the customer app service catalog.
	[ApiController]
	[Route("categories")]
	public class CategoriesController : ControllerBase
	{
		/// <summary>GET /categories -> Category[] { id, name, icon, description, isActive, createdAt }</summary>
		[HttpGet]
		public IActionResult GetAll()
			=> StatusCode(501, new { message = "Not implemented: GetAll categories" });

		/// <summary>GET /categories/{id} -> Category</summary>
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
			=> StatusCode(501, new { message = "Not implemented: GetById category" });

		/// <summary>POST /categories  Body: { name, icon, description } -> Category</summary>
		[HttpPost]
		public IActionResult Create([FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Create category" });

		/// <summary>PUT /categories/{id}  Body: { name, icon, description } -> Category</summary>
		[HttpPut("{id}")]
		public IActionResult Update(int id, [FromBody] object payload)
			=> StatusCode(501, new { message = "Not implemented: Update category" });

		/// <summary>DELETE /categories/{id} -> { success: true }</summary>
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
			=> StatusCode(501, new { message = "Not implemented: Delete category" });

		/// <summary>PATCH /categories/{id}/toggle -> Category (flips isActive)</summary>
		[HttpPatch("{id}/toggle")]
		public IActionResult Toggle(int id)
			=> StatusCode(501, new { message = "Not implemented: Toggle category" });
	}
}
