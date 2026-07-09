using BayTack.Application.Abstractions.Messaging;

namespace BayTack.Application.Features.Categories.Commands.CreateCategory
{
	public sealed record CreateCategoryCommand(string Name, string? Icon, string? Description) : ICommand<CategoryResponse>;
}
