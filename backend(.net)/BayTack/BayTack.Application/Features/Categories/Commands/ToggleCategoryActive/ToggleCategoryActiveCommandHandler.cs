using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Commands.ToggleCategoryActive
{
	public sealed class ToggleCategoryActiveCommandHandler : ICommandHandler<ToggleCategoryActiveCommand, CategoryResponse>
	{
		private readonly IRepository<ServiceCategory, string> _categories;
		private readonly IUnitOfWork _unitOfWork;

		public ToggleCategoryActiveCommandHandler(IRepository<ServiceCategory, string> categories, IUnitOfWork unitOfWork)
		{
			_categories = categories;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CategoryResponse>> Handle(ToggleCategoryActiveCommand request, CancellationToken cancellationToken)
		{
			var category = await _categories.GetByIdAsync(request.Id, cancellationToken);
			if (category is null)
				return Result<CategoryResponse>.NotFound($"Category '{request.Id}' not found");

			category.ToggleActive(updatedBy: null); // TODO: wire real current-user once JWT auth exists
			_categories.Update(category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<CategoryResponse>.Success(
				new CategoryResponse(category.Id, category.Name, category.Icon, category.Description, category.IsActive, category.CreatedAt));
		}
	}
}
