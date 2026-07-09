using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Application.Features.Categories.Specifications;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Commands.UpdateCategory
{
	public sealed class UpdateCategoryCommandHandler : ICommandHandler<UpdateCategoryCommand, CategoryResponse>
	{
		private readonly IRepository<ServiceCategory, string> _categories;
		private readonly IUnitOfWork _unitOfWork;

		public UpdateCategoryCommandHandler(IRepository<ServiceCategory, string> categories, IUnitOfWork unitOfWork)
		{
			_categories = categories;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result<CategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
		{
			var category = await _categories.GetByIdAsync(request.Id, cancellationToken);
			if (category is null)
				return Result<CategoryResponse>.NotFound($"Category '{request.Id}' not found");

			if (!string.IsNullOrWhiteSpace(request.Name) && !request.Name.Equals(category.Name, StringComparison.OrdinalIgnoreCase))
			{
				var nameTaken = await _categories.AnyAsync(new CategoryByNameSpecification(request.Name), cancellationToken);
				if (nameTaken)
					return Result<CategoryResponse>.Failure($"Category '{request.Name}' already exists", statusCode: 409);
			}

			// TODO: pass the real signed-in user id once JWT auth (see Program.cs note) is wired up.
			category.UpdateDetails(request.Name, request.Icon, request.Description, updatedBy: null);
			_categories.Update(category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result<CategoryResponse>.Success(
				new CategoryResponse(category.Id, category.Name, category.Icon, category.Description, category.IsActive, category.CreatedAt));
		}
	}
}
