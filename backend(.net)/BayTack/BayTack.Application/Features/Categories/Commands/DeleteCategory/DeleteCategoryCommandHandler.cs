using BayTack.Application.Abstractions.IRepository;
using BayTack.Application.Abstractions.Messaging;
using BayTack.Application.Common.Models;
using BayTack.Domain.Entities.ServiceAggregate;

namespace BayTack.Application.Features.Categories.Commands.DeleteCategory
{
	public sealed class DeleteCategoryCommandHandler : ICommandHandler<DeleteCategoryCommand>
	{
		private readonly IRepository<ServiceCategory, string> _categories;
		private readonly IUnitOfWork _unitOfWork;

		public DeleteCategoryCommandHandler(IRepository<ServiceCategory, string> categories, IUnitOfWork unitOfWork)
		{
			_categories = categories;
			_unitOfWork = unitOfWork;
		}

		public async Task<Result> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
		{
			var category = await _categories.GetByIdAsync(request.Id, cancellationToken);
			if (category is null)
				return Result.NotFound($"Category '{request.Id}' not found");

			_categories.Remove(category);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			return Result.Success();
		}
	}
}
